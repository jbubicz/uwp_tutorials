﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Waf.UnitTesting;
using System.Waf.UnitTesting.Mocks;
using Waf.Writer.Applications.Controllers;
using Waf.Writer.Applications.Documents;
using Waf.Writer.Applications.Properties;
using Waf.Writer.Applications.Services;
using Test.Writer.Applications.Documents;
using Test.Writer.Applications.Services;
using Waf.Writer.Applications.ViewModels;

namespace Test.Writer.Applications.Controllers
{
    [TestClass]
    public class FileControllerTest : TestClassBase
    {
        [TestMethod]
        public void RegisterDocumentTypesTest()
        {
            FileController fileController = Container.GetExportedValue<FileController>();
            IFileService fileService = Container.GetExportedValue<IFileService>();

            MockDocumentType documentType = new MockDocumentType("Mock Document", ".mock");
            fileController.Register(documentType);

            Assert.IsFalse(fileService.Documents.Any());
            fileController.New(documentType);
            Assert.AreEqual(documentType, fileService.Documents.Single().DocumentType);
        }

        [TestMethod]
        public void NewAndActiveDocumentTest()
        {
            FileController fileController = Container.GetExportedValue<FileController>();
            IFileService fileService = Container.GetExportedValue<IFileService>();

            MockDocumentType documentType = new MockDocumentType("Mock Document", ".mock");
            fileController.Register(documentType);

            Assert.IsFalse(fileService.Documents.Any());
            Assert.IsNull(fileService.ActiveDocument);

            IDocument document = fileController.New(documentType);
            Assert.IsTrue(fileService.Documents.SequenceEqual(new[] { document }));
            Assert.AreEqual(document, fileService.ActiveDocument);

            AssertHelper.ExpectedException<ArgumentNullException>(() => fileController.New(null));
            AssertHelper.ExpectedException<ArgumentException>(() => fileController.New(new MockDocumentType("Dummy", ".dmy")));

            AssertHelper.PropertyChangedEvent(fileService, x => x.ActiveDocument, () => fileService.ActiveDocument = null);
            Assert.AreEqual(null, fileService.ActiveDocument);

            AssertHelper.ExpectedException<ArgumentException>(() => fileService.ActiveDocument = documentType.New());
        }

        [TestMethod]
        public void OpenDocumentTest()
        {
            MockFileDialogService fileDialogService = Container.GetExportedValue<MockFileDialogService>();
            FileController fileController = Container.GetExportedValue<FileController>();
            IFileService fileService = Container.GetExportedValue<IFileService>();

            MockDocumentType documentType = new MockDocumentType("Mock Document", ".mock");
            fileController.Register(documentType);

            Assert.IsFalse(fileService.Documents.Any());
            Assert.IsNull(fileService.ActiveDocument);

            fileDialogService.Result = new FileDialogResult("Document1.mock", new FileType("Mock Document", ".mock"));
            fileService.OpenCommand.Execute(null);

            Assert.AreEqual(FileDialogType.OpenFileDialog, fileDialogService.FileDialogType);
            Assert.AreEqual("Mock Document", fileDialogService.FileTypes.Last().Description);
            Assert.AreEqual(".mock", fileDialogService.FileTypes.Last().FileExtension);

            Assert.AreEqual(DocumentOperation.Open, documentType.DocumentOperation);
            Assert.AreEqual("Document1.mock", documentType.FileName);

            IDocument document = fileService.Documents.Last();
            Assert.AreEqual("Document1.mock", document.FileName);

            Assert.IsTrue(fileService.Documents.SequenceEqual(new[] { document }));
            Assert.AreEqual(document, fileService.ActiveDocument);
            
            // Open the same file again -> It's not opened again, just activated.

            fileService.ActiveDocument = null;
            fileService.OpenCommand.Execute("Document1.mock");
            Assert.IsTrue(fileService.Documents.SequenceEqual(new[] { document }));
            Assert.AreEqual(document, fileService.ActiveDocument);

            // Now the user cancels the OpenFileDialog box

            fileDialogService.Result = new FileDialogResult();
            int documentsCount = fileService.Documents.Count;
            fileService.OpenCommand.Execute(null);
            Assert.AreEqual(documentsCount, fileService.Documents.Count);

            Assert.IsTrue(fileService.Documents.SequenceEqual(new[] { document }));
            Assert.AreEqual(document, fileService.ActiveDocument);
        }

