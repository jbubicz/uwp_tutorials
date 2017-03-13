﻿using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows.Input;
using Waf.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Applications.ViewModels
{
    [Export]
    public class PersonViewModel : ViewModel<IPersonView>
    {
        private bool isValid = true;
        private Person person;
        private ICommand createNewEmailCommand;

        
        [ImportingConstructor]
        public PersonViewModel(IPersonView view)
            : base(view)
        {
        }


        public bool IsEnabled => Person != null;

        public bool IsValid
        {
            get { return isValid; }
            set { SetProperty(ref isValid, value); }
        }

        public Person Person
        {
            get { return person; }
            set
            {
                if (SetProperty(ref person, value))
                {
                    RaisePropertyChanged(nameof(IsEnabled));
                }
            }
        }

        public ICommand CreateNewEmailCommand
        {
            get { return createNewEmailCommand; }
            set { SetProperty(ref createNewEmailCommand, value); }
        }
    }
}
