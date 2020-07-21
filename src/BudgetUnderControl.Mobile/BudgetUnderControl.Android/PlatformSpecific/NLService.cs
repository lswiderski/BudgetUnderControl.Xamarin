using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Service.Notification;
using Android.Views;
using Android.Widget;
using BudgetUnderControl.Common;
using BudgetUnderControl.Common.Contracts.System;
using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.Mobile.PlatformSpecific;
using NLog;
using Plugin.LocalNotifications;
using Xamarin.Forms;

namespace BudgetUnderControl.Droid.PlatformSpecific
{
    [Service(Label = "Budget Under Control Notification Listener", Permission = "android.permission.BIND_NOTIFICATION_LISTENER_SERVICE")]
    [IntentFilter(new[] { "android.service.notification.NotificationListenerService" })]
    class NLService : NotificationListenerService
    {
        private static ILogger logger;
        private ILocalNotificationService notificationService;

        public override void OnCreate()
        {
            base.OnCreate();
            logger = DependencyService.Get<ILogManager>().GetLog();
            notificationService = DependencyService.Get<ILocalNotificationService>();
        }

        public override void OnNotificationPosted(StatusBarNotification sbn)
        {
            base.OnNotificationPosted(sbn);

            string pack = sbn.PackageName;
            string ticker = sbn.Notification.TickerText?.ToString();
            Bundle extras = sbn.Notification.Extras;
            string title = extras.GetString("android.title");
            string text = extras.GetCharSequence("android.text")?.ToString();
            
            if (pack.Equals("com.google.android.gms"))
            {
                logger.Info($"pack: {pack} | ticker: {ticker} | title: {title} | text: {text}");
                logger.Info(sbn.Notification.ToString());

                if (ticker.Equals("View your refund") || ticker.Equals("View your purchase"))
                {
                    var value = this.GetGooglePayValue(text);
                    var bundle = new List<BundleItem> {
                            new BundleItem { Key = PropertyKeys.ADD_TRANSACTION_TITLE, Type = BundleItemType.String, Object = title },            // ticker: View your refund  | text: PLN22.01 was refunded to Mastercard •••• 9901
                            new BundleItem { Key = PropertyKeys.REDIRECT_TO, Type = BundleItemType.Int, Object = ActivityPage.AddTransaction },   // ticker: View your purchase | title: jakdojade.pl | text: PLN3.40 with Mastercard •••• 9901
                            new BundleItem { Key = PropertyKeys.ADD_TRANSACTION_VALUE, Type = BundleItemType.String, Object = value },
                    };
                    notificationService.ShowNotification("New Google Pay Transaction", $"Add {value} from {title}", bundle);
                }
            }
            else if (pack.Equals("com.revolut.revolut"))
            {
                logger.Info($"pack: {pack} | ticker: {ticker} | title: {title} | text: {text}");
                logger.Info(sbn.Notification.ToString());

                var value = this.GetRevolutValue(text);
                var revolutTitle = this.GetRevolutTitle(text);
                var bundle = new List<BundleItem> {
                        new BundleItem { Key = PropertyKeys.ADD_TRANSACTION_TITLE, Type = BundleItemType.String, Object = revolutTitle },            //text: 💳 Paid €10 at The Circus Hostel
                        new BundleItem { Key = PropertyKeys.REDIRECT_TO, Type = BundleItemType.Int, Object = ActivityPage.AddTransaction },          //text: 💳 Paid €3.88 at McDonald's
                        new BundleItem { Key = PropertyKeys.ADD_TRANSACTION_VALUE, Type = BundleItemType.String, Object = value },                   //text: 💳 Paid €20.70 at Transit
                };
                notificationService.ShowNotification("New Revolut Transaction", $"Add {value} from {revolutTitle}", bundle);
            }
            else {
                logger.Info($"pack: {pack} | ticker: {ticker} | title: {title} | text: {text}");
            }
        }

        public override void OnNotificationRemoved(StatusBarNotification sbn)
        {
            base.OnNotificationRemoved(sbn);
        }

        string GetRevolutValue(string text)
        {
            var initialSplit = text.Split(new string[] { "at" }, StringSplitOptions.RemoveEmptyEntries);
            var value = initialSplit.FirstOrDefault().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault().Remove(0, 1);
            return value;
        }

        string GetRevolutTitle(string text)
        {
            var initialSplit = text.Split(new string[] { "at" }, StringSplitOptions.RemoveEmptyEntries);
            var title = initialSplit.LastOrDefault();
            return title;
        }

        string GetGooglePayValue(string text)
        {
            var initialSplit = text.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            var value = initialSplit.FirstOrDefault().Remove(0, 3);
            return value;
        }
    }
}