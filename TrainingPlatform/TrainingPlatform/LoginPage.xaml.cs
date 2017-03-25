using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Authentication.Web;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using winsdkfb;
using winsdkfb.Graph;

namespace TrainingPlatform
{
    public sealed partial class LoginPage : Page
    {
        static FBSession clicnt = FBSession.ActiveSession;
        public string username { get; private set; }
        public string fb_id { get; private set; }

        private readonly string[] requested_permissions ={
            "public_profile",
            "email",
            "user_friends",
            "publish_actions"
        };

        public LoginPage()
        {
            this.InitializeComponent();
            //string SID = WebAuthenticationBroker.GetCurrentApplicationCallbackUri().ToString();
            //Debug.WriteLine(SID);           
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (clicnt.LoggedIn)
            {
                setSession();               
            }
            else
            {
                App.IsLogged = false;
                logout.Visibility = Visibility.Collapsed;
            }
        }



        private async void login_Click(object sender, RoutedEventArgs e)
        {
            FBSession clicnt = FBSession.ActiveSession;
            clicnt.WinAppId = "s-1-15-2-2570658695-634084948-1695579385-2199792897-657639375-2101869116-1835229594";
            clicnt.FBAppId = "1819087251640431";
            FBPermissions permissions = new FBPermissions(requested_permissions);
            FBResult result = await clicnt.LoginAsync(permissions);

            if (result.Succeeded && clicnt.LoggedIn)
            {
                IsLogged.Text = "Successfully logged in";
                Debug.WriteLine(clicnt.User.Id);
                Debug.WriteLine(clicnt.User.Email);
                Debug.WriteLine(clicnt.User.FirstName);
                Debug.WriteLine(clicnt.User.LastName);
                setSession();
            }
            else
            {
                Debug.WriteLine(result.ErrorInfo);
            }
            try
            {
                var vault = new Windows.Security.Credentials.PasswordVault();
                vault.Add(new Windows.Security.Credentials.PasswordCredential(
                    "My App", clicnt.User.Name, clicnt.User.Id));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);   
            }
        }

        private void setSession()
        {
            App.IsLogged = true;
            login.Visibility = Visibility.Collapsed;
            logout.Visibility = Visibility.Visible;
            SquarePicture.UserId = clicnt.User.Id;
        }

        private void UserLikesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void logout_Click(object sender, RoutedEventArgs e)
        {
            FBSession clicnt = FBSession.ActiveSession;
            if (clicnt.LoggedIn)
            {
                await clicnt.LogoutAsync();
                App.IsLogged = false;
            }
            IsLogged.Text = "Successfully logged out";
            SquarePicture.UserId = "";
            login.Visibility = Visibility.Visible;
            logout.Visibility = Visibility.Collapsed;
        }

        private async void OnGet(object sender, RoutedEventArgs e)
        {
            FBSession clicnt = FBSession.ActiveSession;
            if (clicnt.LoggedIn)
            {
                var userId = clicnt.User.Id;
                string endpoint = "/" + userId + "/friends?fields=id,name,email,picture";

                PropertySet parameters = new PropertySet();
                // parameters.Add("limit", "10");     

                FBSingleValue value = new FBSingleValue(endpoint, parameters, Rootobject.FromJson);
                FBResult graphResult = await value.GetAsync();

                if (graphResult.Succeeded)
                {
                    try
                    {
                        Rootobject profile = graphResult.Object as Rootobject;
                        string name = profile.data[0]?.name;
                        string email = profile.data[0]?.email;
                        MessageDialog dialog = new MessageDialog(name + "\n" + email);
                        await dialog.ShowAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageDialog dialog = new MessageDialog(ex.ToString() + "\nBrak znajomych");
                        await dialog.ShowAsync();
                    }
                }
            }
        }

        private void SimpleLoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginTextBox.ClearValue(TextBox.BorderBrushProperty);
            LoginTextBox.ClearValue(TextBox.PlaceholderTextProperty);

            if (string.IsNullOrWhiteSpace(LoginTextBox.Text))
            {
                LoginTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                LoginTextBox.PlaceholderText = "Login is required";
            }
            else if (LoginTextBox.Text.Length <=3)
            {
                LoginTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                
            }
        }

        private void RegisterLink_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Register));
        }
    }
}
