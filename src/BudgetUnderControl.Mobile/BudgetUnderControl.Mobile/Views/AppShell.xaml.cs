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
using System.ComponentModel;
using Xamarin.Essentials;
using BudgetUnderControl.Mobile.Keys;

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
            PropertyChanged += Shell_PropertyChanged;

            Routing.RegisterRoute("login", typeof(Login));
            Routing.RegisterRoute("logout", typeof(Logout));
            Routing.RegisterRoute("exchangeRates", typeof(ExchangeRates));
            Routing.RegisterRoute("editTag", typeof(EditTag));
            Routing.RegisterRoute("accountDetails", typeof(AccountDetails));
            Routing.RegisterRoute("debug", typeof(DebugMenu));
            Routing.RegisterRoute("about", typeof(AboutPage));
            Routing.RegisterRoute("FirstRunPage", typeof(FirstRunPage));
        }

        public async Task OpenAddTransactionAsync(string value, string title)
        {
            var addTransaction = new AddTransaction(value.ToString(), title);
            await Navigation.PushModalAsync(addTransaction);
        }

        public async Task OpenFirstRunAsync()
        {
            await CheckUserExistanceAsync();
        }

        private void Shell_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("FlyoutIsPresented"))
                if (FlyoutIsPresented)
                    Task.WhenAll(OnFlyoutOpenedAsync()); 
                else
                    Task.WhenAll(OnFlyoutClosedAsync());
        }

        private async Task OnFlyoutOpenedAsync()
        {
            await this.CheckUserExistanceAsync();
        }

        private async Task OnFlyoutClosedAsync()
        {
            await this.CheckUserExistanceAsync();
        }

        private async Task CheckUserExistanceAsync()
        {
            if(await SecureStorage.GetAsync(SecurityStorageKeys.UserExternalId) == null)
            {
                var currentPage = Shell.Current.CurrentPage;
                if (currentPage.GetType() != typeof(FirstRunPage))
                {
                    await Shell.Current.GoToAsync("FirstRunPage");
                }
               
            }

        }
    }
}