﻿using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows.Input;
using Waf.InformationManager.EmailClient.Modules.Applications.ViewModels;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;

namespace Waf.InformationManager.EmailClient.Modules.Applications.Controllers
{
    /// <summary>
    /// Responsible for an email account.
    /// </summary>
    [Export]
    internal class EmailAccountsController
    {
        private readonly IShellService shellService;
        private readonly ExportFactory<EditEmailAccountController> editEmailAccountControllerFactory;
        private readonly ExportFactory<EmailAccountsViewModel> emailAccountsViewModelFactory;
        private readonly DelegateCommand emailAccountsCommand;
        private DelegateCommand removeEmailAccountCommand;
        private DelegateCommand editEmailAccountCommand;
        private EmailAccountsViewModel emailAccountsViewModel;

        
        [ImportingConstructor]
        public EmailAccountsController(IShellService shellService, ExportFactory<EditEmailAccountController> editEmailAccountControllerFactory, 
            ExportFactory<EmailAccountsViewModel> emailAccountsViewModelFactory)
        {
            this.shellService = shellService;
            this.editEmailAccountControllerFactory = editEmailAccountControllerFactory;
            this.emailAccountsViewModelFactory = emailAccountsViewModelFactory;
            this.emailAccountsCommand = new DelegateCommand(ShowEmailAccounts);
        }


        public EmailClientRoot Root { get; set; }

        public ICommand EmailAccountsCommand => emailAccountsCommand;

        
        private void ShowEmailAccounts()
        {
            removeEmailAccountCommand = new DelegateCommand(RemoveEmailAccount, CanRemoveEmailAccount);
            editEmailAccountCommand = new DelegateCommand(EditEmailAccount, CanEditEmailAccount);

            emailAccountsViewModel = emailAccountsViewModelFactory.CreateExport().Value;
            emailAccountsViewModel.EmailClientRoot = Root;
            emailAccountsViewModel.NewAccountCommand = new DelegateCommand(NewEmailAccount);
            emailAccountsViewModel.RemoveAccountCommand = removeEmailAccountCommand;
            emailAccountsViewModel.EditAccountCommand = editEmailAccountCommand;

            PropertyChangedEventManager.AddHandler(emailAccountsViewModel, EmailAccountsViewModelPropertyChanged, "");

            emailAccountsViewModel.ShowDialog(shellService.ShellView);

            PropertyChangedEventManager.RemoveHandler(emailAccountsViewModel, EmailAccountsViewModelPropertyChanged, "");
            emailAccountsViewModel = null;
            removeEmailAccountCommand = null;
        }

        private void NewEmailAccount()
        {
            EditEmailAccountController editEmailAccountController = editEmailAccountControllerFactory.CreateExport().Value;
            editEmailAccountController.OwnerWindow = emailAccountsViewModel.View;
            editEmailAccountController.EmailAccount = new EmailAccount();

            editEmailAccountController.Initialize();
            if (editEmailAccountController.Run())
            {
                Root.AddEmailAccount(editEmailAccountController.EmailAccount);
            }
        }

        private bool CanRemoveEmailAccount() { return emailAccountsViewModel.SelectedEmailAccount != null; }

        private void RemoveEmailAccount()
        {
            Root.RemoveEmailAccount(emailAccountsViewModel.SelectedEmailAccount);
        }

        private bool CanEditEmailAccount() { return emailAccountsViewModel.SelectedEmailAccount != null; }

        private void EditEmailAccount()
        {
            var originalAccount = emailAccountsViewModel.SelectedEmailAccount;

            EditEmailAccountController editEmailAccountController = editEmailAccountControllerFactory.CreateExport().Value;
            editEmailAccountController.OwnerWindow = emailAccountsViewModel.View;
            editEmailAccountController.EmailAccount = originalAccount.Clone();

            editEmailAccountController.Initialize();
            if (editEmailAccountController.Run())
            {
                Root.ReplaceEmailAccount(originalAccount, editEmailAccountController.EmailAccount);
            }
        }

        private void EmailAccountsViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(EmailAccountsViewModel.SelectedEmailAccount))
            {
                removeEmailAccountCommand.RaiseCanExecuteChanged();
                editEmailAccountCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
