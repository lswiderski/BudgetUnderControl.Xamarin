using Autofac;
using BudgetUnderControl.Mobile.ViewModels;
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
	public partial class Logout : ContentPage
	{
        ILoginViewModel vm;
        public Logout()
        {
            using (var scope = App.Container.BeginLifetimeScope())
            {
                this.BindingContext = vm = scope.Resolve<ILoginViewModel>();
            }
            InitializeComponent();
        }

        async void OnLogoutButtonClickedAsync(object sender, EventArgs args)
        {
            await vm.LogoutAsync();
            await Shell.Current.GoToAsync("..");

        }
    }
}