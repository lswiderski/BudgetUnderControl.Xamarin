using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Autofac;
using BudgetUnderControl.Views;

using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using Microsoft.Extensions.Configuration;
using BudgetUnderControl.Mobile.IoC;
using BudgetUnderControl.MobileDomain.Repositiories;

[assembly: ExportFont("FontAwesome5Brands.otf", Alias = "FAB")]
[assembly: ExportFont("FontAwesome5Regular.otf", Alias = "FAR")]
[assembly: ExportFont("FontAwesome5Solid.otf", Alias = "FAS")]
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace BudgetUnderControl
{
    public class App : Application
    {
        public static IContainer Container;
        public IConfiguration Configuration { get; }

        public static AppShell AppShell
        {
            get;
            private set;
        }

        public App()
        {
            
        }

        protected override async void OnStart()
        {
            
            AutoFacInit();
            MainPage = AppShell = new AppShell();
            using (var scope = App.Container.BeginLifetimeScope())
            {
                var userRepository = scope.Resolve<IUserRepository>();

                var isUserExist = await userRepository.IsUserExistAsync();

                if (!isUserExist)
                {
                    await AppShell.OpenFirstRunAsync();
                }
            }

            if (Mobile.PlatformSpecific.Properties.REDIRECT_TO.HasValue )
            {
                switch (Mobile.PlatformSpecific.Properties.REDIRECT_TO.Value)
                {
                    case Common.Enums.ActivityPage.AddTransaction:
                        var value = Mobile.PlatformSpecific.Properties.ADD_TRANSACTION_VALUE;
                        var title = Mobile.PlatformSpecific.Properties.ADD_TRANSACTION_TITLE;
                        await AppShell.OpenAddTransactionAsync(value.ToString(), title);
                        break;
                    default:
                        break;
                }
            }
          
        }

        protected override void OnSleep()
        {
            
        }

        protected override void OnResume()
        {
           
        }

        protected void AutoFacInit()
        {
            // Initialize Autofac builder
            var builder = new ContainerBuilder();

            // Register services
            builder.RegisterModule<MobileModule>();

            App.Container = builder.Build();
            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(Container));

        }
    }
}

