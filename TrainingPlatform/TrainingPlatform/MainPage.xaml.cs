﻿using System;
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
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            MyFrame.Navigate(typeof(CoursesList));
            Back.Visibility = Visibility.Collapsed;
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySpliView.IsPaneOpen = !MySpliView.IsPaneOpen;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Login_link.IsSelected)
            {                              
                MyFrame.Navigate(typeof(LoginPage));
                MySpliView.IsPaneOpen = !MySpliView.IsPaneOpen;
            }
            if (AllCourses_link.IsSelected)
            {
                MyFrame.Navigate(typeof(CoursesList));
                MySpliView.IsPaneOpen = !MySpliView.IsPaneOpen;
            }
            if (AddCourse_link.IsSelected)
            {
                MyFrame.Navigate(typeof(AddCourse));
                MySpliView.IsPaneOpen = !MySpliView.IsPaneOpen;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (MyFrame.CanGoBack)
            {
                MyFrame.GoBack();
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(typeof(LoginPage));
        }        
    }
}
