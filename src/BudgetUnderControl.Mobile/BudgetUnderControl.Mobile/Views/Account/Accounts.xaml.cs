using Autofac;
using BudgetUnderControl.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BudgetUnderControl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Accounts : ContentPage
    {
        IAccountsViewModel vm;
        public Accounts()
        {
            InitializeComponent();
            using (var scope = App.Container.BeginLifetimeScope())
            {
                this.BindingContext = vm = scope.Resolve<IAccountsViewModel>();
            }
        }

        protected async void OnAddButtonClicked(object sender, EventArgs args)
        {
            var addAccount = new AddAccount();
            await Navigation.PushModalAsync(addAccount);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await vm.LoadAccounts();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Guid accountId = vm.SelectedAccount.ExternalId;
            await Shell.Current.GoToAsync($"accountDetails?AccountId={accountId}");
        }

    }
}