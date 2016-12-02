using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TrainingPlatform
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddPage : Page
    {
        ObservableCollection<CategoriesList> categories = Database.getCategories("courses_categories");

        public AddPage()
        {
            this.InitializeComponent();
            CatCombo.ItemsSource = categories;
            CatCombo.SelectedItem = categories[0];


        }

        private Course getCourseDetails()
        {
            var cat = CatCombo.SelectedItem as CategoriesList;
            int cat_id = cat.Id;
            string title = title_box.Text;
            string short_desc = short_desc_box.Text;
            string desc = desc_box.Text;
            string price = price_box.Text;
            string img = "Assets/1.jpg";
            Course new_course = new Course { UserId = 1, Category = cat_id, Title = title, Price = price, ImgUrl = img, ShortDescription = short_desc, Description = desc, IsEnabled = 1 };
            return new_course;
        }

        private async void Upload_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".png");
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                var image = new BitmapImage();
                image.SetSource(stream);
                //imageView.Source = image;
            }
            else
            {
                //  
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void add_course_button_Click(object sender, RoutedEventArgs e)
        {
            Course course = getCourseDetails();
            int user_id = course.UserId;
            int cat_id = course.Category;
            string title = course.Title;
            string price = course.Price;
            string img = course.ImgUrl;
            string short_desc = course.ShortDescription;
            string desc = course.Description;
            bool inserted= Database.insertCourse("courses", user_id, cat_id, title, price, img, short_desc, desc);
            if (inserted)
            {
                string success = "Course successfully added!"; 
                MessageDialog dialog = new MessageDialog(success);
                dialog.ShowAsync();
            }
        }
    }
}
