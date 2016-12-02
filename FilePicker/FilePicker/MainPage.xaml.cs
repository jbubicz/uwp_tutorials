using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FilePicker
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
      //public async void multiplefileselection()
      //  {
      //      foreach(var files in filelist)
      //      {
      //          var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
      //          var image = new BitmapImage();
      //          image.SetSource(stream);
      //          yourimagelist.Add(image);
      //      }
      //  }
        private async void openBtn_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");            
            openPicker.FileTypeFilter.Add(".png");
            
            StorageFile file = await openPicker.PickSingleFileAsync();
            

            if(file!=null)
            {
                
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                var image = new BitmapImage();
                image.SetSource(stream);
                imageView.Source = image;
            }
            else
            {
                //
            }
        }
    }
}
