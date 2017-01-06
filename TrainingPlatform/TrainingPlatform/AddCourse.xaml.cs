using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Web;
using Windows.Web.Http;

namespace TrainingPlatform
{

    public sealed partial class AddCourse : Page, IDisposable
    {
        ObservableCollection<CategoriesList> categories = Database.getCategories("courses_categories");

        // A pointer back to the main page.  This is needed if you want to call methods in MainPage such
        // as NotifyUser()
        private HttpClient httpClient;
        private Stream stream = new MemoryStream();
        private CancellationTokenSource cts;
        

        private const int maxUploadFileSize = 100 * 1024 * 1024; // 100 MB (arbitrary limit chosen for this sample)

        public AddCourse()
        {
            this.InitializeComponent();
            CatCombo.ItemsSource = categories;
            CatCombo.SelectedItem = categories[0];
        }

        private Course getCourseDetails()
        {
            var cat = CatCombo.SelectedItem as CategoriesList;
            int cat_id = cat.Id;
            string title = title_box.Text;
            string short_desc = short_desc_box.Text;
            string desc = desc_box.Text;
            string price = price_box.Text;
            string img = "Assets/1.jpg";
            Course new_course = new Course { UserId = 1, Category = cat_id, Title = title, Price = price, ImgUrl = img, ShortDescription = short_desc, Description = desc, IsEnabled = 1 };
            return new_course;
        }

        private async void Upload_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker open = new FileOpenPicker();
            open.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            open.ViewMode = PickerViewMode.Thumbnail;

            // Filter to include a sample subset of file types
            open.FileTypeFilter.Clear();
            open.FileTypeFilter.Add(".bmp");
            open.FileTypeFilter.Add(".png");
            open.FileTypeFilter.Add(".jpeg");
            open.FileTypeFilter.Add(".jpg");

            // Open a stream for the selected file
            StorageFile file = await open.PickSingleFileAsync();
            // Ensure a file was selected
            if (file != null)
            {
                // Ensure the stream is disposed once the image is loaded
                using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    await bitmapImage.SetSourceAsync(fileStream);
                    fileStream.AsStream().CopyTo(stream);
                    //img.Source = bitmapImage;
                }
            }

