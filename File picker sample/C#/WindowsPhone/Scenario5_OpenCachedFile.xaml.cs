﻿//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using SDKTemplate;

using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace FilePicker
{
    /// <summary>
    /// Implement IFileOpenPickerContinuable interface, in order that Continuation Manager can automatically
    /// trigger the method to process returned files.
    /// </summary>
    public sealed partial class Scenario5 : Page, IFileOpenPickerContinuable
    {
        MainPage rootPage = MainPage.Current;
        string fileToken = string.Empty;

        public Scenario5()
        {
            this.InitializeComponent();
            PickFileButton.Click += new RoutedEventHandler(PickFileButton_Click);
            OutputFileButton.Click += new RoutedEventHandler(OutputFileButton_Click);
        }

        private void PickFileButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear previous returned file content, if it exists, between iterations of this scenario
            OutputFileName.Text = "";
            OutputFileContent.Text = "";
            rootPage.NotifyUser("", NotifyType.StatusMessage);

            Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Clear();
            fileToken = string.Empty;

            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.FileTypeFilter.Add(".txt");

            openPicker.PickSingleFileAndContinue();
        }

        /// <summary>
        /// Handle the returned files from file picker
        /// This method is triggered by ContinuationManager based on ActivationKind
        /// </summary>
        /// <param name="args">File open picker continuation activation argment. It cantains the list of files user selected with file open picker </param>
        public void ContinueFileOpenPicker(FileOpenPickerContinuationEventArgs args)
        {
            if (args.Files.Count > 0)
            {
                StorageFile file = args.Files[0];
                fileToken = Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(file);
                OutputFileButton.IsEnabled = true;
                OutputFileAsync(file);
            }
            else
            {
                rootPage.NotifyUser("Operation cancelled.", NotifyType.StatusMessage);
            }
        }

        private async void OutputFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(fileToken))
            {
                rootPage.NotifyUser("", NotifyType.StatusMessage);

                // Windows will call the server app to update the local version of the file
                try
                {
                    StorageFile file = await Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.GetFileAsync(fileToken);
                    OutputFileAsync(file);
                }
                catch (UnauthorizedAccessException)
                {
                    rootPage.NotifyUser("Access is denied.", NotifyType.ErrorMessage);
                }
            }
        }

        private async void OutputFileAsync(StorageFile file)
        {
            string fileContent = await FileIO.ReadTextAsync(file);
            OutputFileName.Text = String.Format(@"Received file: {0}", file.Name);
            OutputFileContent.Text = String.Format(@"File content:{0}{1}", System.Environment.NewLine, fileContent);
        }
    }
}
