using System;
using System.Globalization;
using System.IO;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace MainMenu.Models
{
    public class MainMenuPageButton
    {
        public string NavigationPageName { get; set; }
        public string Title { get; set; }
        public string History { get; set; }
        public string Image { get; set; }

        public MainMenuPageButton() {}
        public MainMenuPageButton(string page, string title = null, string image = null)
        {
            this.NavigationPageName = page;
            this.Title = title;
            this.Image = image;
        }
        public MainMenuPageButton(Type page, string title = null, string image = null)
        {
            this.NavigationPageName = page.FullName;
            this.Title = title;
            this.Image = image;
        }
    }
}