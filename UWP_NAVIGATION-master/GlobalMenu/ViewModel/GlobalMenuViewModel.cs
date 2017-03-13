using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Controls;
using GlobalMenu.View;
using MainMenu.Models;
using Navigator;
using Navigator.Annotations;
using Navigator.Navigation;
using Navigator.Navigation.History;
using Prism.Commands;
using PropertyChanged;

namespace GlobalMenu.ViewModel
{
   
    public class GlobalMenuViewModel
    {
        public NavigationFrame NavigationFrame { get; set; }

        #region DelegateCommands
        public DelegateCommand<HistoryRecord> NavigateCommand { get; set; }
        public DelegateCommand GoHome { get; set; }
        public DelegateCommand GoBack { get; set; }
        public DelegateCommand GoForward { get; set; }

        #endregion


        public GlobalMenuViewModel(Frame frame,Frame nestedFrame)
        {
            

            this.NavigationFrame = new NavigationFrame(frame,nestedFrame);

            NavigationWrapper.NavigationManager.NavigateFrame(nestedFrame, typeof(MainMenu.View.MainPage).FullName);
            

            this.InitializeDelegateCommands();

            NavigationWrapper.NavigationManager.NavigationEventHandler.EventHandler += (sender, args) =>
                                                                                {
                                                                                        this.GoBack.RaiseCanExecuteChanged();
                                                                                        this.GoForward.RaiseCanExecuteChanged();
                                                                                };

        }

        private void InitializeDelegateCommands()
        {
            this.NavigateCommand = new DelegateCommand<HistoryRecord>(
                (h) => NavigationWrapper.NavigationManager.NavigateFrame(this.NavigationFrame.CurrentFrame, h.PageName));

            this.GoBack = new DelegateCommand(() => NavigationWrapper.NavigationManager.NavigateBack(this.NavigationFrame.CurrentFrame,this.NavigationFrame.NestedFrame),
                                              () => NavigationWrapper.NavigationManager.CanNavigateBack());

            this.GoForward = new DelegateCommand(() => NavigationWrapper.NavigationManager.NavigateForward(this.NavigationFrame.NestedFrame),
                                                 () => NavigationWrapper.NavigationManager.CanNavigateForward());

            this.GoHome =  new DelegateCommand(()=>this.NavigationFrame.CurrentFrame.GoBack());
        }
    }
}