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
    public partial class AddAccount : ContentPage
    {
        IAddAccountViewModel vm;

        public AddAccount()
        {
            using (var scope = App.Container.BeginLifetimeScope())
            {
                this.BindingContext = vm = scope.Resolve<IAddAccountViewModel>();
            }
            InitializeComponent();
        }


        async void OnAddButtonClicked(object sender, EventArgs args)
        {
            await vm.AddAccount();
            await Navigation.PopAsync();
        }

        private void OnClearParentButtonClicked(object sender, EventArgs e)
        {
            vm.ClearParentAccountCombo();
        }
    }
}