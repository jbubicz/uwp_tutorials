using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using TrainingPlatform.Models;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using winsdkfb;
using winsdkfb.Graph;

namespace TrainingPlatform
{
    public sealed partial class ViewCourse : Page
    {
        static FBSession clicnt = FBSession.ActiveSession;
        private User user = getUserInfo(clicnt);
        private int course_id;
        private ObservableCollection<CategoriesList> categories = Database.getCategories("courses_categories");
        private ObservableCollection<SectionsList> sections;
        private ObservableCollection<Section> sections_with_lessons = new ObservableCollection<Section>();
        private ObservableCollection<Lesson> lessons;
        private ObservableCollection<FBUserRootobject> friendsList;
        private double value;

        public ViewCourse()
        {
            this.InitializeComponent();
            CatCombo.ItemsSource = categories;
            //CatCombo.SelectedItem = categories[0];
            if (clicnt.LoggedIn)
            {
                App.IsLogged = true;
            }
            GetFriends();
            friendsList = new ObservableCollection<FBUserRootobject>();
        }

        private static User getUserInfo(FBSession clicnt)
        {
            if (clicnt.LoggedIn)
            {
                User user = new User();
                user = user.fbUser(clicnt.User.Id);
                return user;
            }
            else
                return null;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var parameters = (Course)e.Parameter;
            lstGroup.DataContext = parameters;
            //SectionList.DataContext = sections;
            course_id = Int32.Parse(CourseId.Text);
            int actual_category_id = parameters.Category;
            CatCombo.SelectedItem = categories[actual_category_id - 1];
            sections = getSections();
            //getLessons();
            getSectionsWithLessons(sections);
            SectionsLessonsList.ItemsSource = sections_with_lessons;
            SectionList.ItemsSource = sections;
            setControlsVisibility(course_id);
        }

        private void getSectionsWithLessons(ObservableCollection<SectionsList> sections)
        {
            if (sections.Count != 0)
            {
                Section s2 = new Section();
                s2.lessons = Database.getLessons(course_id);
                if (s2.Id==0 && s2.lessons.Count!=0)
                {
                    sections_with_lessons.Add(s2);
                }
                foreach (var section in sections)
                {
                    Section s = new Section();
                    s = section;
                    s.lessons = Database.getLessons(s.Course_id, s.Id);
                    sections_with_lessons.Add(s);
                }
                
            }
            else
            {
                Section s = new Section();               
                s.lessons = Database.getLessons(course_id);
                sections_with_lessons.Add(s);
            }
        }


        private void setControlsVisibility(int course_id)
        {
            setDefaultVisibility();
            if (Double.TryParse(Price_textBlock.Text, out value))
            {
                Price_textBlock.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture, "{0:C2}", value);
            }
            else
            {
                Price_textBlock.Text = String.Empty;
            }
            //if (double.Parse(Rating_textBlock.Text) < 1)
            //{
            //    Rating_textBlock.Visibility = Visibility.Collapsed;
            //}
            if (clicnt.LoggedIn)
            {
                Rating.Visibility = Visibility.Visible;
                int userRate = Database.getUserRate(user.Id, course_id);
                switch (userRate)
                {
                    case 0:
                        clearRating();
                        break;
                    case 1:
                        rb1.IsChecked = true;
                        break;
                    case 2:
                        rb2.IsChecked = true;
                        break;
                    case 3:
                        rb3.IsChecked = true;
                        break;
                    case 4:
                        rb4.IsChecked = true;
                        break;
                    case 5:
                        rb5.IsChecked = true;
                        break;
                    default:
                        clearRating();
                        break;
                }
                Edit_button.Visibility = Visibility.Visible;
                checkIfFBUserIsSigned();
            }
        }

        private void checkIfFBUserIsSigned()
        {
            bool isSigned = Database.checkIfFBUserIsSigned(clicnt.User.Id, course_id);
            if (isSigned)
            {
                SignupButton.Visibility = Visibility.Collapsed;
                SignoffButton.Visibility = Visibility.Visible;
            }
        }

