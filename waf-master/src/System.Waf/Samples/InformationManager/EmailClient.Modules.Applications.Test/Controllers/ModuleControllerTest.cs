﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using Test.InformationManager.EmailClient.Modules.Applications.Views;
using Test.InformationManager.Infrastructure.Modules.Applications.Services;
using Waf.InformationManager.EmailClient.Modules.Applications.Controllers;

namespace Test.InformationManager.EmailClient.Modules.Applications.Controllers
{
    [TestClass]
    public class ModuleControllerTest : EmailClientTest
    {
        [TestMethod]
        public void SaveAndLoad()
        {
            var stream = new MasterMemoryStream();
            var documentService = Container.GetExportedValue<MockDocumentService>();
            documentService.GetStreamAction = (documentPartPath, contentType, fileMode) =>
            {
                stream.Position = 0;
                return stream;
            };

            var controller = Container.GetExportedValue<ModuleController>();
            controller.Initialize();
            controller.Run();
            var root1 = controller.Root;
            root1.EmailAccounts.First().Name = "Test name";
            controller.Shutdown();

            controller.Initialize();
            controller.Run();
            var root2 = controller.Root;
            Assert.AreNotSame(root1, root2);
            Assert.AreEqual("Test name", root2.EmailAccounts.First().Name);
            controller.Shutdown();

            stream.MasterDispose();
        }
        
        [TestMethod]
        public void ShowAndCloseEmailViews()
        {
            var controller = Container.GetExportedValue<ModuleController>();

            // Initialize the controller

            controller.Initialize();

            Assert.IsTrue(controller.Root.EmailAccounts.Any());
            Assert.IsTrue(controller.Root.Inbox.Emails.Any());
            Assert.IsTrue(controller.Root.Sent.Emails.Any());
            Assert.IsTrue(controller.Root.Drafts.Emails.Any());

            var navigationService = Container.GetExportedValue<MockNavigationService>();
            Assert.AreEqual(5, navigationService.NavigationNodes.Count());
            Assert.AreEqual("Inbox", navigationService.NavigationNodes.ElementAt(0).Name);
            Assert.AreEqual("Outbox", navigationService.NavigationNodes.ElementAt(1).Name);
            Assert.AreEqual("Sent", navigationService.NavigationNodes.ElementAt(2).Name);
            Assert.AreEqual("Drafts", navigationService.NavigationNodes.ElementAt(3).Name);
            Assert.AreEqual("Deleted", navigationService.NavigationNodes.ElementAt(4).Name);

            Assert.AreEqual(controller.Root.Inbox.Emails.Count, navigationService.NavigationNodes.ElementAt(0).ItemCount);
            Assert.AreEqual(controller.Root.Outbox.Emails.Count, navigationService.NavigationNodes.ElementAt(1).ItemCount);
            Assert.AreEqual(controller.Root.Sent.Emails.Count, navigationService.NavigationNodes.ElementAt(2).ItemCount);
            Assert.AreEqual(controller.Root.Drafts.Emails.Count, navigationService.NavigationNodes.ElementAt(3).ItemCount);
            Assert.AreEqual(controller.Root.Deleted.Emails.Count, navigationService.NavigationNodes.ElementAt(4).ItemCount);

            // Run the controller

            controller.Run();

            // Show the inbox

            var shellService = Container.GetExportedValue<MockShellService>();
            Assert.IsNull(shellService.ContentView);
            Assert.IsFalse(shellService.ToolBarCommands.Any());

            navigationService.NavigationNodes.ElementAt(0).ShowAction();

            Assert.IsNotNull(shellService.ContentView);
            Assert.AreEqual(3, shellService.ToolBarCommands.Count());

            navigationService.NavigationNodes.ElementAt(0).CloseAction();

            Assert.IsFalse(shellService.ToolBarCommands.Any());

            // Show the outbox

            navigationService.NavigationNodes.ElementAt(1).ShowAction();
            Assert.IsTrue(shellService.ToolBarCommands.Any());
            navigationService.NavigationNodes.ElementAt(1).CloseAction();
            Assert.IsFalse(shellService.ToolBarCommands.Any());

            // Show the sent emails

            navigationService.NavigationNodes.ElementAt(2).ShowAction();
            Assert.IsTrue(shellService.ToolBarCommands.Any());
            navigationService.NavigationNodes.ElementAt(2).CloseAction();
            Assert.IsFalse(shellService.ToolBarCommands.Any());

            // Show the drafts

            navigationService.NavigationNodes.ElementAt(3).ShowAction();
            Assert.IsTrue(shellService.ToolBarCommands.Any());
            navigationService.NavigationNodes.ElementAt(3).CloseAction();
            Assert.IsFalse(shellService.ToolBarCommands.Any());

            // Show the deleted emails

            navigationService.NavigationNodes.ElementAt(4).ShowAction();
            Assert.IsTrue(shellService.ToolBarCommands.Any());
            navigationService.NavigationNodes.ElementAt(4).CloseAction();
            Assert.IsFalse(shellService.ToolBarCommands.Any());

            // Shutdown the controller

            controller.Shutdown();
        }

        [TestMethod]
        public void ItemCountSynchronizerTest()
        {
            var controller = Container.GetExportedValue<ModuleController>();
            controller.Initialize();

            var inbox = controller.Root.Inbox;
            var navigationService = Container.GetExportedValue<MockNavigationService>();
            var inboxNode = navigationService.NavigationNodes.First();

            Assert.AreEqual(inbox.Emails.Count, inboxNode.ItemCount);
            inbox.RemoveEmail(inbox.Emails.First());
            Assert.AreEqual(inbox.Emails.Count, inboxNode.ItemCount);
        }

        [TestMethod]
        public void NewEmail()
        {
            var controller = Container.GetExportedValue<ModuleController>();
            controller.Initialize();

            var navigationService = Container.GetExportedValue<MockNavigationService>();
            var inboxNode = navigationService.NavigationNodes.First();
            
            inboxNode.ShowAction();
            
            var shellService = Container.GetExportedValue<MockShellService>();

            bool isShowCalled = false;
            MockNewEmailView.ShowAction = view =>
            {
                isShowCalled = true;
            };
            
            shellService.ToolBarCommands.First().Command.Execute(null);
            Assert.IsTrue(isShowCalled);
        }


        private class MasterMemoryStream : MemoryStream
        {
            protected override void Dispose(bool disposing)
            {
                // Do not dispose the stream now.
            }

            public void MasterDispose()
            {
                base.Dispose(true);
            }
        }
    }
}
