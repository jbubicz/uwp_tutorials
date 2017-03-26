using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace TrainingPlatform
{    
    public sealed partial class CoursesList : Page
    {
        public CoursesList()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }


        private void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainPivot.SelectedIndex == 0)
            {               
                ObservableCollection<Course> lstsource = Database.getAllActiveCourses("courses");
                lstGroup.ItemsSource = lstsource;
            }
            else if (MainPivot.SelectedIndex == 1)
            {
                ObservableCollection<Course> lstsource = Database.getTopCourses("courses");
                TopCoursesListView.ItemsSource = lstsource;
            }
            else if (MainPivot.SelectedIndex == 2)
            {
                ObservableCollection<Course> lstsource = Database.getNewestCourses("courses");
                NewestCoursesListView.ItemsSource = lstsource;
            }
        }

        private void lstGroup_ItemClick(object sender, ItemClickEventArgs e)
        {
            Course course = e.ClickedItem as Course;
            Frame.Navigate(typeof(ViewCourse), course);
        }

        private void TopCoursesListView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void NewestCoursesListView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}
