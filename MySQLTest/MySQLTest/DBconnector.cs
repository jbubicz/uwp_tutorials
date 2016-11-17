using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace MySQLTest
{
    public class DBconnector
    {
        static string server = "127.0.0.1";
        static string database = "mydb";
        static string user = "root";
        static string pswd = "";

        public static void test()
        {
            EncodingProvider ppp;
            ppp = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(ppp);
            string connectionString = "Server = " + server + ";database = " + database + ";uid = " + user + ";password = " + pswd + ";SslMode=None;charset=utf8";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand getCommand = connection.CreateCommand();
                getCommand.CommandText = "SELECT * FROM `users`";
                using (MySqlDataReader reader = getCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Debug.Write(reader.GetString(0));
                        Debug.Write(reader.GetString(2));
                        Debug.Write(reader.GetString(3));
                    }
                }
            }
        }
    }
}