            Uri uri = new Uri(serverAddressField.Text);
            Windows.Web.Http.HttpClient client = new Windows.Web.Http.HttpClient();
            HttpStreamContent streamContent = new HttpStreamContent(stream.AsInputStream());
            Windows.Web.Http.HttpRequestMessage request = new Windows.Web.Http.HttpRequestMessage(Windows.Web.Http.HttpMethod.Post, uri);
            request.Content = streamContent;
            Windows.Web.Http.HttpResponseMessage response = await client.PostAsync(uri, streamContent).AsTask(cts.Token);
        }

        private async void DoDownloadOrUpload(bool isDownload)
        {
            Uri uri = new Uri(serverAddressField.Text);
            string Username = "FTP-User";
            string Password = "123456789";

            FtpClient client = new FtpClient();
            await client.ConnectAsync(
                new HostName(uri.Host),
                uri.Port.ToString(),
                Username,
                Password);

            if (isDownload)
            {    
                byte[] data = await client.DownloadAsync(uri.AbsolutePath);              
            }
            else
            {
                byte[] data = Encoding.UTF8.GetBytes(serverAddressField.Text);
                await client.UploadAsync(uri.AbsolutePath, data);
            }          
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void add_course_button_Click(object sender, RoutedEventArgs e)
        {
            Course course = getCourseDetails();
            int user_id = course.UserId;
            int cat_id = course.Category;
            string title = course.Title;
            string price = course.Price;
            string img = course.ImgUrl;
            string short_desc = course.ShortDescription;
            string desc = course.Description;
            bool inserted = Database.insertCourse("courses", user_id, cat_id, title, price, img, short_desc, desc);
            if (inserted)
            {
                string success = "Course successfully added!";
                MessageDialog dialog = new MessageDialog(success);
                await dialog.ShowAsync();
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (EnsureUnsnapped())
            {
                FileSavePicker savePicker = new FileSavePicker();
                savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                // Dropdown of file types the user can save the file as
                savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
                // Default file name if the user does not type one in or select a file to replace
                savePicker.SuggestedFileName = "New Document";

                StorageFile file = await savePicker.PickSaveFileAsync();
                if (file != null)
                {
                    // Prevent updates to the remote version of the file until we finish making changes and call CompleteUpdatesAsync.
                    CachedFileManager.DeferUpdates(file);
                    // write to file
                    await FileIO.WriteTextAsync(file, file.Name);
                    // Let Windows know that we're finished changing the file so the other app can update the remote version of the file.
                    // Completing updates may require Windows to ask for user input.
                    FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                    if (status == FileUpdateStatus.Complete)
                    {
                        OutputTextBlock.Text = "File " + file.Name + " was saved.";
                    }
                    else
                    {
                        OutputTextBlock.Text = "File " + file.Name + " couldn't be saved.";
                    }
                }
                else
                {
                    OutputTextBlock.Text = "Operation cancelled.";
                }
            }
        }

        internal bool EnsureUnsnapped()
        {
            // FilePicker APIs will not work if the application is in a snapped state.
            // If an app wants to show a FilePicker while snapped, it must attempt to unsnap first
#pragma warning disable CS0618 // Type or member is obsolete
            bool unsnapped = ((ApplicationView.Value != ApplicationViewState.Snapped) || ApplicationView.TryUnsnap());
#pragma warning restore CS0618 // Type or member is obsolete
            if (!unsnapped)
            {
                NotifyUser("Cannot unsnap the sample.", NotifyType.StatusMessage);
            }

            return unsnapped;
        }

        public void NotifyUser(string strMessage, NotifyType type)
        {
            switch (type)
            {
                // Use the status message style. 
                case NotifyType.StatusMessage:
                    StatusBlock.Style = Resources["StatusStyle"] as Style;
                    break;
                // Use the error message style. 
                case NotifyType.ErrorMessage:
                    StatusBlock.Style = Resources["ErrorStyle"] as Style;
                    break;
            }
            StatusBlock.Text = strMessage;

            // Collapse the StatusBlock if it has no text to conserve real estate. 
            if (StatusBlock.Text != String.Empty)
            {
                StatusBlock.Visibility = Visibility.Visible;
            }
            else
            {
                StatusBlock.Visibility = Visibility.Collapsed;
            }
        }
        public enum NotifyType
        {
            StatusMessage,
            ErrorMessage
        };

        


        public void Dispose()
        {
            if (cts != null)
            {
                cts.Dispose();
                cts = null;
            }

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Helpers.CreateHttpClient(ref httpClient);
            cts = new CancellationTokenSource();
        }

        // Enumerate the uploads that were going on in the background while the app was closed.
        private async Task DiscoverActiveUploadsAsync()
        {
            IReadOnlyList<UploadOperation> uploads = null;
            try
            {
                uploads = await BackgroundUploader.GetCurrentUploadsAsync();
            }
            catch (Exception ex)
            {
                if (!IsExceptionHandled("Discovery error", ex))
                {
                    throw;
                }
                return;
            }

            Log("Loading background uploads: " + uploads.Count);

            if (uploads.Count > 0)
            {
                List<Task> tasks = new List<Task>();
                foreach (UploadOperation upload in uploads)
                {
                    Log(String.Format(CultureInfo.CurrentCulture, "Discovered background upload: {0}, Status: {1}",
                        upload.Guid, upload.Progress.Status));

                    // Attach progress and completion handlers.
                    tasks.Add(HandleUploadAsync(upload, false));
                }

                // Don't await HandleUploadAsync() in the foreach loop since we would attach to the second
                // upload only when the first one completed; attach to the third upload when the second one
                // completes etc. We want to attach to all uploads immediately.
                // If there are actions that need to be taken once uploads complete, await tasks here, outside
                // the loop.
                await Task.WhenAll(tasks);
            }
        }

        private void StartUpload_Click(object sender, RoutedEventArgs e)
        {
            // Validating the URI is required since it was received from an untrusted source (user input).
            // The URI is validated by calling Uri.TryCreate() that will return 'false' for strings that are not valid URIs.
            // Note that when enabling the text box users may provide URIs to machines on the intrAnet that require
            // the "Home or Work Networking" capability.
            Uri uri;
            if (!Uri.TryCreate(serverAddressField.Text.Trim(), UriKind.Absolute, out uri))
            {
                NotifyUser("Invalid URI.", NotifyType.ErrorMessage);
                return;
            }

            FileOpenPicker picker = new FileOpenPicker();
            picker.FileTypeFilter.Add("*");

#if WINDOWS_PHONE_APP
            picker.ContinuationData.Add("uri", uri.OriginalString);
            picker.PickSingleFileAndContinue();
#else
            StartSingleFileUpload(picker, uri);
#endif
        }

        private async void UploadSingleFile(Uri uri, StorageFile file)
        {
            if (file == null)
            {
               NotifyUser("No file selected.", NotifyType.ErrorMessage);
                return;
            }

            BasicProperties properties = await file.GetBasicPropertiesAsync();
            if (properties.Size > maxUploadFileSize)
            {
                NotifyUser(String.Format(CultureInfo.CurrentCulture,
                    "Selected file exceeds max. upload file size ({0} MB).", maxUploadFileSize / (1024 * 1024)),
                    NotifyType.ErrorMessage);
                return;
            }

            BackgroundUploader uploader = new BackgroundUploader();
            uploader.SetRequestHeader("Filename", file.Name);

            UploadOperation upload = uploader.CreateUpload(uri, file);
            Log(String.Format(CultureInfo.CurrentCulture, "Uploading {0} to {1}, {2}", file.Name, uri.AbsoluteUri,
                upload.Guid));

            // Attach progress and completion handlers.
            await HandleUploadAsync(upload, true);
        }

        private void StartMultipartUpload_Click(object sender, RoutedEventArgs e)
        {
            // By default 'serverAddressField' is disabled and URI validation is not required. When enabling the text
            // box validating the URI is required since it was received from an untrusted source (user input).
            // The URI is validated by calling Uri.TryCreate() that will return 'false' for strings that are not valid URIs.
            // Note that when enabling the text box users may provide URIs to machines on the intrAnet that require
            // the "Home or Work Networking" capability.
            Uri uri;
            if (!Uri.TryCreate(serverAddressField.Text.Trim(), UriKind.Absolute, out uri))
            {
                NotifyUser("Invalid URI.", NotifyType.ErrorMessage);
                return;
            }

            FileOpenPicker picker = new FileOpenPicker();
            picker.FileTypeFilter.Add("*");

#if WINDOWS_PHONE_APP
            picker.ContinuationData.Add("uri", uri.OriginalString);
            picker.PickMultipleFilesAndContinue();
#else
            StartMultipleFilesUpload(picker, uri);
#endif
        }

#if WINDOWS_PHONE_APP
        public void ContinueFileOpenPicker(Windows.ApplicationModel.Activation.FileOpenPickerContinuationEventArgs args)
        {
            Uri uri;
            string uriString = args.ContinuationData["uri"] as string;
            if (Uri.TryCreate(uriString, UriKind.Absolute, out uri))
            {
                if (args.Files.Count == 1)
                {
                    UploadSingleFile(uri, args.Files[0]);
                }
                else
                {
                    UploadMultipleFiles(uri, args.Files);
                }
            }
        }
#else
        private async void StartSingleFileUpload(FileOpenPicker picker, Uri uri)
        {
            StorageFile file = await picker.PickSingleFileAsync();
            UploadSingleFile(uri, file);
        }

        private async void StartMultipleFilesUpload(FileOpenPicker picker, Uri uri)
        {
            IReadOnlyList<StorageFile> files = await picker.PickMultipleFilesAsync();
            UploadMultipleFiles(uri, files);
        }
#endif

        private async void UploadMultipleFiles(Uri uri, IReadOnlyList<StorageFile> files)
        {
            if (files.Count == 0)
            {
                NotifyUser("No file selected.", NotifyType.ErrorMessage);
                return;
            }

            ulong totalFileSize = 0;
            for (int i = 0; i < files.Count; i++)
            {
                BasicProperties properties = await files[i].GetBasicPropertiesAsync();
                totalFileSize += properties.Size;

                if (totalFileSize > maxUploadFileSize)
                {
                    NotifyUser(String.Format(CultureInfo.CurrentCulture,
                        "Size of selected files exceeds max. upload file size ({0} MB).",
                        maxUploadFileSize / (1024 * 1024)), NotifyType.ErrorMessage);
                    return;
                }
            }

            List<BackgroundTransferContentPart> parts = new List<BackgroundTransferContentPart>();
            for (int i = 0; i < files.Count; i++)
            {
                BackgroundTransferContentPart part = new BackgroundTransferContentPart("File" + i, files[i].Name);
                part.SetFile(files[i]);
                parts.Add(part);
            }

            BackgroundUploader uploader = new BackgroundUploader();
            UploadOperation upload = await uploader.CreateUploadAsync(uri, parts);

            String fileNames = files[0].Name;
            for (int i = 1; i < files.Count; i++)
            {
                fileNames += ", " + files[i].Name;
            }

            Log(String.Format(CultureInfo.CurrentCulture, "Uploading {0} to {1}, {2}", fileNames, uri.AbsoluteUri,
                upload.Guid));

            // Attach progress and completion handlers.
            await HandleUploadAsync(upload, true);
        }

        private void CancelAll_Click(object sender, RoutedEventArgs e)
        {
            Log("Canceling all active uploads");

            cts.Cancel();
            cts.Dispose();

            // Re-create the CancellationTokenSource and activeUploads for future uploads.
            cts = new CancellationTokenSource();
        }

        // Note that this event is invoked on a background thread, so we cannot access the UI directly.
        private void UploadProgress(UploadOperation upload)
        {
            MarshalLog(String.Format(CultureInfo.CurrentCulture, "Progress: {0}, Status: {1}", upload.Guid,
                upload.Progress.Status));

            BackgroundUploadProgress progress = upload.Progress;

            double percentSent = 100;
            if (progress.TotalBytesToSend > 0)
            {
                percentSent = progress.BytesSent * 100 / progress.TotalBytesToSend;
            }

            MarshalLog(String.Format(CultureInfo.CurrentCulture,
                " - Sent bytes: {0} of {1} ({2}%), Received bytes: {3} of {4}", progress.BytesSent,
                progress.TotalBytesToSend, percentSent, progress.BytesReceived, progress.TotalBytesToReceive));

            if (progress.HasRestarted)
            {
                MarshalLog(" - Upload restarted");
            }

            if (progress.HasResponseChanged)
            {
                // We've received new response headers from the server.
                MarshalLog(" - Response updated; Header count: " + upload.GetResponseInformation().Headers.Count);

                // If you want to stream the response data this is a good time to start.
                 upload.GetResultStreamAt(0);
            }
        }

        private async Task HandleUploadAsync(UploadOperation upload, bool start)
        {
            try
            {
                LogStatus("Running: " + upload.Guid, NotifyType.StatusMessage);

                Progress<UploadOperation> progressCallback = new Progress<UploadOperation>(UploadProgress);
                if (start)
                {
                    // Start the upload and attach a progress handler.
                    await upload.StartAsync().AsTask(cts.Token, progressCallback);
                }
                else
                {
                    // The upload was already running when the application started, re-attach the progress handler.
                    await upload.AttachAsync().AsTask(cts.Token, progressCallback);
                }

                ResponseInformation response = upload.GetResponseInformation();

                LogStatus(string.Format(CultureInfo.CurrentCulture, "Completed: {0}, Status Code: {1}", upload.Guid, response.StatusCode), NotifyType.StatusMessage);
            }
            catch (TaskCanceledException)
            {
                LogStatus("Canceled: " + upload.Guid, NotifyType.StatusMessage);
            }
            catch (Exception ex)
            {
                if (!IsExceptionHandled("Error", ex, upload))
                {
                    throw;
                }
            }
        }

        private bool IsExceptionHandled(string title, Exception ex, UploadOperation upload = null)
        {
            WebErrorStatus error = BackgroundTransferError.GetStatus(ex.HResult);
            if (error == WebErrorStatus.Unknown)
            {
                return false;
            }

            if (upload == null)
            {
                LogStatus(String.Format(CultureInfo.CurrentCulture, "Error: {0}: {1}", title, error),
                    NotifyType.ErrorMessage);
            }
            else
            {
                LogStatus(String.Format(CultureInfo.CurrentCulture, "Error: {0} - {1}: {2}", upload.Guid, title,
                    error), NotifyType.ErrorMessage);
            }

            return true;
        }

        // When operations happen on a background thread we have to marshal UI updates back to the UI thread.
        private void MarshalLog(string value)
        {
            var ignore = this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Log(value);
            });
        }

        private void Log(string message)
        {
            outputField.Text += message + "\r\n";
        }

        private void LogStatus(string message, NotifyType type)
        {
            NotifyUser(message, type);
            Log(message);
        }

    }
}
