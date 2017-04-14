﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace TrainingPlatform
{
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<string> suggestions = new ObservableCollection<string>();

        public MainPage()
        {
            this.InitializeComponent();           
            MyFrame.Navigate(typeof(CoursesList));
            MyFrame.Navigated += myFrame_Navigated;
            setLogIn();
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;                        
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            setLogIn();
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
                Login_link.IsSelected = false;
            }
            if (AllCourses_link.IsSelected)
            {
                MyFrame.Navigate(typeof(CoursesList));
                MySpliView.IsPaneOpen = !MySpliView.IsPaneOpen;
                AllCourses_link.IsSelected = false;
            }
            if (AddCourse_link.IsSelected)
            {
                MyFrame.Navigate(typeof(AddCourse));
                MySpliView.IsPaneOpen = !MySpliView.IsPaneOpen;
                AddCourse_link.IsSelected = false;
            }
        }

        private void myFrame_Navigated(object sender, NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                ((Frame)sender).CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (MyFrame.CanGoBack)
            {
                e.Handled = true;
                MyFrame.GoBack();
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

        private async void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Only get results when it was a user typing, 
            // otherwise assume the value got filled in by TextMemberPath 
            // or the handler for SuggestionChosen.
            try
            {
                if (suggestions.Count == 0)
                {
                    suggestions = Database.getSuggestions();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error getting suggestions! " + e.ToString());
            }
            string text = sender.Text;
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                //Set the ItemsSource to be your filtered dataset
                if (sender.Text.Trim().Length > 1)
                {
                    sender.ItemsSource = await Task<string[]>.Run(() => { return getSuggestions(text); });
                }
                else
                {

                }

            }
        }

        private string[] getSuggestions(string text)
        {
            string[] result = null;
            string t = text.Trim().ToLower();
            result = suggestions.Where(x => x.ToLower().Contains(t)).ToArray();
            return result;
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            // Set sender.Text. You can use args.SelectedItem to build your text string.
            //string suggestion = args.SelectedItem.ToString();
            //if (suggestion != null || suggestion != "")
            //{
            //    ObservableCollection<Course> courses = Database.getCourse(suggestion);
            //    Frame.Navigate(typeof(SearchedCoursesList), courses);
            //}
        }


        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            string suggestion = "";
            if (args.ChosenSuggestion != null)
            {
                suggestion = args.ChosenSuggestion.ToString().Trim();
                if (suggestion.Length > 1)
                {
                    Course courses = Database.getCourse(suggestion);
                    MyFrame.Navigate(typeof(ViewCourse), courses);
                }
                else
                {

                }
            }
            else
            {
                suggestion = args.QueryText.Trim().ToLower();
                if (suggestion.Length > 1)
                {
                    ObservableCollection<Course> courses = Database.getCourses(suggestion);
                    MyFrame.Navigate(typeof(SearchedCoursesList), courses);

                }
            }
        }

        public void setLogIn()
        {
            if (App.IsLogged)
            {
                LoginButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                LoginButton.Visibility = Visibility.Visible;
            }
        }
    }
}
