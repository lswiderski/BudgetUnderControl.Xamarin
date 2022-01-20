using Autofac;
using BudgetUnderControl.Mobile.ViewModels;
using BudgetUnderControl.ViewModel;
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
	public partial class ExchangeRates : ContentPage
	{
        ICurrencyViewModel vm;
        public ExchangeRates ()
		{
            using (var scope = App.Container.BeginLifetimeScope())
            {
                this.BindingContext = vm = scope.Resolve<ICurrencyViewModel>();
            }
            InitializeComponent ();
		}

        private async void OnAddExchangeRateButtonClicked(object sender, EventArgs e)
        {
            var addExchangeRate = new AddExchangeRate();
            await Navigation.PushAsync(addExchangeRate);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await vm.LoadExchangeRatesAsync();
        }
    }
}