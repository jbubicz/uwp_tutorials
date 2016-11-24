using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Facebook;
namespace OAuth1
{
    public partial class frmSignIn : Form
    {
        public frmSignIn()
        {
            InitializeComponent();
        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            string OAuthURL = @"https://www.facebook.com/dialog/oauth?client_id=427753907263857&redirect_uri=https://www.facebook.com/connect/login_success.html&response_type=token&scope=user_groups";
            WebFacebook.Navigate(OAuthURL);

        }
        string access_token;
        private void WebFacebook_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (WebFacebook.Url.AbsoluteUri.Contains("access_token"))
            {
                string url1 = WebFacebook.Url.AbsoluteUri;
                string url2 = url1.Substring(url1.IndexOf("access_token") + 13);
                 access_token = url2.Substring(0, url2.IndexOf("&"));
                MessageBox.Show("access_token = "+access_token);

                FacebookClient fb = new FacebookClient(access_token);

                dynamic data = fb.Get("/me");

                MessageBox.Show("id="+data.id + Environment.NewLine + "name="+data.name+Environment.NewLine +"link="+data.link);
                
            }
        }

        private void btnGetList_Click(object sender, EventArgs e)
        {
            FacebookClient fb = new FacebookClient(access_token);
            dynamic FriendList = fb.Get("/me/groups");

            int count = (int)FriendList.data.Count;

            for (int i = 0; i < count; i++)
            {
                lstFriendList.Items.Add(FriendList.data[i].name);
            }

        }
    }
}
