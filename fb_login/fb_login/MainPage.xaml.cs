using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Authentication.Web;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using winsdkfb;
using winsdkfb.Graph;
using Newtonsoft.Json;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace fb_login
{

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {

            this.InitializeComponent();
            string SID = WebAuthenticationBroker.GetCurrentApplicationCallbackUri().ToString();
            Debug.WriteLine(SID);
        }

        private readonly string[] requested_permissions ={
            "public_profile",
            "email",
            "user_friends",
            "publish_actions"
        };

        private async void login_Click(object sender, RoutedEventArgs e)
        {
            FBSession clicnt = FBSession.ActiveSession;          
            clicnt.WinAppId = "s-1-15-2-3519449986-424852128-1301220477-1625817896-4185431776-1950452228-152039786";
            clicnt.FBAppId = "1776174319303320";            
            FBPermissions permissions = new FBPermissions(requested_permissions);
            FBResult result = await clicnt.LoginAsync(permissions);
            if (result.Succeeded && clicnt.LoggedIn)
            {
                Debug.WriteLine(result.Object);

                Debug.WriteLine(result.Succeeded);
                // login.Content = "Logout";
                IsLogged.Text = "Logged in";
                Debug.WriteLine(clicnt.User.Gender);

                Debug.WriteLine(clicnt.User.FirstName);
                Debug.WriteLine(clicnt.User.LastName);
                SquarePicture.UserId = clicnt.User.Id;                 
            }
            else
            {
                Debug.WriteLine(result.ErrorInfo);
            }
            //string mail = clicnt.User.Email;
            //Debug.Write(mail);
            //await clicnt.LogoutAsync();
            //Debug.WriteLine(result.Succeeded);
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
            }
        }

        private async void OnGet(object sender, RoutedEventArgs e)
        {
            string endpoint = "/me/friends";

            PropertySet parameters = new PropertySet();
            parameters.Add("limit", "10");

            FBSingleValue value = new FBSingleValue(endpoint, parameters, Rootobject.FromJson);
            FBResult graphResult = await value.GetAsync();

            if (graphResult.Succeeded)
            {
                Rootobject profile = graphResult.Object as Rootobject;
                string name = profile.data[0]?.name;
                MessageDialog dialog = new MessageDialog(name);
                await dialog.ShowAsync();
            }
        }

    }
}

