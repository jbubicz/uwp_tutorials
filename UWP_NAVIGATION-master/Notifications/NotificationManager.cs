using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Microsoft.Toolkit.Uwp.Notifications;

namespace Notifications
{
    public static class NotificationManager
    {
        public static async Task<bool> YesNoAsyncDialog(string message, string title = null)
        {
            var dialog = new MessageDialog(message, title??"");
            dialog.Commands.Add(new UICommand("Yes") { Id = 0 });
            dialog.Commands.Add(new UICommand("No") { Id = 1 });
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;
            var dialogResult = await dialog.ShowAsync();
            if ((int)dialogResult.Id == 0)
                return true;
            else
                return false;
        }
        public static async Task Notification(string message, string title = null)
        {
            if (message != null)
            {
                var dialog = new MessageDialog(message, title??"");
                dialog.Commands.Add(new UICommand("Ok"));
                await dialog.ShowAsync();
            }
        }

        #region Notifications on tiles
        private static TileContent CreateTitleContent(string title, string captionSubtle1, string captionSubtle2)
        {
            TileContent content = new TileContent()
            {
                Visual = new TileVisual()
                {
                    TileWide = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            Children =
                            {
                                new AdaptiveText()
                                {
                                    Text = title,
                                    HintStyle = AdaptiveTextStyle.Subtitle
                                },

                                new AdaptiveText()
                                {
                                    Text = captionSubtle1,
                                    HintStyle = AdaptiveTextStyle.CaptionSubtle
                                },

                                new AdaptiveText()
                                {
                                    Text = captionSubtle2,
                                    HintStyle = AdaptiveTextStyle.CaptionSubtle
                                }
                            }
                        }
                    }
                }
            };

            //TileNotification n = new TileNotification(content.GetXml());

            //var a = n.Content.GetXml();

            return content;
        }
        private static void Algorithm(TileUpdater tileUpdater, TileContent content)
        {
            var notification = new TileNotification(content.GetXml());
            tileUpdater.Update(notification);
        }
        private static void AddNotifications()
        {
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);
            updater.Clear();

            Algorithm(updater, CreateTitleContent("Title1", "SubCaption", "Desctription..."));
            Algorithm(updater, CreateTitleContent("Title2", "SubCaption", "Desctription..."));
            Algorithm(updater, CreateTitleContent("Title3", "SubCaption", "Desctription..."));
            Algorithm(updater, CreateTitleContent("Title4", "SubCaption", "Desctription..."));
            Algorithm(updater, CreateTitleContent("Title5", "SubCaption", "Desctription..."));
        }
        #endregion
    }
}
