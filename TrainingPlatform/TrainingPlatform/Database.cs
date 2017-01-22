using MySql.Data.MySqlClient;
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
        //static string server = "127.0.0.1";
        //static string database = "mydb";
        //static string user = "root";
        //static string pswd = "";
        //http://breko.eu/phpmyadmin/
        static string server = "188.116.20.191";
        static string database = "brecowww_szkolenia";
        static string user = "brecowww_szkolus";
        static string pswd = "szkolenia123";

        public static ObservableCollection<Course> getAllActiveCourses(string tableName)
        {
            ObservableCollection<Course> result = new ObservableCollection<Course>();
            string connectionString = getConnectionString();
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
                                course.Price = reader.GetString("price");
                                course.ImgUrl = reader.GetString("img");
                                course.ShortDescription = reader.GetString("short_description");
                                course.Description = reader.GetString("description");
                                course.IsEnabled = reader.GetInt32("is_enabled");
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
            string connectionString = getConnectionString();
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
                                course.Price = reader.GetString("price");
                                course.ImgUrl = reader.GetString("img");
                                course.ShortDescription = reader.GetString("short_description");
                                course.Description = reader.GetString("description");
                                course.IsEnabled = reader.GetInt32("is_enabled");
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

        public static ObservableCollection<CategoriesList> getCategories(string tableName)
        {
            ObservableCollection<CategoriesList> result = new ObservableCollection<CategoriesList>();
            string connectionString = getConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "SELECT * FROM " + tableName;
                    using (MySqlDataReader reader = getCommand.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CategoriesList category = new CategoriesList();
                                category.Id = reader.GetInt32("id");
                                category.Title = reader.GetString("title");
                                result.Add(category);
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

        public static ObservableCollection<RolesList> getRoles(string tableName)
        {
            ObservableCollection<RolesList> result = new ObservableCollection<RolesList>();
            string connectionString = getConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "SELECT * FROM " + tableName;
                    using (MySqlDataReader reader = getCommand.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RolesList role = new RolesList();
                                role.Id = reader.GetInt32("id");
                                role.Title = reader.GetString("title");
                                result.Add(role);
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

        public static User getFBUserInfo(string fb_id)
        {
            User user = new User();
            string connectionString = getConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "SELECT `id`, `role_id`, `name`, `email`, `avatar`, `about`, `points`, `created`, `modified` FROM `users`" +
                        "WHERE `fb_id`=@fb_id";
                    getCommand.Parameters.AddWithValue("@fb_id", fb_id);
                    using (MySqlDataReader reader = getCommand.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                user.Id = reader.GetInt32("id");
                                user.Role_id = reader.GetInt32("role_id");
                                user.Name = reader.GetString("name");
                                user.Email= reader.GetString("email");
                                user.Avatar= reader.GetString("avatar");
                               // user.About = reader.GetString("about");
                                user.Points = reader.GetInt32("points");
                                user.Created = reader.GetDateTime("created");
                                user.Modified = reader.GetDateTime("modified");
                            }
                        }
                    }
                    connection.Close();
                }
                return user;
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
                return null;
            }
        }

        public static bool insertCourse(string tableName, int user_id, int cat_id, string title, string price, string img, string short_desc, string desc)
        {
            string connectionString = getConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "INSERT INTO " + tableName +
                        "(`user_id`, `category_id`, `title`, `price`, `img`, `short_description`, `description`) "
                        + "VALUES(@user_id, @category_id, @title, @price, @img, @short_description, @description)";
                    getCommand.Parameters.AddWithValue("@user_id", user_id);
                    getCommand.Parameters.AddWithValue("@category_id", cat_id);
                    getCommand.Parameters.AddWithValue("@title", title);
                    getCommand.Parameters.AddWithValue("@price", price);
                    getCommand.Parameters.AddWithValue("@img", img);
                    getCommand.Parameters.AddWithValue("@short_description", short_desc);
                    getCommand.Parameters.AddWithValue("@description", desc);
                    Debug.Write(getCommand.CommandText);
                    getCommand.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
                return false;
            }
        }

        public static bool updateCourse(string tableName, int course_id, int cat_id, string title, string price, string img, string short_desc, string desc)
        {
            string connectionString = getConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "UPDATE" + tableName +
                        "SET `category_id`=@category_id,`title`=@title,`price`=@price,`img`=@img,`short_description`=@short_description,`description`=@description" +
                        "WHERE `id`=@course_id";
                    getCommand.Parameters.AddWithValue("@course_id", course_id);
                    getCommand.Parameters.AddWithValue("@category_id", cat_id);
                    getCommand.Parameters.AddWithValue("@title", title);
                    getCommand.Parameters.AddWithValue("@price", price);
                    getCommand.Parameters.AddWithValue("@img", img);
                    getCommand.Parameters.AddWithValue("@short_description", short_desc);
                    getCommand.Parameters.AddWithValue("@description", desc);
                    Debug.Write(getCommand.CommandText);
                    getCommand.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
                return false;
            }
        }

        public static bool deleteCourse(string tableName, int course_id)
        {
            string connectionString = getConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "UPDATE" + tableName +
                        "SET `is_enabled`=0" +
                        "WHERE `id`=@course_id";
                    getCommand.Parameters.AddWithValue("@course_id", course_id);
                    Debug.Write(getCommand.CommandText);
                    getCommand.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
                return false;
            }
        }

        public static bool insertUser(string tableName, string fb_id, int role_id, string name, string email, string pass, string salt, string avatar)
        {
            string connectionString = getConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    if (fb_id == "0")
                    {
                        getCommand.CommandText = "INSERT INTO " + tableName +
                            "(`role_id`, `name`, `email`, `password`, `salt`, `avatar`) "
                            + "VALUES(@role_id, @name, @email, @password, @salt, @avatar)";
                    }
                    else
                    {
                        getCommand.CommandText = "INSERT INTO " + tableName +
                        "(`fb_id`, `role_id`, `name`, `email`, `password`, `salt`, `avatar`) "
                        + "VALUES(@fb_id, @role_id, @name, @email, @password, @salt, @avatar)";
                        getCommand.Parameters.AddWithValue("@fb_id", fb_id);
                    }
                    getCommand.Parameters.AddWithValue("@role_id", role_id);
                    getCommand.Parameters.AddWithValue("@name", name);
                    getCommand.Parameters.AddWithValue("@email", email);
                    getCommand.Parameters.AddWithValue("@password", pass);
                    getCommand.Parameters.AddWithValue("@salt", salt);
                    getCommand.Parameters.AddWithValue("@avatar", avatar);
                    Debug.Write(getCommand.CommandText);
                    getCommand.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
                return false;
            }
        }

        public static bool courseSignup(string tableName, int user_id, int course_id)
        {
            string connectionString = getConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "INSERT INTO " + tableName +
                        "(`user_id`, `course_id`) "
                        + "VALUES(@user_id, @course_id)";                   
                    getCommand.Parameters.AddWithValue("@user_id", user_id);
                    getCommand.Parameters.AddWithValue("@course_id", course_id);
                    Debug.Write(getCommand.CommandText);
                    getCommand.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
                return false;
            }
        }

        public static bool courseSignoff(string tableName, int user_id, int course_id)
        {
            string connectionString = getConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "UPDATE" + tableName +
                        "SET `is_active`=0" +
                        "WHERE `user_id`=@user_id` AND `course_id`=@course_id";
                    getCommand.Parameters.AddWithValue("@user_id", user_id);
                    getCommand.Parameters.AddWithValue("@course_id", course_id);
                    Debug.Write(getCommand.CommandText);
                    getCommand.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
                return false;
            }
        }

        private static string getConnectionString()
        {
            EncodingProvider ppp;
            ppp = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(ppp);
            string connectionString = "Server = " + server + ";database = " + database + ";uid = " + user + ";password = " + pswd + ";SslMode=None;charset=utf8";
            return connectionString;
        }

    }
}

