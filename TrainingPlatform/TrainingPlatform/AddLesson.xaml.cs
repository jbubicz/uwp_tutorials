using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TrainingPlatform.Models;
using Windows.System.Display;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using winsdkfb;

namespace TrainingPlatform
{
    public sealed partial class AddLesson : Page
    {
        static FBSession clicnt = FBSession.ActiveSession;
        private Database database = new Database();
        private User user = getUserInfo(clicnt);
        private ObservableCollection<SectionsList> sections;
        private Section section = new Section();
        private Lesson lesson = new Lesson();
        private int course_id;
        private DisplayRequest appDisplayRequest = null;

        public AddLesson()
        {
            this.InitializeComponent();
            if (clicnt.LoggedIn)
            {
                App.IsLogged = true;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            course_id = (int)e.Parameter;
            getSections();
            if (sections == null || sections.Count == 0)
            {
                SectionCombo.Visibility = Visibility.Collapsed;
            }
            else
            {
                SectionCombo.ItemsSource = sections;
                SectionCombo.SelectedItem = sections[0];
            }
        }

        private static User getUserInfo(FBSession clicnt)
        {
            if (App.IsLogged)
            {
                User user = new User();
                user = user.fbUser(clicnt.User.Id);
                return user;
            }
            else
                return null;
        }

        private ObservableCollection<SectionsList> getSections()
        {
            try
            {
                return sections = Database.getSections(course_id);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error getting course sections: " + e.ToString());
                return null;
            }
        }

        private async void AddLesson_button_Click(object sender, RoutedEventArgs e)
        {
            getNewLessonDetails();
            string message;
            if (Database.insertLesson(lesson))
            {
                message = "Lesson successfully added!";
            }
            else
            {
                message = "Error. Lesson NOT added!";
            }
            MessageDialog dialog = new MessageDialog(message);
            await dialog.ShowAsync();
        }

        private void getNewLessonDetails()
        {
            lesson.User_id = user.Id;
            section = SectionCombo.SelectedItem as Section;
            lesson.Section_id = section == null ? 0 : section.Id;
            lesson.Course_id = course_id;
            lesson.Img = "Assets/course.jpg";
            lesson.Video = "https://fpdl.vimeocdn.com/vimeo-prod-skyfire-std-us/01/1518/8/207594487/708355308.mp4?token=58d058ff_0xb4ad22edac0d7edcf7ad770be15f118286d4fc8a";
            lesson.Lesson_title = title_box.Text;
            lesson.Free = Convert.ToInt32(Free_checkbox.IsChecked);
            lesson.Description = desc_box.Text;
            lesson.IsEnabled = Convert.ToInt32(Enable_checkbox.IsChecked);
        }
    }
}
