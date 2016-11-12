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
            if (result.Succeeded)
            {
                Debug.WriteLine(result.Succeeded);
            }
            else
            {
                Debug.WriteLine(result.ErrorInfo);
            }
        }
    }
}

