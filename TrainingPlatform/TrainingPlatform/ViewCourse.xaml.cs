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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TrainingPlatform
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewCourse : Page
    {
        public ViewCourse()
        {
            this.InitializeComponent();                  
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var parameters = (Course)e.Parameter;
            lstGroup.DataContext = parameters;            
        }

        //private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        //{
        //    MySpliView.IsPaneOpen = !MySpliView.IsPaneOpen;
        //}

        private void lstGroup_ItemClick(object sender, ItemClickEventArgs e)
        {
            Course course = e.ClickedItem as Course;
            Frame.Navigate(typeof(ViewCourse));
        }

        private void lstAlphaBetic_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void lstAlphaBetic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //private void Back_Click(object sender, RoutedEventArgs e)
        //{
        //    if (Frame.CanGoBack)
        //    {
        //        Frame.GoBack();                
        //    }
        //}

        private void StartUpload_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            StartUploadButton.Visibility = Visibility.Visible;
            Title_textBlock.Visibility = Visibility.Collapsed;
            Title_textBox.Visibility = Visibility.Visible;
            ShortDesc_textBlock.Visibility = Visibility.Collapsed;
            ShortDesc_textBox.Visibility = Visibility.Visible;
            Price_textBlock.Visibility = Visibility.Collapsed;
            Price_textBox.Visibility = Visibility.Visible;
            Edit_button.Visibility = Visibility.Collapsed;
            Save_button.Visibility = Visibility.Visible;
            Cancel_button.Visibility = Visibility.Visible;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
