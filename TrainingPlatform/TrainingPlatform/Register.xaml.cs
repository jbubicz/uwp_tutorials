using System;
using System.Collections.ObjectModel;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Security.Cryptography;
using System.Text;
using winsdkfb;
using System.Diagnostics;
using winsdkfb.Graph;

namespace TrainingPlatform
{
    public sealed partial class Register : Page
    {
        ObservableCollection<RolesList> roles = Database.getRoles("users_roles");

        public Register()
        {
            this.InitializeComponent();
            RoleCombo.ItemsSource = roles;
            RoleCombo.SelectedItem = roles[0];
        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            User user = getUserDetails();
            bool exists = Database.checkIfFBUserExists(user.Fb_id);
            if (!exists)
            {
                addUser(user);
            }
            else
            {
                string message = "User is already registered!";
                MessageDialog dialog = new MessageDialog(message);
            }
        }

        private async void addUser(User user)
        {
            int user_id = user.Id;
            string fb_id = user.Fb_id;
            int role_id = user.Role_id;
            string name = user.Name;
            string email = user.Email;
            string pass = user.Password;
            string salt = user.Salt;
            string avatar = user.Avatar;
            try
            {
                bool inserted = Database.insertUser("users", fb_id, role_id, name, email, pass, salt, avatar);
                string message = "";

                if (inserted)
                {
                    message = "User successfully added!";
                }
                else
                {
                    message = "User not added!";
                }
                MessageDialog dialog = new MessageDialog(message);
                await dialog.ShowAsync();
            }
            catch (Exception e)
            {
                MessageDialog dialog = new MessageDialog(e.ToString());
            }
        }

        private User getUserDetails()
        {
            var role = RoleCombo.SelectedItem as RolesList;
            int role_id = role.Id;
            string name = NameTextBox.Text;
            string email = EmailTextBox.Text;
            string salt = createSalt(8);
            string pass = generateSHA256Hash(PasswordTexBox2.Text, salt);
            string avatar = "Assets/1.jpg";
            User new_user = new User { Role_id = role_id, Name = name, Email = email, Password = pass, Salt = salt, Avatar = avatar };
            return new_user;
        }

        private string createSalt(int size)
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            var buff = new byte[size];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        private String generateSHA256Hash(String input, String salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input + salt);
            HMACSHA256 sha256hash = new HMACSHA256();
            byte[] hash = sha256hash.ComputeHash(bytes);
            return BitConverter.ToString(hash);
        }

        private readonly string[] requested_permissions ={
            "public_profile",
            "email",
            "user_friends",
            "publish_actions"
        };

        private async void FBRegisterButton_Click(object sender, RoutedEventArgs e)
        {
            FBSession clicnt = FBSession.ActiveSession;
            clicnt.WinAppId = "s-1-15-2-2570658695-634084948-1695579385-2199792897-657639375-2101869116-1835229594";
            clicnt.FBAppId = "1819087251640431";
            FBPermissions permissions = new FBPermissions(requested_permissions);
            FBResult result = await clicnt.LoginAsync(permissions);

            if (result.Succeeded && clicnt.LoggedIn)
            {
                Debug.WriteLine(clicnt.User.Id);
                Debug.WriteLine(clicnt.User.Email);
                Debug.WriteLine(clicnt.User.FirstName);
                Debug.WriteLine(clicnt.User.LastName);
                getFBUserInfo();
                //User user = getFBUserDetails(clicnt.User);
                //addUser(user);
                //SquarePicture.UserId = clicnt.User.Id;
            }
            else
            {
                Debug.WriteLine(result.ErrorInfo);
            }
        }

        private async void getFBUserDetails(FBUserRootobject fbuser)
        {
            string fb_id = fbuser.id;
            var role = RoleCombo.SelectedItem as RolesList;
            int role_id = role.Id;
            string name = fbuser.name;
            string email = fbuser.email;
            string salt = createSalt(6);
            string pass = generateSHA256Hash("fbuser", salt);
            string avatar = fbuser.picture.data.url;
            User new_user = new User { Fb_id = fb_id, Role_id = role_id, Name = name, Email = email, Password = pass, Salt = salt, Avatar = avatar };
            bool exists = Database.checkIfFBUserExists(new_user.Fb_id);
            if (!exists)
            {
                addUser(new_user);
            }
            else
            {
                string message = "User is already registered!";
                MessageDialog dialog = new MessageDialog(message);
                await dialog.ShowAsync();
            }
        }

        private async void getFBUserInfo()
        {
            FBSession clicnt = FBSession.ActiveSession;
            if (clicnt.LoggedIn)
            {
                var userId = clicnt.User.Id;
                string endpoint = "/" + userId + "?access_token=1819087251640431|n5eZecurPAfO37og6RphwZ04tb8&fields=id,name,email,picture";

                PropertySet parameters = new PropertySet();
                FBSingleValue value = new FBSingleValue(endpoint, parameters, FBUserRootobject.FromJson);
                FBResult graphResult = await value.GetAsync();

                if (graphResult.Succeeded)
                {
                    try
                    {
                        FBUserRootobject profile = graphResult.Object as FBUserRootobject;
                        FBPicture.UserId = userId;
                        getFBUserDetails(profile);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString() + "\nBrak danych");
                    }
                }
            }

        }

        private async void LogoutFBButton_Click(object sender, RoutedEventArgs e)
        {
            FBSession clicnt = FBSession.ActiveSession;
            if (clicnt.LoggedIn)
            {
                await clicnt.LogoutAsync();
            }
        }
    }
}
