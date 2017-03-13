using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Prism.Commands;
using Navigator.Navigation;
using Notifications;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ArrangeTypeModifiers

namespace SRBD_UWP.ViewModel
{
    class MainPageViewModel
    {
        public DelegateCommand NavigateToGlobalMenuCommand { get; set; }
        public DelegateCommand ExitAppCommand { get; set; }
        public MainPageViewModel()
        {
            this.Initialize();
        }

        private void Initialize()
        {
            this.NavigateToGlobalMenuCommand =  new DelegateCommand(() =>
            NavigationWrapper.NavigationManager.NavigateFrameSilent(
                Window.Current.Content as Frame, typeof(GlobalMenu.View.MainPage).FullName));

            this.ExitAppCommand = new DelegateCommand(Exit);    
        }

        private static async void Exit()
        {
            if (await NotificationManager.YesNoAsyncDialog("Do you want to leave the application?", "Leaving the app"))
                Application.Current.Exit();
        }

    }
}
