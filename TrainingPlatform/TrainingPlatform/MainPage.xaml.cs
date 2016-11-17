using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TrainingPlatform
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySpliView.IsPaneOpen = !MySpliView.IsPaneOpen;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Link1.IsSelected)
            {

            }
            if (Link2.IsSelected)
            {

            }
        }

        private void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainPivot.SelectedIndex == 0)
            {
                //List<Words> lstsource = Database.getAllWords("Favorites");
                //lstFavorites.ItemsSource = lstsource;
                //if (lstFavorites.Items.Count > 0)
                //    btnMulti.Visibility = Visibility.Visible;
                //btnRemind.Visibility = Visibility.Visible;
                List<Course> lstsource = Database.getAllCourses("courses");
                lstGroup.ItemsSource = lstsource;
            }
            else if (MainPivot.SelectedIndex == 1)
            {
                //List<Course> lstsource = Database.getAllCourses("courses");
                //lstAlphaBetic.ItemsSource = lstsource;                
            }
            else { return; }
        }

        private void lstGroup_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void lstAlphaBetic_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void lstAlphaBetic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void lstVideo_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}
