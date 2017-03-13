﻿using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Waf.Applications;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using Waf.InformationManager.EmailClient.Modules.Domain.AccountSettings;

namespace Waf.InformationManager.EmailClient.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class Pop3SettingsViewModel : ViewModel<IPop3SettingsView>
    {
        private Pop3Settings model;
        private bool useSameUserCredits;


        [ImportingConstructor]
        public Pop3SettingsViewModel(IPop3SettingsView view) : base(view)
        {
        }


        public Pop3Settings Model 
        { 
            get { return model; }
            set
            {
                if (SetProperty(ref model, value))
                {
                    PropertyChangedEventManager.AddHandler(model.Pop3UserCredits, Pop3UserCreditsPropertyChanged, "");
                }
            }
        }

        public bool UseSameUserCredits
        {
            get { return useSameUserCredits; }
            set
            {
                if (SetProperty(ref useSameUserCredits, value))
                {
                    if (useSameUserCredits)
                    {
                        Model.SmtpUserCredits.UserName = Model.Pop3UserCredits.UserName;
                        Model.SmtpUserCredits.Password = Model.Pop3UserCredits.Password;
                    }
                }
            }
        }

        private void Pop3UserCreditsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(UserCredits.UserName))
            {
                if (UseSameUserCredits)
                {
                    Model.SmtpUserCredits.UserName = Model.Pop3UserCredits.UserName;
                }
            }
            else if (e.PropertyName == nameof(UserCredits.Password))
            {
                if (UseSameUserCredits)
                {
                    Model.SmtpUserCredits.Password = Model.Pop3UserCredits.Password;
                }
            }
        }
    }
}
