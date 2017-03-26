using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TrainingPlatform.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.System.Display;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using winsdkfb;

namespace TrainingPlatform
{
    public sealed partial class ViewLesson : Page
    {
        static FBSession clicnt = FBSession.ActiveSession;
        private User user = getUserInfo(clicnt);
        private ObservableCollection<SectionsList> sections;
        private Lesson lesson;
        private int course_id;
        private ObservableCollection<Comment> comments;
        private DisplayRequest appDisplayRequest = null;
        private Database db = new Database();

        public ViewLesson()
        {
            this.InitializeComponent();
            if (clicnt.LoggedIn)
            {
                App.IsLogged = true;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            lesson = (Lesson)e.Parameter;
            ViewLessonForm.DataContext = lesson;
            EditLessonForm.DataContext = lesson;
            Uri pathUri = new Uri(lesson.Video);
            //Video.Source = MediaSource.CreateFromUri(pathUri);
            Video.Source = pathUri;
            course_id = lesson.Course_id;
            getSections();
            if (sections == null || sections.Count == 0)
            {
                SectionCombo.Visibility = Visibility.Collapsed;
            }
            else
            {
                SectionCombo.ItemsSource = sections;
                int actual_section_id = lesson.Section_id;
                var actual_section_index = from section in sections
                                      where section.Id == actual_section_id
                                      select sections.IndexOf(section);
                SectionCombo.SelectedItem = sections[actual_section_index.FirstOrDefault()];
            }
            getComments();
            lesson.comments = comments;
            CommentsList.ItemsSource = comments;
            setControlsVisibility(lesson);

            //Video.MediaPlayer.PlaybackSession.PlaybackStateChanged += MediaPlayerElement_CurrentStateChanged;
            //base.OnNavigatedTo(e);
        }

        private static User getUserInfo(FBSession clicnt)
        {
            if (App.IsLogged)
            {
                User user = new User();
                user = user.fbUser(clicnt.User.Id);
                return user;
            }
            else
                return null;
        }

        private ObservableCollection<Comment> getComments()
        {
            try
            {
                return comments = Database.getComments(lesson.Id);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error getting comments: " + e.ToString());
                return null;
            }
        }

        private void MediaPlayerElement_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            MediaPlaybackSession playbackSession = sender as MediaPlaybackSession;
            if (playbackSession != null && playbackSession.NaturalVideoHeight != 0)
            {
                if (playbackSession.PlaybackState == MediaPlaybackState.Playing)
                {
                    if (appDisplayRequest == null)
                    {
                        // This call creates an instance of the DisplayRequest object
                        appDisplayRequest = new DisplayRequest();
                        appDisplayRequest.RequestActive();
                    }
                }
                else // PlaybackState is Buffering, None, Opening or Paused
                {
                    if (appDisplayRequest != null)
                    {
                        // Deactivate the displayr request and set the var to null
                        appDisplayRequest.RequestRelease();
                        appDisplayRequest = null;
                    }
                }
            }
        }

        private void buckButton_Tapped(object sender, BackRequestedEventArgs e)
        {
        }

        private void setControlsVisibility(Lesson lesson)
        {
            if (lesson.Free == 1)
            {
                Free_checkbox.IsChecked = true;
                IsFree_label.Visibility = Visibility.Visible;
            }
            else
            {
                Free_checkbox.IsChecked = false;
                IsFree_label.Visibility = Visibility.Collapsed;
            }
            if (lesson.IsEnabled == 1)
            {
                Enable_checkbox.IsChecked = true;
            }
            else
            {
                Free_checkbox.IsChecked = false;
            }
            EditLessonForm.Visibility = Visibility.Visible;
            ViewLessonForm.Visibility = Visibility.Collapsed;
        }

        private async void SaveLesson_button_Click(object sender, RoutedEventArgs e)
        {
            lesson.Lesson_title = Title_box.Text;
            lesson.Description = Desc_box.Text;
            lesson.Free = Convert.ToInt32(Free_checkbox.IsChecked);
            lesson.IsEnabled = Convert.ToInt32(Enable_checkbox.IsChecked);
            var section_new = SectionCombo.SelectedItem as Section;
            lesson.Section_id = section_new.Id;
            string massage = "";
            if (Database.updateLesson(lesson))
            {
                massage = "Lesson updated successfully!";
            }
            else
            {
                massage = "Error while updating lesson!";
            }
            MessageDialog dialog = new MessageDialog(massage);
            await dialog.ShowAsync();
            Frame.Navigate(typeof(ViewLesson), lesson);
        }

        private ObservableCollection<SectionsList> getSections()
        {
            try
            {
                return sections = Database.getSections(course_id);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error inserting course section: " + e.ToString());
                return null;
            }
        }

        private void Edit_button_Click(object sender, RoutedEventArgs e)
        {
            Visibility visibility = ViewLessonForm.Visibility;
            ViewLessonForm.Visibility = EditLessonForm.Visibility;
            EditLessonForm.Visibility = visibility;
        }

        private void FullWindow_Click(object sender, RoutedEventArgs e)
        {
            Video.IsFullWindow = !Video.IsFullWindow;
        }

        private async void AddNewComment_button_Click(object sender, RoutedEventArgs e)
        {
            if (App.IsLogged)
                if (AddNewComment_textBox.Text != "")
                {
                    Comment comment = new Comment();
                    comment.User_id = user.Id;
                    comment.Lesson_id = lesson.Id;
                    comment.Value = AddNewComment_textBox.Text;
                    comment.Author = user.Name;
                    comment.Created = DateTime.Now;
                    comment.Modified = DateTime.Now;
                    Database.insertComment(comment);
                    comments.Insert(0, comment);
                    AddNewComment_textBox.Text = "";
                    AddNewComment_textBox.ClearValue(TextBox.BorderBrushProperty);
                }
                else
                {
                    AddNewComment_textBox.BorderBrush = new SolidColorBrush(Colors.Red);
                }
            else
            {
                string massage = "You must be signed in!";
                MessageDialog dialog = new MessageDialog(massage);
                await dialog.ShowAsync();
            }
        }
    }
}
