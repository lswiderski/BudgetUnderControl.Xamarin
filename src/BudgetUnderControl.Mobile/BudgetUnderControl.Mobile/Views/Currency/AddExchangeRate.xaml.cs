using Autofac;
using BudgetUnderControl.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BudgetUnderControl.Mobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddExchangeRate : ContentPage
	{
        ICurrencyViewModel vm;
        public AddExchangeRate()
        {
            using (var scope = App.Container.BeginLifetimeScope())
            {
                this.BindingContext = vm = scope.Resolve<ICurrencyViewModel>();
            }
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await vm.LoadCurrenciesAsync();
        }

        private async void OnAddExchangeRateButtonClicked(object sender, EventArgs e)
        {
            await vm.AddExchangeRateAsync();
            await Navigation.PopAsync();
        }

        private async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

    }
}