using BudgetUnderControl.Mobile.ViewModels;
using System;
using Autofac;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BudgetUnderControl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FirstRunPage : ContentPage
    {
        ILoginViewModel vm;
        public FirstRunPage()
        {
            using (var scope = App.Container.BeginLifetimeScope())
            {
                this.BindingContext = vm = scope.Resolve<ILoginViewModel>();
            }
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        async void OnLoginButtonClickedAsync(object sender, EventArgs args)
        {
            await Shell.Current.GoToAsync("login");
        }

        async void OnCreateNewUserClickedAsync(object sender, EventArgs args)
        {
            await vm.CreateNewUserAsync();

        }
        async void OnSettingsClickedAsync(object sender, EventArgs args)
        {
            var settingsPage = new SettingsPage();
            await Navigation.PushModalAsync(settingsPage);

        }
    }
}