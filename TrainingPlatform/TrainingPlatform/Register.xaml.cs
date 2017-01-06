using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Security.Cryptography;
using System.Text;

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

        private async void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            User user = getUserDetails();
            int user_id = user.Id;
            int fb_id = user.Fb_id;
            int role_id = user.Role_id;
            string name = user.Name;
            string email = user.Email;
            string pass = user.Password;
            string salt = user.Salt;
            string avatar = user.Avatar;
            bool inserted = Database.insertUser("users", fb_id, role_id, name, email, pass, salt);
            if (inserted)
            {
                string success = "User successfully added!";
                MessageDialog dialog = new MessageDialog(success);
                await dialog.ShowAsync();
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

    }
}
