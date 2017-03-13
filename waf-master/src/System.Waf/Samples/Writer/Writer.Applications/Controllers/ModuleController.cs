﻿using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Threading;
using System.Waf.Applications;
using Waf.Writer.Applications.Properties;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.ViewModels;

namespace Waf.Writer.Applications.Controllers
{
    /// <summary>
    /// Responsible for the application lifecycle.
    /// </summary>
    [Export(typeof(IModuleController)), Export]
    internal class ModuleController : IModuleController
    {
        private readonly IEnvironmentService environmentService;
        private readonly FileController fileController;
        private readonly RichTextDocumentController richTextDocumentController;
        private readonly PrintController printController;
        private readonly ShellViewModel shellViewModel;
        private readonly MainViewModel mainViewModel;
        private readonly StartViewModel startViewModel;
        private readonly DelegateCommand exitCommand;

        
        [ImportingConstructor]
        public ModuleController(IEnvironmentService environmentService, IPresentationService presentationService, ShellService shellService,
            Lazy<FileController> fileController, Lazy<RichTextDocumentController> richTextDocumentController, Lazy<PrintController> printController,
            Lazy<ShellViewModel> shellViewModel, Lazy<MainViewModel> mainViewModel, Lazy<StartViewModel> startViewModel)
        {
            // Upgrade the settings from a previous version when the new version starts the first time.
            if (Settings.Default.IsUpgradeNeeded)
            {
                Settings.Default.Upgrade();
                Settings.Default.IsUpgradeNeeded = false;
            }

            // Initializing the cultures must be done first. Therefore, we inject the Controllers and ViewModels lazy.
            InitializeCultures();
            presentationService.InitializeCultures();

            this.environmentService = environmentService;
            this.fileController = fileController.Value;
            this.richTextDocumentController = richTextDocumentController.Value;
            this.printController = printController.Value;
            this.shellViewModel = shellViewModel.Value;
            this.mainViewModel = mainViewModel.Value;
            this.startViewModel = startViewModel.Value;

            shellService.ShellView = this.shellViewModel.View;
            this.shellViewModel.Closing += ShellViewModelClosing;
            this.exitCommand = new DelegateCommand(Close);
        }

        
        public void Initialize()
        {
            shellViewModel.ExitCommand = exitCommand;
            mainViewModel.StartView = startViewModel.View;
            
            printController.Initialize();
            fileController.Initialize();
        }

        public void Run()
        {
            shellViewModel.ContentView = mainViewModel.View;
            
            if (!string.IsNullOrEmpty(environmentService.DocumentFileName))
            {
                fileController.Open(environmentService.DocumentFileName);
            }
            
            shellViewModel.Show();
        }

        public void Shutdown()
        {
            fileController.Shutdown();
            printController.Shutdown();

            if (shellViewModel.NewLanguage != null) 
            { 
                Settings.Default.UICulture = shellViewModel.NewLanguage.Name; 
            }
            try
            {
                Settings.Default.Save();
            }
            catch (Exception)
            {
                // When more application instances are closed at the same time then an exception occurs.
            }
        }

        private static void InitializeCultures()
        {
            if (!string.IsNullOrEmpty(Settings.Default.Culture))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Settings.Default.Culture);
            }
            if (!string.IsNullOrEmpty(Settings.Default.UICulture))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.UICulture);
            }
        }

        private void Close()
        {
            shellViewModel.Close();
        }

        private void ShellViewModelClosing(object sender, CancelEventArgs e)
        {
            // Try to close all documents and see if the user has already saved them.
            e.Cancel = !fileController.CloseAll();
        }
    }
}
