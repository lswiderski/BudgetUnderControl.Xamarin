﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Autofac;
using BudgetUnderControl.Views;

using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using BudgetUnderControl.Infrastructure.IoC;
using Microsoft.Extensions.Configuration;
using BudgetUnderControl.Mobile.IoC;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace BudgetUnderControl
{
    public class App : Application
    {
        public static IContainer Container;
        public IConfiguration Configuration { get; }

        public static MasterPage MasterPage
        {
            get;
            private set;
        }

        public App()
        {
            
        }

        protected override void OnStart()
        {
            
            AutoFacInit();
            MainPage = MasterPage = new MasterPage();

            if(Mobile.PlatformSpecific.Properties.REDIRECT_TO.HasValue )
            {
                switch (Mobile.PlatformSpecific.Properties.REDIRECT_TO.Value)
                {
                    case Common.Enums.ActivityPage.AddTransaction:
                        var value = Mobile.PlatformSpecific.Properties.ADD_TRANSACTION_VALUE;
                        var title = Mobile.PlatformSpecific.Properties.ADD_TRANSACTION_TITLE;
                        MasterPage.NavigateTo(typeof(AddTransaction), value.ToString(), title);
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
            builder.RegisterModule<InfrastructureModule>();
            builder.RegisterModule<MobileModule>();

            App.Container = builder.Build();
            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(Container));

        }
    }
}
