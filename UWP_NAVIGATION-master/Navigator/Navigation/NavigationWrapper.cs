using System;
using Windows.UI.Xaml.Controls;
using Navigator.Navigation;

namespace Navigator.Navigation
{
    public static class NavigationWrapper
    {
        private static Page[] pagesTemporaryStorage;

        private static readonly NavigationManagerCallEventHandler managerCallHandler;

        private static readonly Lazy<NavigationManager> LazyNavigationManager;

        public static NavigationManager NavigationManager
        {
            get
            {
                managerCallHandler.OnPreCall();
                var manager = LazyNavigationManager.Value;
                managerCallHandler.OnPostCall();
                return manager;
            }
        }

        static NavigationWrapper()
        {
            LazyNavigationManager = new Lazy<NavigationManager>();

            managerCallHandler = new NavigationManagerCallEventHandler();
            managerCallHandler.NavigationManagerPostCall += (sender, args) => OnNavigationManagerPostCall();
        }

        private static void OnNavigationManagerPostCall()
        {
            if (pagesTemporaryStorage != null)
            {
                LazyNavigationManager.Value.PagesManager.InitializePages(pagesTemporaryStorage);
                pagesTemporaryStorage = null;
            }
        }

        /// <summary>
        /// This methid is used to initialize Navigation Manager Pages
        /// </summary>
        /// <param name="pages">list of pages</param>
        public static void InitializePages(Page[] pages)
        {
            pagesTemporaryStorage = pages;
        }

        private class NavigationManagerCallEventHandler
        {
            internal event EventHandler NavigationManagerPreCall;
            internal event EventHandler NavigationManagerPostCall;

            internal void OnPreCall()
            {
                this.NavigationManagerPreCall?.Invoke(null,null);
            }

            internal void OnPostCall()
            {
                this.NavigationManagerPostCall?.Invoke(null, null);
            }
        }
    }
}
