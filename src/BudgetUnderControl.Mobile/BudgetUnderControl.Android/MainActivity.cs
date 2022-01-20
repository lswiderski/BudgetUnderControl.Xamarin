using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using BudgetUnderControl.Views;
using System.Diagnostics;
using System.IO;
using Xamarin.Forms;
using BudgetUnderControl.Common;
using NLog;
using BudgetUnderControl.Mobile.PlatformSpecific;
using BudgetUnderControl.Common.Enums;
using Android.Content;
using BudgetUnderControl.Mobile.CommonDTOs;
using BudgetUnderControl.Mobile;
using Xamarin.Essentials;
using BudgetUnderControl.Mobile.Keys;
using System.Collections.Generic;

namespace BudgetUnderControl.Droid
{
    [Activity(Label = "Budget Under Control", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IThemeChanger
    {
        private static ILogger logger;
        internal static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle bundle)
        {
            Instance = this;
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
           
            base.OnCreate(bundle);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            Android.Gms.Ads.MobileAds.Initialize(this, "ca-app-pub-1938975042085430~4938848442");
            global::Xamarin.Forms.Forms.SetFlags("Expander_Experimental");
            global::Xamarin.Forms.Forms.Init(this, bundle);
            global::Xamarin.Forms.FormsMaterial.Init(this, bundle);
            Xamarin.Essentials.Platform.Init(this, bundle);
            DisplayCrashReport();
            
            var app = new App(this);
            logger = DependencyService.Get<ILogManager>().GetLog();
            ApplyTheme(Preferences.Get(PreferencesKeys.IsDarkModeOn, false), false);
            LoadApplication(app);
            CheckIfComeFromNotification();
        }

        public static readonly int PickImageId = 1000;
        public TaskCompletionSource<ImagePickerResultDTO> PickImageTaskCompletionSource { get; set; }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if(requestCode == PickImageId)
            {
                if((resultCode == Result.Ok) && (data != null))
                {
                    Android.Net.Uri uri = data.Data;
                    Stream stream = ContentResolver.OpenInputStream(uri);
                    var result = new ImagePickerResultDTO
                    {
                        Stream = stream,
                        Path = uri.Path,
                    };
                    PickImageTaskCompletionSource.SetResult(result);
                }
                else
                {
                    PickImageTaskCompletionSource.SetResult(null);
                }
            }

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void CheckIfComeFromNotification()
        {
            var extras = base.Intent.Extras;
            if (extras != null)
            {
                var redirectTo = extras.GetInt(PropertyKeys.REDIRECT_TO);
                var add_value = extras.GetString(PropertyKeys.ADD_TRANSACTION_VALUE);
                var add_title = extras.GetString(PropertyKeys.ADD_TRANSACTION_TITLE);

                if (!string.IsNullOrEmpty(add_value))
                {
                    Properties.ADD_TRANSACTION_VALUE = add_value;
                }
                if (!string.IsNullOrEmpty(add_title))
                {
                    Properties.ADD_TRANSACTION_TITLE = add_title;
                }
                if (redirectTo != 0)
                {
                    Properties.REDIRECT_TO = (ActivityPage)redirectTo;
                }
            }
        }

        private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            var newExc = new Exception("TaskSchedulerOnUnobservedTaskException", unobservedTaskExceptionEventArgs.Exception);
            LogUnhandledException(newExc);
        }
        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var newExc = new Exception("CurrentDomainOnUnhandledException", unhandledExceptionEventArgs.ExceptionObject as Exception);
            LogUnhandledException(newExc);
        }

        internal static void LogUnhandledException(Exception exception)
        {
            try
            {
                logger.Error(exception);

                const string errorFileName = "Fatal.log";
                var libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                var errorFilePath = Path.Combine(libraryPath, errorFileName);
                var errorMessage = String.Format("Time: {0}\r\nError: Unhandled Exception\r\n{1}",
                DateTime.Now, exception.ToString());
                File.WriteAllText(errorFilePath, errorMessage);

                // Log to Android Device Logging.
                Android.Util.Log.Error("Crash Report", errorMessage);
            }
            catch
            {
                // just suppress any error logging exceptions
            }
        }

        /// <summary>
        // If there is an unhandled exception, the exception information is diplayed 
        // on screen the next time the app is started (only in debug configuration)
        /// </summary>
        [Conditional("DEBUG")]
        private void DisplayCrashReport()
        {
            const string errorFilename = "Fatal.log";
            var libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var errorFilePath = Path.Combine(libraryPath, errorFilename);

            if (!File.Exists(errorFilePath))
            {
                return;
            }

            var errorText = File.ReadAllText(errorFilePath);
            new AlertDialog.Builder(this)
                .SetPositiveButton("Clear", (sender, args) =>
                {
                    File.Delete(errorFilePath);
                })
                .SetNegativeButton("Close", (sender, args) =>
                {
            // User pressed Close.
        })
                .SetMessage(errorText)
                .SetTitle("Crash Report")
                .Show();
        }

        public void ApplyTheme(bool darkmodeOn, bool refresh)
        {
           
            if (darkmodeOn)
            {
                Theme.ApplyStyle(Resource.Style.MainThemeDark, refresh);
                App.Current.UserAppTheme = OSAppTheme.Dark;
            }
            else
            {
                Theme.ApplyStyle(Resource.Style.MainTheme, refresh);
                App.Current.UserAppTheme = OSAppTheme.Light;
            }
        }
    }
}

