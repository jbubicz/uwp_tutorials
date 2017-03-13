using System.Composition;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MainMenu.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MainMenu.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [Export(typeof(Page))]
    public sealed partial class MainPage : Page
    {
        private bool dataContextInitialzed;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            this.Loaded += (sender, args) =>
                           {
                               if (!this.dataContextInitialzed)
                               {
                                   this.DataContext = new MainMenuViewModel(this.Frame);
                                   this.dataContextInitialzed = true;
                               }
                           };

        }

        private async void Image_OnImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            await Notifications.NotificationManager.Notification("Failed to load "+ e?.ErrorMessage);
        }

        private async void Image_OnImageOpened(object sender, RoutedEventArgs e)
        {
            await Notifications.NotificationManager.Notification("Opened");
        }

        
    }
}

