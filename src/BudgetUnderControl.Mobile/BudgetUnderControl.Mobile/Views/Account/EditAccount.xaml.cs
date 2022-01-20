using Autofac;
using BudgetUnderControl.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BudgetUnderControl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditAccount : ContentPage
    {
        IEditAccountViewModel vm;

        Guid accountId;
        public EditAccount(Guid accountId)
        {
            using (var scope = App.Container.BeginLifetimeScope())
            {
                this.BindingContext = vm = scope.Resolve<IEditAccountViewModel>();
            }
            this.accountId = accountId;
            InitializeComponent();
        }

        async void OnEditButtonClicked(object sender, EventArgs args)
        {
            await vm.SaveAccount();
            await Navigation.PopAsync(true);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.LoadAccount(accountId);
        }

        private void OnClearParentButtonClicked(object sender, EventArgs e)
        {
            vm.ClearParentAccountCombo();
        }
    }
}