        private void StartUpload_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            CatCombo.Visibility = Visibility.Visible;
            StartUploadButton.Visibility = Visibility.Visible;
            Title_textBlock.Visibility = Visibility.Collapsed;
            Title_textBox.Visibility = Visibility.Visible;
            ShortDesc_textBlock.Visibility = Visibility.Collapsed;
            ShortDesc_textBox.Visibility = Visibility.Visible;
            Desc_textBlock.Visibility = Visibility.Collapsed;
            Desc_textBox.Visibility = Visibility.Visible;
            Price_textBlock.Visibility = Visibility.Collapsed;
            Price_textBox.Visibility = Visibility.Visible;
            AddCourseSections.Visibility = Visibility.Visible;
            Edit_button.Visibility = Visibility.Collapsed;
            Save_button.Visibility = Visibility.Visible;
            Cancel_button.Visibility = Visibility.Visible;
            SignupButton.Visibility = Visibility.Collapsed;
            SignoffButton.Visibility = Visibility.Collapsed;
            Rating.Visibility = Visibility.Collapsed;

        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            Course course = null;
            course = getEditedCourseDetails();
            int user_id = course.UserId;
            int cat_id = course.Category;
            string title = course.Title;
            string price = course.Price;
            string img = course.ImgUrl;
            string short_desc = course.ShortDescription;
            string desc = course.Description;
            if (!addSectionsToDatabase(sections))
            {
                string error = "Error adding sections!";
                MessageDialog dialog = new MessageDialog(error);
                await dialog.ShowAsync();
            }
            bool updated = Database.updateCourse("courses", course_id, user_id, cat_id, title, price, img, short_desc, desc);
            if (updated)
            {
                string success = "Course successfully edited!";
                MessageDialog dialog = new MessageDialog(success);
                await dialog.ShowAsync();
            }
            Frame.Navigate(typeof(ViewCourse), course);
        }

        private bool addSectionsToDatabase(ObservableCollection<SectionsList> sections)
        {
            if (sections != null)
            {
                int i = 1;
                foreach (var section in sections)
                {
                    section.Section_order = i;
                    i++;
                }
                try
                {
                    Database.insertSection(sections, course_id);
                    return true;
                }
                catch (Exception)
                {
                    Debug.WriteLine("Error inserting course section");
                    return false;
                }
            }
            return false;
        }

        private Course getEditedCourseDetails()
        {
            var cat = CatCombo.SelectedItem as CategoriesList;
            int cat_id = cat.Id;
            string title = Title_textBox.Text;
            string short_desc = ShortDesc_textBox.Text;
            string desc = Desc_textBox.Text;
            string price = Price_textBox.Text;
            string img = "Assets/course.jpg";
            Course edited_course =
                new Course { Id = course_id, UserId = user.Id, Category = cat_id, Title = title, Price = price, ImgUrl = img, ShortDescription = short_desc, Description = desc, IsEnabled = 1 };
            return edited_course;
        }

        private ObservableCollection<SectionsList> getSections()
        {
            return sections = Database.getSections(course_id);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            sections.Clear();
            setControlsVisibility(course_id);
        }

        private void setDefaultVisibility()
        {
            CatCombo.Visibility = Visibility.Collapsed;
            StartUploadButton.Visibility = Visibility.Collapsed;
            Title_textBlock.Visibility = Visibility.Visible;
            Title_textBox.Visibility = Visibility.Collapsed;
            ShortDesc_textBlock.Visibility = Visibility.Visible;
            ShortDesc_textBox.Visibility = Visibility.Collapsed;
            Desc_textBlock.Visibility = Visibility.Visible;
            Desc_textBox.Visibility = Visibility.Collapsed;
            Price_textBlock.Visibility = Visibility.Visible;
            Price_textBox.Visibility = Visibility.Collapsed;
            AddCourseSections.Visibility = Visibility.Collapsed;
            Save_button.Visibility = Visibility.Collapsed;
            Cancel_button.Visibility = Visibility.Collapsed;
            if (App.IsLogged)
            {
                Edit_button.Visibility = Visibility.Visible;
                SignupButton.Visibility = Visibility.Visible;
            }
        }

        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            if (clicnt.LoggedIn)
            {
                //int course_id = Int32.Parse(CourseId.Text);
                if (Database.courseSignup("courses_members", user.Id, course_id))
                {
                    Debug.WriteLine(user.Name + " signed up for " + course_id);
                    SignupButton.Visibility = Visibility.Collapsed;
                    SignoffButton.Visibility = Visibility.Visible;
                }
                else
                {
                    Debug.WriteLine("Error signing up");
                }
            }
        }

        private void SignoffButton_Click(object sender, RoutedEventArgs e)
        {
            if (clicnt.LoggedIn)
            {
                //int course_id = Int32.Parse(CourseId.Text);
                if (Database.courseSignoff("courses_members", user.Id, course_id))
                {
                    Debug.WriteLine(user.Name + " signed off " + course_id);
                    SignupButton.Visibility = Visibility.Visible;
                    SignoffButton.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Debug.WriteLine("Error signing off");
                }
            }
        }

        private async void GetFriends()
        {
            FBSession clicnt = FBSession.ActiveSession;
            if (clicnt.LoggedIn)
            {
                var userId = clicnt.User.Id;
                string endpoint = "/" + userId + "/friends?fields=id,name,email,picture";
                PropertySet parameters = new PropertySet();
                //parameters.Add("limit", "3");
                FBSingleValue value = new FBSingleValue(endpoint, parameters, Rootobject.FromJson);
                FBResult graphResult = await value.GetAsync();

                if (graphResult.Succeeded)
                {
                    try
                    {
                        Rootobject profile = graphResult.Object as Rootobject;
                        int friends_count = profile.data.Count();                        
                        if (Convert.ToBoolean(friends_count))
                        {
                            var friendsList = new ObservableCollection<FBUserRootobject>();
                            for (int i = 0; i < friends_count; i++)
                            {
                                string id = profile.data[i]?.id;
                                if (Database.checkIfFBUserIsSigned(id, course_id))
                                {
                                    friendsList.Add(new FBUserRootobject() { id = profile.data[i]?.id, name = profile.data[i]?.name, email = profile.data[i]?.email });
                                    Debug.WriteLine("Friend {0} added to friends list.", i + 1);
                                }
                            }
                            if (friendsList.Count > 0)
                            {
                                TitleFbFriendsTextBlock.Visibility = Visibility.Visible;
                                FriendsList.ItemsSource = friendsList;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageDialog dialog = new MessageDialog(ex.ToString());
                        await dialog.ShowAsync();
                    }
                }
            }
        }

        private void BGRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb != null)
            {
                string rating = rb.Tag.ToString();
                Database.rateCourse(user.Id, course_id, Int32.Parse(rating));
            }
        }

        private void ClearRating_Click(object sender, RoutedEventArgs e)
        {
            clearRating();
            Database.rateCourse(user.Id, course_id, 0);
        }

        private void clearRating()
        {
            rb1.IsChecked = false;
            rb2.IsChecked = false;
            rb3.IsChecked = false;
            rb4.IsChecked = false;
            rb5.IsChecked = false;
        }

        private void AddSectionButton_Click(object sender, RoutedEventArgs e)
        {
            SectionsList selection = new SectionsList(course_id);
            sections.Add(selection);
        }

        private async void AddNewLesson_button_Click(object sender, RoutedEventArgs e)
        {
            if (!addSectionsToDatabase(sections))
            {
                string error = "Error adding sections!";
                MessageDialog dialog = new MessageDialog(error);
                await dialog.ShowAsync();
            }
            getSections();
            Frame.Navigate(typeof(AddLesson), course_id);
        }

        private void SectionsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Section section_selected = e.ClickedItem as Section;
            int section_id = section_selected.Id;
            //lessons = Database.getLessons(course_id, section_id);
            //sections_with_lessons[section_selected.Section_order - 1].lessons = lessons;
            //int i = Int32.Parse(sections_with_lessons.Select((v, index) => new { Section = v, Index = index }).Where(x => x.Section.Id == section_id).Select(x => x.Index).ToString());
            //int index = sections_with_lessons.IndexOf(from section in sections_with_lessons
            //                                          //select sections_with_lessons.section
            //                              where section.Id == section_id
            //                              select section);
            //var parent = sender as DependencyObject;
            //while (!(parent is ListView))
            //{
            //    parent = VisualTreeHelper.GetParent(parent);
            //}
            //var innerListView = parent as ListView;
        }

        private void LessonsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Lesson lesson_selected = e.ClickedItem as Lesson;
            Frame.Navigate(typeof(ViewLesson), lesson_selected);
        }
    }
}
