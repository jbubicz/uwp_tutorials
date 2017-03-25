﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingPlatform.Models;

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
                    getCommand.CommandText = "SELECT * FROM " + tableName + " WHERE `is_enabled`=1 ORDER BY `modified` DESC";
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
                                course.Rate = getCourseRating(course.Id);
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
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public static ObservableCollection<Course> getTopCourses(string tableName)
        {
            ObservableCollection<Course> result = new ObservableCollection<Course>();
            string connectionString = getConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "SELECT * FROM " + tableName +
                        " c LEFT JOIN `courses_ratings` cr ON c.id = cr.course_id " +
                        "WHERE c.`is_enabled`=1 " +
                        "ORDER BY cr.rating, c.modified " +
                        "LIMIT 10";
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
                                course.Rate = getCourseRating(course.Id);
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
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public static ObservableCollection<Course> getCourse(string suggestion)
        {
            ObservableCollection<Course> result = new ObservableCollection<Course>();
            string connectionString = getConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "SELECT `id`, `user_id`, `category_id`, `title`, `price`, `img`, `short_description`, `description`, `is_enabled`, `created`, `modified` " +
                        "FROM `courses` " +
                        "WHERE `title` LIKE @suggestion AND `is_enabled`=1 " +
                        "ORDER BY `created` DESC ";
                    getCommand.Parameters.AddWithValue("@suggestion", "%" + suggestion + "%");
                    Debug.WriteLine(getCommand.CommandText);
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
                                course.Rate = getCourseRating(course.Id);
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
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public static ObservableCollection<Course> getNewestCourses(string tableName)
        {
            ObservableCollection<Course> result = new ObservableCollection<Course>();
            string connectionString = getConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "SELECT * FROM " + tableName + " WHERE `is_enabled`=1 " +
                        "ORDER BY `created` DESC " +
                        "LIMIT 10";
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
                                course.Rate = getCourseRating(course.Id);
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
                Debug.WriteLine(e.Message);
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
                    getCommand.CommandText = "SELECT * FROM " + tableName + " WHERE `is_enabled`=0";
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
                                course.Rate = getCourseRating(course.Id);
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
                Debug.WriteLine(e.Message);
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
                Debug.WriteLine(e.Message);
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
                Debug.WriteLine(e.Message);
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
                    getCommand.CommandText = "SELECT `id`, `role_id`, `name`, `email`, `avatar`, `about`, `points`, `created`, `modified` FROM `users` " +
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
                                user.Email = reader.GetString("email");
                                user.Avatar = reader.GetString("avatar");
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
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public static ObservableCollection<SectionsList> getSections(int course_id)
        {
            ObservableCollection<SectionsList> sections = new ObservableCollection<SectionsList>();
            sections.Clear();
            string connectionString = getConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "SELECT `id`, `course_id`, `section_order`, `title` " +
                        "FROM `courses_sections` " +
                        "WHERE course_id=@course_id " +
                        "ORDER BY section_order ASC";
                    getCommand.Parameters.AddWithValue("@course_id", course_id);
                    Debug.WriteLine(getCommand.CommandText);
                    using (MySqlDataReader reader = getCommand.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SectionsList section = new SectionsList();
                                section.Id = reader.GetInt32("id");
                                section.Course_id = reader.GetInt32("course_id");
                                section.Section_order = reader.GetInt32("section_order");
                                section.Title = reader.GetString("title");
                                sections.Add(section);
                            }
                        }
                    }
                    connection.Close();
                }
                return sections;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public static ObservableCollection<Lesson> getLessons(int course_id)
        {
            ObservableCollection<Lesson> lessons = new ObservableCollection<Lesson>();
            lessons.Clear();
            string connectionString = getConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "SELECT `id`, `user_id`, `section_id`,`course_id`, `img`, `video`, `title`, `free`, `description`, `lesson_order`, `is_enabled`, `created`, `modified` " +
                        "FROM `lessons` " +
                        "WHERE course_id=@course_id AND section_id=0 " +
                        "ORDER BY lesson_order ASC";
                    getCommand.Parameters.AddWithValue("@course_id", course_id);
                    Debug.WriteLine(getCommand.CommandText);
                    using (MySqlDataReader reader = getCommand.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                Lesson lesson = new Lesson();
                                lesson.Id = reader.GetInt32("id");
                                lesson.User_id = reader.GetInt32("user_id");
                                lesson.Section_id = reader.GetInt32("section_id");
                                lesson.Course_id = reader.GetInt32("course_id");
                                lesson.Img = reader.GetString("img");
                                lesson.Video = reader.GetString("video");
                                lesson.Lesson_title = reader.GetString("title");
                                lesson.Free = reader.GetInt32("free");
                                lesson.Description = reader.GetString("description");
                                lesson.Lesson_order = reader.GetInt32("lesson_order");
                                lesson.IsEnabled = reader.GetInt32("is_enabled");
                                lesson.Created = reader.GetDateTime("created");
                                lesson.Modified = reader.GetDateTime("modified");
                                lessons.Add(lesson);
                            }
                        }
                    }
                    connection.Close();
                }
                return lessons;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public static ObservableCollection<Lesson> getLessons(int course_id, int section_id)
        {
            ObservableCollection<Lesson> lessons = new ObservableCollection<Lesson>();
            lessons.Clear();
            string connectionString = getConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "SELECT `id`, `user_id`, `section_id`, `course_id`, `img`, `video`, `title`, `free`, `description`, `lesson_order`, `is_enabled`, `created`, `modified` " +
                        "FROM `lessons` " +
                        "WHERE course_id=@course_id AND section_id=@section_id " +
                        "ORDER BY lesson_order ASC";
                    getCommand.Parameters.AddWithValue("@course_id", course_id);
                    getCommand.Parameters.AddWithValue("@section_id", section_id);
                    Debug.WriteLine(getCommand.CommandText);
                    using (MySqlDataReader reader = getCommand.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                Lesson lesson = new Lesson();
                                lesson.Id = reader.GetInt32("id");
                                lesson.User_id = reader.GetInt32("user_id");
                                lesson.Section_id = reader.GetInt32("section_id");
                                lesson.Course_id = reader.GetInt32("course_id");
                                lesson.Img = reader.GetString("img");
                                lesson.Video = reader.GetString("video");
                                lesson.Lesson_title = reader.GetString("title");
                                lesson.Free = reader.GetInt32("free");
                                lesson.Description = reader.GetString("description");
                                lesson.Lesson_order = reader.GetInt32("lesson_order");
                                lesson.IsEnabled = reader.GetInt32("is_enabled");
                                lesson.Created = reader.GetDateTime("created");
                                lesson.Modified = reader.GetDateTime("modified");
                                lessons.Add(lesson);
                            }
                        }
                    }
                    connection.Close();
                }
                return lessons;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public static ObservableCollection<Comment> getComments(int lesson_id)
        {
            ObservableCollection<Comment> comments = new ObservableCollection<Comment>();
            comments.Clear();
            string connectionString = getConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "SELECT  comments.`id`,comments.`user_id`,users.name,comments.`lesson_id`,comments.`text`,comments.`created`,comments.`modified` " +
                        "FROM `lessons_comments` AS comments LEFT JOIN `users` AS users ON comments.user_id = users.id " +
                        "WHERE comments.`lesson_id`=@lesson_id " +
                        "ORDER BY comments.modified DESC";
                    getCommand.Parameters.AddWithValue("@lesson_id", lesson_id);
                    Debug.WriteLine(getCommand.CommandText);
                    using (MySqlDataReader reader = getCommand.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                Comment comment = new Comment();
                                comment.Id = reader.GetInt32("id");
                                comment.User_id = reader.GetInt32("user_id");
                                comment.Author = reader.GetString("name");
                                comment.Lesson_id = reader.GetInt32("lesson_id");
                                comment.Value = reader.GetString("text");
                                comment.Created = reader.GetDateTime("created");
                                comment.Modified = reader.GetDateTime("modified");
                                comments.Add(comment);
                            }
                        }
                    }
                    connection.Close();
                }
                return comments;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public static double getCourseRating(int course_id)
        {
            int rate = 0;
            double average = 0.0;
            List<int> result = new List<int>();
            string connectionString = getConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText =
                        "SELECT `id`, `user_id`, `course_id`, `rating`, `created` " +
                        "FROM `courses_ratings` " +
                        "WHERE `course_id` = @course_id";
                    getCommand.Parameters.AddWithValue("@course_id", course_id);
                    using (MySqlDataReader reader = getCommand.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                rate = reader.GetInt32("rating");
                                result.Add(rate);
                            }
                        }
                    }
                    connection.Close();
                }
                average = result.Count > 0 ? result.Average() : 0.0;
                return Math.Round(average, 1);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return 0.0;
            }
        }

        public static int getUserRate(int user_id, int course_id)
        {
            int rate = -1;
            string connectionString = getConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText =
                        "SELECT `id`, `user_id`, `course_id`, `rating`, `created` " +
                        "FROM `courses_ratings` " +
                        "WHERE `user_id`=@user_id AND `course_id` = @course_id";
                    getCommand.Parameters.AddWithValue("@user_id", user_id);
                    getCommand.Parameters.AddWithValue("@course_id", course_id);
                    Debug.WriteLine(getCommand.CommandText);
                    using (MySqlDataReader reader = getCommand.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                rate = reader.GetInt32("rating");
                            }
                        }
                    }
                    connection.Close();
                    return rate;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return rate;
            }
        }

        public static ObservableCollection<string> getSuggestions()
        {
            ObservableCollection<string> suggestions = new ObservableCollection<string>();
            string connectionString = getConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText =
                        "SELECT `title` " +
                        "FROM `courses` " +
                        "WHERE `is_enabled`=1 " +
                        "ORDER BY title ASC";
                    Debug.WriteLine(getCommand.CommandText);
                    using (MySqlDataReader reader = getCommand.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                string suggest = reader.GetString("title");
                                suggestions.Add(suggest);
                            }
                        }
                    }
                    connection.Close();
                    return suggestions;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public static bool checkIfFBUserIsSigned(string fb_id, int course_id)
        {
            bool isSigned = false;
            string connectionString = getConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "SELECT `is_active` FROM `courses_members` AS cm " +
                        "LEFT JOIN `users` AS u ON cm.user_id = u.id " +
                        "WHERE `fb_id`=@fb_id AND `course_id`=@course_id " +
                        "ORDER BY cm.created DESC " +
                        "LIMIT 1;";
                    getCommand.Parameters.AddWithValue("@fb_id", fb_id);
                    getCommand.Parameters.AddWithValue("@course_id", course_id);
                    using (MySqlDataReader reader = getCommand.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                isSigned = reader.GetBoolean("is_active");
                            }
                        }
                    }
                    connection.Close();
                }
                return isSigned;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public static bool checkIfFBUserExists(string fb_id)
        {
            bool exists = false;
            string connectionString = getConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "SELECT COUNT(`fb_id`) as if_exists FROM `users` " +
                        "WHERE `fb_id`=@fb_id";
                    getCommand.Parameters.AddWithValue("@fb_id", fb_id);
                    using (MySqlDataReader reader = getCommand.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                exists = reader.GetBoolean("if_exists");
                            }
                        }
                    }
                    connection.Close();
                }
                return exists;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
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
                        " (`user_id`, `category_id`, `title`, `price`, `img`, `short_description`, `description`) "
                        + "VALUES(@user_id, @category_id, @title, @price, @img, @short_description, @description)";
                    getCommand.Parameters.AddWithValue("@user_id", user_id);
                    getCommand.Parameters.AddWithValue("@category_id", cat_id);
                    getCommand.Parameters.AddWithValue("@title", title);
                    getCommand.Parameters.AddWithValue("@price", price);
                    getCommand.Parameters.AddWithValue("@img", img);
                    getCommand.Parameters.AddWithValue("@short_description", short_desc);
                    getCommand.Parameters.AddWithValue("@description", desc);
                    Debug.WriteLine(getCommand.CommandText);
                    getCommand.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public static bool updateCourse(string tableName, int course_id, int user_id, int cat_id, string title, string price, string img, string short_desc, string desc)
        {
            string connectionString = getConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "UPDATE " + tableName +
                        " SET `user_id`=@user_id, `category_id`=@category_id,`title`=@title,`price`=@price,`img`=@img,`short_description`=@short_description,`description`=@description " +
                        "WHERE `id`=@course_id";
                    getCommand.Parameters.AddWithValue("@user_id", user_id);
                    getCommand.Parameters.AddWithValue("@course_id", course_id);
                    getCommand.Parameters.AddWithValue("@category_id", cat_id);
                    getCommand.Parameters.AddWithValue("@title", title);
                    getCommand.Parameters.AddWithValue("@price", price);
                    getCommand.Parameters.AddWithValue("@img", img);
                    getCommand.Parameters.AddWithValue("@short_description", short_desc);
                    getCommand.Parameters.AddWithValue("@description", desc);
                    Debug.WriteLine(getCommand.CommandText);
                    getCommand.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
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
                    getCommand.CommandText = "UPDATE " + tableName +
                        " SET `is_enabled`=0 " +
                        "WHERE `id`=@course_id";
                    getCommand.Parameters.AddWithValue("@course_id", course_id);
                    Debug.WriteLine(getCommand.CommandText);
                    getCommand.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public static bool updateLesson(Lesson lesson)
        {
            string connectionString = getConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "UPDATE `lessons` " +
                        "SET `user_id`=@user_id, `section_id`=@section_id,`img`=@img, `title`=@title,`video`=@video,`free`=@free,`description`=@description,`lesson_order`=@lesson_order, `is_enabled`=@is_enabled " +
                        "WHERE `id`=@lesson_id";
                    getCommand.Parameters.AddWithValue("@user_id", lesson.User_id);
                    getCommand.Parameters.AddWithValue("@section_id", lesson.Section_id);
                    getCommand.Parameters.AddWithValue("@img", lesson.Img);
                    getCommand.Parameters.AddWithValue("@title", lesson.Lesson_title);
                    getCommand.Parameters.AddWithValue("@video", lesson.Video);
                    getCommand.Parameters.AddWithValue("@free", lesson.Free);
                    getCommand.Parameters.AddWithValue("@description", lesson.Description);
                    getCommand.Parameters.AddWithValue("@lesson_order", lesson.Lesson_order);
                    getCommand.Parameters.AddWithValue("@is_enabled", lesson.IsEnabled);
                    getCommand.Parameters.AddWithValue("@lesson_id", lesson.Id);
                    Debug.WriteLine(getCommand.CommandText);
                    getCommand.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
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
                            " (`role_id`, `name`, `email`, `password`, `salt`, `avatar`) "
                            + "VALUES(@role_id, @name, @email, @password, @salt, @avatar)";
                    }
                    else
                    {
                        getCommand.CommandText = "INSERT INTO " + tableName +
                        " (`fb_id`, `role_id`, `name`, `email`, `password`, `salt`, `avatar`) "
                        + "VALUES(@fb_id, @role_id, @name, @email, @password, @salt, @avatar)";
                        getCommand.Parameters.AddWithValue("@fb_id", fb_id);
                    }
                    getCommand.Parameters.AddWithValue("@role_id", role_id);
                    getCommand.Parameters.AddWithValue("@name", name);
                    getCommand.Parameters.AddWithValue("@email", email);
                    getCommand.Parameters.AddWithValue("@password", pass);
                    getCommand.Parameters.AddWithValue("@salt", salt);
                    getCommand.Parameters.AddWithValue("@avatar", avatar);
                    Debug.WriteLine(getCommand.CommandText);
                    getCommand.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
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
                        " (`user_id`, `course_id`) "
                        + "VALUES(@user_id, @course_id)";
                    getCommand.Parameters.AddWithValue("@user_id", user_id);
                    getCommand.Parameters.AddWithValue("@course_id", course_id);
                    Debug.WriteLine(getCommand.CommandText);
                    getCommand.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
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
                    getCommand.CommandText = "UPDATE " + tableName +
                        " SET `is_active`=0 " +
                        "WHERE `id` = (SELECT `id` " +
                        "WHERE `user_id`=@user_id AND `course_id`=@course_id " +
                        "ORDER BY created DESC " +
                        "LIMIT 1)";
                    getCommand.Parameters.AddWithValue("@user_id", user_id);
                    getCommand.Parameters.AddWithValue("@course_id", course_id);
                    Debug.WriteLine(getCommand.CommandText);
                    getCommand.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public static bool rateCourse(int user_id, int course_id, int rating)
        {
            int rate = getUserRate(user_id, course_id);
            string connectionString = getConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    if (rate >= 0)
                    {
                        getCommand.CommandText =
                        "UPDATE `courses_ratings` " +
                        "SET `rating`=@rating " +
                        "WHERE `user_id`=@user_id AND `course_id` = @course_id";
                    }
                    else
                    {
                        getCommand.CommandText =
                        "INSERT INTO `courses_ratings`(`user_id`, `course_id`, `rating`) " +
                        "VALUES(@user_id,@course_id,@rating)";
                    }
                    getCommand.Parameters.AddWithValue("@user_id", user_id);
                    getCommand.Parameters.AddWithValue("@course_id", course_id);
                    getCommand.Parameters.AddWithValue("@rating", rating);
                    Debug.WriteLine(getCommand.CommandText);
                    getCommand.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public static bool insertSection(ObservableCollection<SectionsList> sections, int course_id)
        {
            int i = 0;
            string connectionString = getConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    //getCommand.CommandText = "DELETE FROM `courses_sections` " +
                    //       "WHERE course_id=@course_id";
                    //getCommand.Parameters.AddWithValue("@course_id", course_id);
                    //Debug.WriteLine(getCommand.CommandText);
                    //getCommand.ExecuteNonQuery();
                    foreach (var section in sections)
                    {
                        if (section.Id == 0)
                        {
                            getCommand.CommandText = "INSERT INTO `courses_sections` " +
                                "(`course_id`, `section_order`, `title`) " +
                                "VALUES(@course_id" + i + ", @section_order" + i + ", @title" + i + ")";
                        }
                        else
                        {
                            getCommand.CommandText = "UPDATE `courses_sections` " +
                                "SET `section_order`=@section_order" + i + ", title=@title" + i + " " +
                                "WHERE `id`=@id" + i + " AND `course_id` = @course_id" + i;
                        }
                        getCommand.Parameters.AddWithValue("@id" + i, section.Id);
                        getCommand.Parameters.AddWithValue("@course_id" + i, section.Course_id);
                        getCommand.Parameters.AddWithValue("@section_order" + i, section.Section_order);
                        getCommand.Parameters.AddWithValue("@title" + i, section.Title);
                        Debug.WriteLine(getCommand.CommandText);
                        getCommand.ExecuteNonQuery();
                        i++;
                    }
                    connection.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public static bool insertLesson(Lesson lesson)
        {
            Lesson l = lesson;
            string connectionString = getConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand1 = connection.CreateCommand();
                    getCommand1.CommandText = "SELECT MAX(`lesson_order`) AS max_order FROM `lessons` " +
                        "WHERE course_id =@course_id1 AND section_id=@section_id1";
                    getCommand1.Parameters.AddWithValue("@section_id1", l.Section_id);
                    getCommand1.Parameters.AddWithValue("@course_id1", l.Course_id);
                    using (MySqlDataReader reader = getCommand1.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                int ordinal = reader.GetOrdinal("max_order");
                                if (reader.IsDBNull(ordinal))
                                {
                                    l.Lesson_order = 1;
                                }
                                else
                                {
                                    l.Lesson_order = reader.GetInt32("max_order") + 1;
                                }
                            }
                        }
                        else
                        {
                            l.Lesson_order = 1;
                        }
                    }
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "INSERT INTO `lessons` " +
                        "(`user_id`, `section_id`, `course_id`, `img`, `video`, `title`, `free`, `description`, `lesson_order`, `is_enabled`) "
                        + "VALUES(@user_id, @section_id2, @course_id2, @img, @video, @title, @free, @description, @lesson_order, @is_enabled)";
                    getCommand.Parameters.AddWithValue("@user_id", l.User_id);
                    getCommand.Parameters.AddWithValue("@section_id2", l.Section_id);
                    getCommand.Parameters.AddWithValue("@course_id2", l.Course_id);
                    getCommand.Parameters.AddWithValue("@img", l.Img);
                    getCommand.Parameters.AddWithValue("@video", l.Video);
                    getCommand.Parameters.AddWithValue("@title", l.Lesson_title);
                    getCommand.Parameters.AddWithValue("@free", l.Free);
                    getCommand.Parameters.AddWithValue("@description", l.Description);
                    getCommand.Parameters.AddWithValue("@lesson_order", l.Lesson_order);
                    getCommand.Parameters.AddWithValue("@is_enabled", l.IsEnabled);
                    Debug.WriteLine(getCommand.CommandText);
                    getCommand.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }


        public static bool insertComment(Comment comment)
        {
            string connectionString = getConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "INSERT INTO `lessons_comments` " +
                        " (`user_id`, `lesson_id`, `text`) "
                        + "VALUES(@user_id, @lesson_id, @text)";
                    getCommand.Parameters.AddWithValue("@user_id", comment.User_id);
                    getCommand.Parameters.AddWithValue("@lesson_id", comment.Lesson_id);
                    getCommand.Parameters.AddWithValue("@text", comment.Value);
                    Debug.WriteLine(getCommand.CommandText);
                    getCommand.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        private static string getConnectionString()
        {
            EncodingProvider ppp;
            ppp = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(ppp);
            string connectionString = "Server = " + server + ";database = " + database + ";uid = " +
                user + ";password = " + pswd + ";SslMode=None;charset=utf8";
            return connectionString;
        }

    }
}

