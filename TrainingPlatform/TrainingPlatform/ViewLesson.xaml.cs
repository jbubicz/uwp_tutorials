using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TrainingPlatform.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.System.Display;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace TrainingPlatform
{
    public sealed partial class ViewLesson : Page
    {
        private ObservableCollection<SectionsList> sections;
        private Lesson lesson;
        private int course_id;
        private DisplayRequest appDisplayRequest = null;

        public ViewLesson()
        {
            this.InitializeComponent();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //var sections = (ObservableCollection<SectionsList>)e.Parameter;
            lesson = (Lesson)e.Parameter;
            LessonForm.DataContext = lesson;
            course_id = lesson.Course_id;
            sections = getSections();
            if (sections == null || sections.Count == 0)
            {
                SectionCombo.Visibility = Visibility.Collapsed;
            }
            else
            {
                SectionCombo.ItemsSource = sections;
            }
            Uri pathUri = new Uri(lesson.Video);
            Video.Source = MediaSource.CreateFromUri(pathUri);
            setControlsVisibility(lesson);

            //Video.MediaPlayer.PlaybackSession.PlaybackStateChanged += MediaPlayerElement_CurrentStateChanged;
            //base.OnNavigatedTo(e);
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
            if (lesson.Free==1)
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

        private void SaveLesson_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Back_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private ObservableCollection<SectionsList> getSections()
        {
            return sections = Database.getSections(course_id);
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

        private void AddNewComment_button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
