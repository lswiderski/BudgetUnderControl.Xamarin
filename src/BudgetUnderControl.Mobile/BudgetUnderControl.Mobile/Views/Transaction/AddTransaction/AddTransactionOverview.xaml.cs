using Autofac;
using BudgetUnderControl.Mobile.Markers;
using BudgetUnderControl.Mobile.PlatformSpecific;
using BudgetUnderControl.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace BudgetUnderControl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddTransactionOverview : ContentPage
    {
        IAddTransactionViewModel vm;
        bool isLoaded = false;
        public AddTransactionOverview()
        {
            InitializeComponent();

            amount.Completed += (object sender, EventArgs e) => { name.Focus(); };
            name.Completed += (object sender, EventArgs e) => { type.Focus(); };
            type.Unfocused += (object sender, FocusEventArgs e) => { account.Focus(); };
            account.Unfocused += (object sender, FocusEventArgs e) => { categories.Focus(); };
        }

        public void SetContext(IAddTransactionViewModel model)
        {
            this.BindingContext = vm = model;
            amount.Focus();
        }

        async void OnAddButtonClicked(object sender, EventArgs args)
        {
            await vm.AddTransacionAsync();

            if (Navigation.ModalStack.Any())
            {
                await Navigation.PopModalAsync();
            }
            else
            {
                await Shell.Current.GoToAsync("Transactions");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(!isLoaded)
            {
                amount.Focus();
                isLoaded = true;

            }
            
        }
    }
}