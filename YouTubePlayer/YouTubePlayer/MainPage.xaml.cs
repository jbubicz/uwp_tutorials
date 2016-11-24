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
using MyToolkit.Multimedia;

namespace YouTubePlayer
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            //var url = await YouTube.GetVideoUriAsync("rjVqGEGhD9c", YouTubeQuality.Quality1080P);
            //Player.Source = url.Uri;
            Player.Play();
          
           
        }

        private void pause_Click(object sender, RoutedEventArgs e)
        {
            Player.Pause();
        }

        private void Player_BufferingProgressChanged(object sender, RoutedEventArgs e)
        {
            dupa.Visibility = Visibility.Visible;
        }

        private void Player_Loading(FrameworkElement sender, object args)
        {
          //  dupa.Visibility = Visibility.Visible;
        }
    }
}
