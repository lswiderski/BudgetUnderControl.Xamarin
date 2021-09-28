using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BudgetUnderControl;
using BudgetUnderControl.Mobile.Views;
using Autofac;
using BudgetUnderControl.CommonInfrastructure.Settings;

namespace BudgetUnderControl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            using (var scope = App.Container.BeginLifetimeScope())
            {
                var settings = scope.Resolve<GeneralSettings>();
                Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(settings.SyncfusionLicenseKey);
            }

            InitializeComponent();
          
            Routing.RegisterRoute("login", typeof(Login));
            Routing.RegisterRoute("logout", typeof(Logout));
            Routing.RegisterRoute("exchangeRates", typeof(ExchangeRates));
            Routing.RegisterRoute("editTag", typeof(EditTag));
            Routing.RegisterRoute("accountDetails", typeof(AccountDetails));
        }

        public async Task OpenAddTransactionAsync(string value, string title)
        {
            var addTransaction = new AddTransaction(value.ToString(), title);
            await Navigation.PushModalAsync(addTransaction);
        }

        public async Task OpenFirstRunAsync()
        {
            var firstRunPage = new FirstRunPage();
            await Navigation.PushModalAsync(firstRunPage);
        }
    }
}