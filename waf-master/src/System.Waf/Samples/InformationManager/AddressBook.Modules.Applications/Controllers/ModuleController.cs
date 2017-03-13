﻿using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Net.Mime;
using System.Runtime.Serialization;
using System.Waf.Applications;
using Waf.InformationManager.AddressBook.Interfaces.Applications;
using Waf.InformationManager.AddressBook.Interfaces.Domain;
using Waf.InformationManager.AddressBook.Modules.Applications.SampleData;
using Waf.InformationManager.AddressBook.Modules.Domain;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;

namespace Waf.InformationManager.AddressBook.Modules.Applications.Controllers
{
    /// <summary>
    /// Responsible for the whole module. This controller delegates the tasks to other controllers.
    /// </summary>
    [Export(typeof(IModuleController)), Export(typeof(IAddressBookService)), Export]
    internal class ModuleController : IModuleController, IAddressBookService
    {
        private const string documentPartPath = "AddressBook/Content.xml";
        
        private readonly IShellService shellService;
        private readonly IDocumentService documentService;
        private readonly INavigationService navigationService;
        private readonly ExportFactory<ContactController> contactControllerFactory;
        private readonly ExportFactory<SelectContactController> selectContactControllerFactory;
        private readonly Lazy<DataContractSerializer> serializer;
        private ContactController activeContactController;
        private AddressBookRoot root;

        
        [ImportingConstructor]
        public ModuleController(IShellService shellService, IDocumentService documentService, INavigationService navigationService,
            ExportFactory<ContactController> contactControllerFactory, ExportFactory<SelectContactController> selectContactControllerFactory)
        {
            this.shellService = shellService;
            this.documentService = documentService;
            this.navigationService = navigationService;
            this.contactControllerFactory = contactControllerFactory;
            this.selectContactControllerFactory = selectContactControllerFactory;
            this.serializer = new Lazy<DataContractSerializer>(CreateDataContractSerializer);
        }


        internal AddressBookRoot Root => root;


        public void Initialize()
        {
            using (var stream = documentService.GetStream(documentPartPath, MediaTypeNames.Text.Xml, FileMode.Open))
            {
                if (stream.Length == 0)
                {
                    root = new AddressBookRoot();
                    foreach (var contact in SampleDataProvider.CreateContacts()) { root.AddContact(contact); }
                }
                else
                {
                    root = (AddressBookRoot)serializer.Value.ReadObject(stream);
                }
            }
            
            navigationService.AddNavigationNode("Contacts", ShowAddressBook, CloseAddressBook, 2, 1);
        }

        public void Run()
        {   
        }

        public void Shutdown()
        {
            using (var stream = documentService.GetStream(documentPartPath, MediaTypeNames.Text.Xml, FileMode.Create))
            {
                serializer.Value.WriteObject(stream, root);
            }
        }

        public ContactDto ShowSelectContactView(object ownerView)
        {
            SelectContactController selectContactController = selectContactControllerFactory.CreateExport().Value;
            selectContactController.OwnerView = ownerView;
            selectContactController.Root = root;
            selectContactController.Initialize();
            selectContactController.Run();
            selectContactController.Shutdown();
            return selectContactController.SelectedContact.ToDto();
        }

        private void ShowAddressBook()
        {
            activeContactController = contactControllerFactory.CreateExport().Value;
            activeContactController.Root = root;
            activeContactController.Initialize();
            activeContactController.Run();

            ToolBarCommand uiNewContactCommand = new ToolBarCommand(activeContactController.NewContactCommand, "_New contact",
                "Creates a new contact.");
            ToolBarCommand uiDeleteCommand = new ToolBarCommand(activeContactController.DeleteContactCommand, "_Delete",
                "Deletes the selected contact.");
            shellService.AddToolBarCommands(new[] { uiNewContactCommand, uiDeleteCommand });
        }

        private void CloseAddressBook()
        {
            shellService.ClearToolBarCommands();

            activeContactController?.Shutdown();
            activeContactController = null;
        }

        private DataContractSerializer CreateDataContractSerializer()
        {
            return new DataContractSerializer(typeof(AddressBookRoot));
        }
    }
}
