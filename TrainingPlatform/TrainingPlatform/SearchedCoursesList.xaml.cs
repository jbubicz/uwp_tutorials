using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace TrainingPlatform
{
    public sealed partial class SearchedCoursesList : Page
    {
        private ObservableCollection<Course> courses = new ObservableCollection<Course>();

        public SearchedCoursesList()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            courses = (ObservableCollection<Course>)e.Parameter;
            SearchedCourses_list.ItemsSource = courses;
            setControlsVisibility(courses);
        }

        private void setControlsVisibility(ObservableCollection<Course> courses)
        {
            if (courses.Count==0)
            {
                NoResults.Visibility = Visibility.Visible;
            }
            else
            {
                NoResults.Visibility = Visibility.Collapsed;
            }
        }       

        private void SearchedCoursesList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Course course = e.ClickedItem as Course;
            Frame.Navigate(typeof(ViewCourse), course);
        }
    }
}
