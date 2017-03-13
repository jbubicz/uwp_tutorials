// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project. 
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc. 
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File". 
// You do not need to add suppressions to this file manually. 

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "New", Scope = "member", Target = "Waf.Writer.Applications.Documents.IDocumentType.#New()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Scope = "member", Target = "Waf.Writer.Applications.ModuleController.#richTextDocumentController")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Scope = "type", Target = "Waf.Writer.Applications.Controllers.PrintController")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Scope = "member", Target = "Waf.Writer.Applications.Controllers.ModuleController.#richTextDocumentController")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member", Target = "Waf.Writer.Applications.Controllers.PrintController.#ShowPrintPreview()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "CanNew", Scope = "member", Target = "Waf.Writer.Applications.Documents.DocumentType.#New()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "CanOpen", Scope = "member", Target = "Waf.Writer.Applications.Documents.DocumentType.#Open(System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "CanSave", Scope = "member", Target = "Waf.Writer.Applications.Documents.DocumentType.#Save(Waf.Writer.Applications.Documents.IDocument,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member", Target = "Waf.Writer.Applications.Documents.XpsExportDocumentType.#SaveCore(Waf.Writer.Applications.Documents.IDocument,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "Waf.Writer.Applications.ViewModels.ShellViewModel.#.ctor(Waf.Writer.Applications.Views.IShellView,Waf.Writer.Applications.Services.IShellService)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Waf.Writer.Applications.Controllers.ModuleController.#Shutdown()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "Waf.Writer.Applications.ViewModels.ShellViewModel.#.ctor(Waf.Writer.Applications.Views.IShellView,Waf.Writer.Applications.Services.IPresentationService,Waf.Writer.Applications.Services.IShellService)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Scope = "member", Target = "Waf.Writer.Applications.ViewModels.ShellViewModel.#.ctor(Waf.Writer.Applications.Views.IShellView,Waf.Writer.Applications.Services.IPresentationService,Waf.Writer.Applications.Services.IShellService)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "Waf.Writer.Applications.ViewModels.ShellViewModel.#Title")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "DocumentType", Scope = "member", Target = "Waf.Writer.Applications.Controllers.FileController.#Open()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Waf.Writer.Applications.Controllers.FileController.#OpenCore(System.String,System.Waf.Applications.Services.FileType)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "DocumentType", Scope = "member", Target = "Waf.Writer.Applications.Controllers.FileController.#SaveAs(Waf.Writer.Applications.Documents.IDocument)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Waf.Writer.Applications.Controllers.FileController.#SaveCore(Waf.Writer.Applications.Documents.IDocumentType,Waf.Writer.Applications.Documents.IDocument,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "SpellCheck", Scope = "member", Target = "Waf.Writer.Applications.ViewModels.RichTextViewModel.#IsSpellCheckEnabled")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "SpellCheck", Scope = "member", Target = "Waf.Writer.Applications.Services.IEditingCommands.#IsSpellCheckAvailable")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "SpellCheck", Scope = "member", Target = "Waf.Writer.Applications.Services.IEditingCommands.#IsSpellCheckEnabled")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "Waf.Writer.Applications.ViewModels.RichTextViewModel.#OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "Waf.Writer.Applications.ViewModels.ShellViewModel.#.ctor(Waf.Writer.Applications.Views.IShellView,System.Waf.Applications.Services.IMessageService,Waf.Writer.Applications.Services.IPresentationService,Waf.Writer.Applications.Services.IShellService,Waf.Writer.Applications.Services.IFileService)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "2", Scope = "member", Target = "Waf.Writer.Applications.ViewModels.ShellViewModel.#.ctor(Waf.Writer.Applications.Views.IShellView,System.Waf.Applications.Services.IMessageService,Waf.Writer.Applications.Services.IPresentationService,Waf.Writer.Applications.Services.IShellService,Waf.Writer.Applications.Services.IFileService)")]