        [TestMethod]
        public void OpenDocumentViaCommandLineTest()
        {
            FileController fileController = Container.GetExportedValue<FileController>();
            IFileService fileService = Container.GetExportedValue<IFileService>();

            MockDocumentType documentType = new MockDocumentType("Mock Document", ".mock");
            fileController.Register(documentType);

            Assert.IsFalse(fileService.Documents.Any());
            Assert.IsNull(fileService.ActiveDocument);

            // Open is called with a fileName which might be a command line parameter.
            fileService.OpenCommand.Execute("Document1.mock");
            IDocument document = fileService.Documents.Last();
            Assert.AreEqual("Document1.mock", document.FileName);

            Assert.IsTrue(fileService.Documents.SequenceEqual(new[] { document }));
            Assert.AreEqual(document, fileService.ActiveDocument);

            // Call open with a fileName that has an invalid extension
            MockMessageService messageService = Container.GetExportedValue<MockMessageService>();
            messageService.Clear();
            fileService.OpenCommand.Execute("Document.wrongextension");
            Assert.AreEqual(MessageType.Error, messageService.MessageType);
            Assert.IsFalse(string.IsNullOrEmpty(messageService.Message));

            // Call open with a fileName that doesn't exist
            messageService.Clear();
            fileService.OpenCommand.Execute("2i0501fh-89f1-4197-a318-d5241135f4f6.rtf");
            Assert.AreEqual(MessageType.Error, messageService.MessageType);
            Assert.IsFalse(string.IsNullOrEmpty(messageService.Message));
        }

        [TestMethod]
        public void OpenExceptionsTest()
        {
            FileController fileController = Container.GetExportedValue<FileController>();
            FieldInfo documentTypesField = typeof(FileController).GetField("documentTypes", BindingFlags.Instance | BindingFlags.NonPublic);
            ((IList)documentTypesField.GetValue(fileController)).Clear();

            AssertHelper.ExpectedException<ArgumentException>(() => fileController.Open(null));

            IFileService fileService = Container.GetExportedValue<IFileService>();
            AssertHelper.ExpectedException<InvalidOperationException>(() => fileService.OpenCommand.Execute(null));
        }

        [TestMethod]
        public void OpenExceptionTestWithRecentFileList()
        {
            FileController fileController = Container.GetExportedValue<FileController>();
            IFileService fileService = Container.GetExportedValue<IFileService>();
            foreach (var recentFile in fileService.RecentFileList.RecentFiles.ToArray())
            {
                fileService.RecentFileList.Remove(recentFile);
            }
            
            MockDocumentType documentType = new MockDocumentType("Mock Document", ".mock");
            documentType.ThrowException = true;
            fileController.Register(documentType);

            fileService.RecentFileList.AddFile("Document1.mock");
            Assert.IsTrue(fileService.RecentFileList.RecentFiles.Any());
            
            fileService.OpenCommand.Execute("Document1.mock");
            
            // Ensure that the recent file is remove from the list.
            Assert.IsFalse(fileService.RecentFileList.RecentFiles.Any());
        }

        [TestMethod]
        public void SaveDocumentTest()
        {
            MockFileDialogService fileDialogService = Container.GetExportedValue<MockFileDialogService>();
            FileController fileController = Container.GetExportedValue<FileController>();
            IFileService fileService = Container.GetExportedValue<IFileService>();
            MockDocumentType documentType = new MockDocumentType("Mock Document", ".mock");

            fileController.Register(documentType);
            fileController.New(documentType);
            IDocument document = fileService.Documents.Single();
            document.FileName = "Document.mock";

            fileDialogService.Result = new FileDialogResult("Document1.mock", new FileType("Mock Document", ".mock"));
            fileService.SaveAsCommand.Execute(null);
            
            Assert.AreEqual(FileDialogType.SaveFileDialog, fileDialogService.FileDialogType);
            Assert.AreEqual("Mock Document", fileDialogService.FileTypes.Single().Description);
            Assert.AreEqual(".mock", fileDialogService.FileTypes.Single().FileExtension);
            Assert.AreEqual("Mock Document", fileDialogService.DefaultFileType.Description);
            Assert.AreEqual(".mock", fileDialogService.DefaultFileType.FileExtension);
            Assert.AreEqual("Document", fileDialogService.DefaultFileName);
            
            Assert.AreEqual(DocumentOperation.Save, documentType.DocumentOperation);
            Assert.AreEqual(document, documentType.Document);
            
            Assert.AreEqual("Document1.mock", documentType.FileName);

            // Change the CanSave to return false so that no documentType is able to save the document anymore

            documentType.CanSaveResult = false;
            Assert.IsFalse(fileService.SaveCommand.CanExecute(null));

            // Simulate an exception during the Save operation.

            MockMessageService messageService = Container.GetExportedValue<MockMessageService>();
            messageService.Clear();
            documentType.ThrowException = true;
            documentType.CanSaveResult = true;
            fileService.SaveAsCommand.Execute(null);
            Assert.AreEqual(MessageType.Error, messageService.MessageType);
            Assert.IsFalse(string.IsNullOrEmpty(messageService.Message));
        }

        [TestMethod]
        public void SaveDocumentWhenFileExistsTest()
        {
            // Get the absolute file path
            string fileName = Path.GetFullPath("SaveWhenFileExistsTest.mock");

            MockFileDialogService fileDialogService = Container.GetExportedValue<MockFileDialogService>();
            FileController fileController = Container.GetExportedValue<FileController>();
            IFileService fileService = Container.GetExportedValue<IFileService>();
            MockDocumentType documentType = new MockDocumentType("Mock Document", ".mock");
            fileController.Register(documentType);

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine("Hello World");
            }

