﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Windows.Input;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;

namespace Waf.InformationManager.EmailClient.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class EmailListViewModel : ViewModel<IEmailListView>
    {
        private IReadOnlyList<Email> emails;
        private Email selectedEmail;
        private ICommand deleteEmailCommand;
        private string filterText = "";

        
        [ImportingConstructor]
        public EmailListViewModel(IEmailListView view) : base(view)
        {
        }


        public IReadOnlyList<Email> Emails
        { 
            get { return emails; }
            set { SetProperty(ref emails, value); }
        }

        public Email SelectedEmail
        {
            get { return selectedEmail; }
            set { SetProperty(ref selectedEmail, value); }
        }

        public IEnumerable<Email> EmailCollectionView { get; set; }

        public ICommand DeleteEmailCommand
        {
            get { return deleteEmailCommand; }
            set { SetProperty(ref deleteEmailCommand, value); }
        }

        public string FilterText
        {
            get { return filterText; }
            set { SetProperty(ref filterText, value); }
        }


        public void FocusItem()
        {
            ViewCore.FocusItem();
        }

        public bool Filter(Email email)
        {
            return email.Title.IndexOf(FilterText, StringComparison.CurrentCultureIgnoreCase) >= 0
                || email.From.IndexOf(FilterText, StringComparison.CurrentCultureIgnoreCase) >= 0
                || email.To.Any(x => x.IndexOf(FilterText, StringComparison.CurrentCultureIgnoreCase) >= 0);
        }
    }
}
