using System;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Navigator.NavigationEventsHandler
{
    public class NavigationArgs : EventArgs
    {
        public NavigationArgs() : this(null) { }
        public NavigationArgs(string navigatedPageFullName)
        {
            this.NavigatedPageFullName = navigatedPageFullName;
        }
        public string NavigatedPageFullName { get; set; }
    }
}