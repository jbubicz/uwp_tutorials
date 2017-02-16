using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using winsdkfb;
using winsdkfb.Graph;

namespace TrainingPlatform
{
    public sealed partial class ViewCourse : Page
    {
        private string resourceName = "My App";
        static FBSession clicnt = FBSession.ActiveSession;
        User user = getUserInfo(clicnt);
        //private string fb_id;
        private int course_id;
        ObservableCollection<CategoriesList> categories = Database.getCategories("courses_categories");
        private ObservableCollection<FBUserRootobject> friendsList;
        private double value;

        public ViewCourse()
        {
            this.InitializeComponent();
            CatCombo.ItemsSource = categories;
            //CatCombo.SelectedItem = categories[0];
            //GetCredentialFromLocker();
            GetFriends();
            friendsList = new ObservableCollection<FBUserRootobject>();
        }

        private Windows.Security.Credentials.PasswordCredential GetCredentialFromLocker()
        {
            Windows.Security.Credentials.PasswordCredential credential = null;

            var vault = new Windows.Security.Credentials.PasswordVault();
            var credentialList = vault.FindAllByResource(resourceName);
            if (credentialList.Count > 0)
            {
                if (credentialList.Count == 1)
                {
                    credential = credentialList[0];
                }
                else
                {
                    // When there are multiple usernames,
                    // retrieve the default username. If one doesn't
                    // exist, then display UI to have the user select
                    // a default username.

                    // defaultUserName = GetDefaultUserNameUI();

                    // credential = vault.Retrieve(resourceName, defaultUserName);
                }
            }

            return credential;
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
            course_id = Int32.Parse(CourseId.Text);
            int actual_category_id = parameters.Category;
            CatCombo.SelectedItem = categories[actual_category_id - 1];
            setButtonsVisibility(course_id);
        }

        private void setButtonsVisibility(int course_id)
        {
            if (Double.TryParse(Price_textBlock.Text, out value))
            {
                Price_textBlock.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture, "{0:C2}", value);
            }
            else
            {
                Price_textBlock.Text = String.Empty;
            }
            if (double.Parse(Rating_textBlock.Text) < 1)
            {
                Rating_textBlock.Visibility = Visibility.Collapsed;
            }
            if (clicnt.LoggedIn)
            {
                Edit_button.Visibility = Visibility.Visible;
                bool isSigned = Database.checkIfFBUserIsSigned(clicnt.User.Id, course_id);
                if (isSigned)
                {
                    SignupButton.Visibility = Visibility.Collapsed;
                    SignoffButton.Visibility = Visibility.Visible;
                }
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
            Edit_button.Visibility = Visibility.Collapsed;
            Save_button.Visibility = Visibility.Visible;
            Cancel_button.Visibility = Visibility.Visible;
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
            bool updated = Database.updateCourse("courses", course_id, user_id, cat_id, title, price, img, short_desc, desc);
            if (updated)
            {
                string success = "Course successfully edited!";
                MessageDialog dialog = new MessageDialog(success);
                await dialog.ShowAsync();
            }
            Frame.Navigate(typeof(ViewCourse), course);
            //setDefaultVisibility();
        }

        private Course getEditedCourseDetails()
        {
            //Course edited_course = null;

            var cat = CatCombo.SelectedItem as CategoriesList;
            int cat_id = cat.Id;
            string title = Title_textBox.Text;
            string short_desc = ShortDesc_textBox.Text;
            string desc = Desc_textBox.Text;
            string price = Price_textBox.Text;
            string img = "Assets/course.jpg";
            Course edited_course = new Course { UserId = user.Id, Category = cat_id, Title = title, Price = price, ImgUrl = img, ShortDescription = short_desc, Description = desc, IsEnabled = 1 };
            return edited_course;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            setDefaultVisibility();
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
            Edit_button.Visibility = Visibility.Visible;
            Save_button.Visibility = Visibility.Collapsed;
            Cancel_button.Visibility = Visibility.Collapsed;
        }

        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            if (clicnt.LoggedIn)
            {
                //int course_id = Int32.Parse(CourseId.Text);
                if (Database.courseSignup("courses_members", user.Id, course_id))
                {
                    Debug.Write("\n" + user.Name + " signed up for " + course_id);
                    SignupButton.Visibility = Visibility.Collapsed;
                    SignoffButton.Visibility = Visibility.Visible;
                }
                else
                {
                    Debug.Write("\nError signing up");
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
                    Debug.Write("\n" + user.Name + " signed off " + course_id);
                    SignupButton.Visibility = Visibility.Visible;
                    SignoffButton.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Debug.Write("\nError signing off");
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
                        int friends_count = profile.summary.total_count;
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

        }

        private void ClearRating_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
