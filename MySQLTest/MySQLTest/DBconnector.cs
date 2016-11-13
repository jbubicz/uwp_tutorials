using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MySQLTest
{
    public class DBconnector
    {
        static string server = "127.0.0.1";
        static string database = "hurtownia";
        static string user = "root";
        static string pswd = "root";

        //public static bool login(string email, string password)
        //{
        //    string connectionString = "Server = " + server + ";database = " + database + ";uid = " + user + ";password = " + pswd + ";SslMode=None;";
        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        MySqlCommand checkLogin = new MySqlCommand("select password_hash, password_salt from users where email = \"" + email + "\"", connection);
        //        using (MySqlDataReader reader = checkLogin.ExecuteReader())
        //        {
        //            reader.Read();
        //            string hash = reader.GetString("password_hash");
        //            string salt = reader.GetString("password_salt");

        //            bool result = passwordGenerator.compare(password, hash, salt);

        //            if (result)
        //                return true;
        //            else
        //                return false;
        //        }
        //    }
        //}
    }
}
