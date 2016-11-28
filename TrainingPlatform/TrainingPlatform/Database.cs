﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingPlatform
{
    class Database
    {
        static string server = "127.0.0.1";
        static string database = "mydb";
        static string user = "root";
        static string pswd = "";

        public static ObservableCollection<Course> getAllActiveCourses(string tableName)
        {
            ObservableCollection<Course> result = new ObservableCollection<Course>();
            EncodingProvider ppp;
            ppp = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(ppp);
            string connectionString = "Server = " + server + ";database = " + database + ";uid = " + user + ";password = " + pswd + ";SslMode=None;charset=utf8";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "SELECT * FROM " + tableName + " WHERE `is_enabled` =1";
                    using (MySqlDataReader reader = getCommand.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                Course course = new Course();
                                course.Id = reader.GetInt32("id");
                                course.UserId = reader.GetInt32("user_id");
                                course.Category = reader.GetInt32("category_id");
                                course.Title = reader.GetString("title");
                                course.Price = reader.GetFloat("price");
                                course.ImgUrl = reader.GetString("img");
                                course.ShortDescription = reader.GetString("short_description");
                                course.Description = reader.GetString("description");
                                course.IsEnabled = reader.GetBoolean("is_enabled");
                                course.Created = reader.GetDateTime("created");
                                course.Modified = reader.GetDateTime("modified");
                                result.Add(course);
                            }
                        }
                    }
                    connection.Close();
                }
                return result;
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
                return null;
            }
        }

        public static ObservableCollection<Course> getAllDisabledCourses(string tableName)
        {
            ObservableCollection<Course> result = new ObservableCollection<Course>();
            EncodingProvider ppp;
            ppp = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(ppp);
            string connectionString = "Server = " + server + ";database = " + database + ";uid = " + user + ";password = " + pswd + ";SslMode=None;charset=utf8";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "SELECT * FROM " + tableName + " WHERE `is_enabled` =0";
                    using (MySqlDataReader reader = getCommand.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                Course course = new Course();
                                course.Id = reader.GetInt32("id");
                                course.UserId = reader.GetInt32("user_id");
                                course.Category = reader.GetInt32("category_id");
                                course.Title = reader.GetString("title");
                                course.Price = reader.GetFloat("price");
                                course.ImgUrl = reader.GetString("img");
                                course.ShortDescription = reader.GetString("short_description");
                                course.Description = reader.GetString("description");
                                course.IsEnabled = reader.GetBoolean("is_enabled");
                                course.Created = reader.GetDateTime("created");
                                course.Modified = reader.GetDateTime("modified");
                                result.Add(course);
                            }
                        }
                    }
                    connection.Close();
                }
                return result;
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
                return null;
            }
        }

            public static ObservableCollection<Course> getCourseDetails(string tableName)
        {
            ObservableCollection<Course> result = new ObservableCollection<Course>();
            EncodingProvider ppp;
            ppp = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(ppp);
            string connectionString = "Server = " + server + ";database = " + database + ";uid = " + user + ";password = " + pswd + ";SslMode=None;charset=utf8";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "SELECT * FROM " + tableName + " WHERE `is_enabled` =0";
                    using (MySqlDataReader reader = getCommand.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                Course course = new Course();
                                course.Id = reader.GetInt32("id");
                                course.UserId = reader.GetInt32("user_id");
                                course.Category = reader.GetInt32("category_id");
                                course.Title = reader.GetString("title");
                                course.Price = reader.GetFloat("price");
                                course.ImgUrl = reader.GetString("img");
                                course.ShortDescription = reader.GetString("short_description");
                                course.Description = reader.GetString("description");
                                course.IsEnabled = reader.GetBoolean("is_enabled");
                                course.Created = reader.GetDateTime("created");
                                course.Modified = reader.GetDateTime("modified");
                                result.Add(course);
                            }
                        }
                    }
                    connection.Close();
                }
                return result;
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
                return null;
            }
        }
    }
}
