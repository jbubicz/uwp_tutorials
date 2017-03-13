using System;

namespace Navigator.NavigationEventsHandler
{
    public sealed class NavigationEventHandler
    {
        public event EventHandler EventHandler;

        public void OnNavigated(string pageFullName)
        {
            this.EventHandler?.Invoke(this,new NavigationArgs(pageFullName));
        }
    }
}