            IDocument document = fileController.New(documentType);
            ((MockDocument)document).Modified = true;
            // We set the absoulte file path to simulate that we already saved the document
            document.FileName = fileName;

            fileService.SaveCommand.Execute(null);
            Assert.AreEqual(DocumentOperation.Save, documentType.DocumentOperation);
            Assert.AreEqual(document, documentType.Document);
            Assert.AreEqual(fileName, documentType.FileName);

            // Simulate the scenario when the user overwrites the existing file
            fileDialogService.Result = new FileDialogResult(fileName, new FileType("Mock Document", ".mock"));
            fileService.SaveAsCommand.Execute(null);
            Assert.AreEqual("Mock Document", fileDialogService.DefaultFileType.Description);
            Assert.AreEqual(".mock", fileDialogService.DefaultFileType.FileExtension);
            Assert.AreEqual(DocumentOperation.Save, documentType.DocumentOperation);
            Assert.AreEqual(document, documentType.Document);
            Assert.AreEqual(fileName, documentType.FileName);
        }

        [TestMethod]
        public void SaveExceptionsTest()
        {
            FileController fileController = Container.GetExportedValue<FileController>();
            MockDocumentType documentType = new MockDocumentType("Mock Document", ".mock");
            documentType.CanSaveResult = false;
            
            FieldInfo documentTypesField = typeof(FileController).GetField("documentTypes", BindingFlags.Instance | BindingFlags.NonPublic);
            ((IList)documentTypesField.GetValue(fileController)).Clear();
            fileController.Register(documentType);

            MethodInfo saveAsMethod = typeof(FileController).GetMethod("SaveAs", BindingFlags.Instance | BindingFlags.NonPublic);
            IDocument document = fileController.New(documentType);
            AssertHelper.ExpectedException<InvalidOperationException>(() =>
            {
                try
                {
                    saveAsMethod.Invoke(fileController, new[] { document });
                }
                catch (TargetInvocationException e)
                {
                    throw e.InnerException;
                }
            });
        }

        [TestMethod]
        public void OpenSaveDocumentTest()
        {
            MockFileDialogService fileDialogService = Container.GetExportedValue<MockFileDialogService>();
            IFileService fileService = Container.GetExportedValue<IFileService>();

            fileDialogService.Result = new FileDialogResult();
            fileService.OpenCommand.Execute(null);
            Assert.AreEqual(FileDialogType.OpenFileDialog, fileDialogService.FileDialogType);

            Assert.IsFalse(fileService.SaveCommand.CanExecute(null));
            Assert.IsFalse(fileService.SaveAsCommand.CanExecute(null));

            fileService.NewCommand.Execute(null);

            Assert.IsFalse(fileService.SaveCommand.CanExecute(null));
            Assert.IsTrue(fileService.SaveAsCommand.CanExecute(null));

            MainViewModel mainViewModel = Container.GetExportedValue<MainViewModel>();
            RichTextViewModel richTextViewModel = ViewHelper.GetViewModel<RichTextViewModel>((IView)mainViewModel.ActiveDocumentView);

            AssertHelper.CanExecuteChangedEvent(fileService.SaveCommand, () =>
                richTextViewModel.Document.Modified = true);

            Assert.IsTrue(fileService.SaveCommand.CanExecute(null));
            Assert.IsTrue(fileService.SaveAsCommand.CanExecute(null));

            fileDialogService.Result = new FileDialogResult();
            fileService.SaveCommand.Execute(null);
            Assert.AreEqual(FileDialogType.SaveFileDialog, fileDialogService.FileDialogType);
            Assert.IsTrue(richTextViewModel.Document.Modified);

            fileDialogService.Result = new FileDialogResult();
            fileService.SaveAsCommand.Execute(null);
            Assert.AreEqual(FileDialogType.SaveFileDialog, fileDialogService.FileDialogType);
        }

        [TestMethod]
        public void CloseDocumentTest()
        {
            FileController fileController = Container.GetExportedValue<FileController>();
            IFileService fileService = Container.GetExportedValue<IFileService>();
            MockDocumentType documentType = new MockDocumentType("Mock Document", ".mock");
            fileController.Register(documentType);

            fileController.New(documentType);
            fileService.CloseCommand.Execute(null);
            Assert.IsFalse(fileService.Documents.Any());
        }

        [TestMethod]
        public void UpdateCommandsTest()
        {
            IFileService documentManager = Container.GetExportedValue<IFileService>();

            documentManager.NewCommand.Execute(null);
            documentManager.NewCommand.Execute(null);
            documentManager.ActiveDocument = null;

            AssertHelper.CanExecuteChangedEvent(documentManager.CloseCommand, () =>
                documentManager.ActiveDocument = documentManager.Documents.First());
            AssertHelper.CanExecuteChangedEvent(documentManager.SaveCommand, () =>
                documentManager.ActiveDocument = documentManager.Documents.Last());
            AssertHelper.CanExecuteChangedEvent(documentManager.SaveAsCommand, () =>
                documentManager.ActiveDocument = documentManager.Documents.First());
        }
    }
}
