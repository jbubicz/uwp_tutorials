using System;
using Windows.UI.Xaml.Controls;
using MainMenu.View;
using Navigator;
using Navigator.Navigation;
using Prism.Commands;

namespace MainMenu.ViewModel
{
    public class BlankPage3ViewModel
    {
        public DelegateCommand NavigateCommand { get; set; }
        public DelegateCommand NavigateBackCommand { get; set; }

        public BlankPage3ViewModel(Frame frame)
        { 
            this.Initalize(frame);
        }

        private void Initalize(Frame frame)
        {
            this.NavigateCommand =
                new DelegateCommand(() =>
                NavigationWrapper.NavigationManager.NavigateFrame(
                    frame, typeof(BlankPage4).FullName));                    
        }
    }
}