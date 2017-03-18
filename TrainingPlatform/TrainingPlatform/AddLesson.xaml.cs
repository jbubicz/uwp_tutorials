using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TrainingPlatform.Models;
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
    public sealed partial class AddLesson : Page
    {
        //ObservableCollection<CategoriesList> categories = Database.getSections(course_id);
        public AddLesson()
        {
            this.InitializeComponent();
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);            
            var sections = (ObservableCollection<SectionsList>)e.Parameter;
            SectionCombo.ItemsSource = sections;
            var course_id = (from section in sections
                            select section.Course_id).FirstOrDefault();

                             
           
        }
        
        private void DeleteLesson_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddLesson_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Back_button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
