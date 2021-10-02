using Autofac;
using BudgetUnderControl.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using BudgetUnderControl.Mobile.Keys;

namespace BudgetUnderControl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OverviewPage : ContentPage
    {
        private FloatingActionButtonView fab;
        IOverviewViewModel vm;
        public OverviewPage()
        {
            using (var scope = App.Container.BeginLifetimeScope())
            {
                this.BindingContext = vm = scope.Resolve<IOverviewViewModel>();
            }

            InitializeComponent();
            InitFAB();

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            mainFrame.Children.Clear();



            var lines = new Dictionary<string, string>();

            var total = await SecureStorage.GetAsync(SecurityStorageKeys.BalanceTotal);
            if (total == null)
            {
                var balance = await vm.GetCurrentBalanceAsync();
                var totalBalance = balance.Where(x => x.IsExchanged).FirstOrDefault();
                lines.Add("Total", $"Total: {totalBalance.Value}");

                lines.Add("PLN", balance.Where(x => !x.IsExchanged && x.Value != 0 && x.Currency == "PLN").OrderBy(x => x.Currency).Select(item => $"{item.Currency}: {item.Value}").FirstOrDefault());
                lines.Add("EUR", balance.Where(x => !x.IsExchanged && x.Value != 0 && x.Currency == "EUR").OrderBy(x => x.Currency).Select(item => $"{item.Currency}: {item.Value}").FirstOrDefault());
                lines.Add("USD", balance.Where(x => !x.IsExchanged && x.Value != 0 && x.Currency == "USD").OrderBy(x => x.Currency).Select(item => $"{item.Currency}: {item.Value}").FirstOrDefault());

                await SecureStorage.SetAsync(SecurityStorageKeys.BalanceTotal, !string.IsNullOrEmpty(lines["Total"]) ? lines["Total"] : string.Empty);
                await SecureStorage.SetAsync("Balance_PLN", !string.IsNullOrEmpty(lines["PLN"]) ? lines["PLN"] : string.Empty);
                await SecureStorage.SetAsync("Balance_EUR", !string.IsNullOrEmpty(lines["EUR"]) ? lines["EUR"] : string.Empty);
                await SecureStorage.SetAsync("Balance_USD", !string.IsNullOrEmpty(lines["USD"]) ? lines["USD"] : string.Empty);
            }
            else
            {
                lines.Add("Total", total);
                lines.Add("PLN", await SecureStorage.GetAsync("Balance_PLN"));
                lines.Add("EUR", await SecureStorage.GetAsync("Balance_EUR"));
                lines.Add("USD", await SecureStorage.GetAsync("Balance_USD"));
            }

            DisplayBalance(lines);

        }

        private void DisplayBalance(Dictionary<string, string> lines)
        {
            mainFrame.Children.Add(new Label { Text = lines["Total"] });
            mainFrame.Children.Add(new Label { Text = string.Empty });
            mainFrame.Children.Add(new Label { Text = lines["PLN"] });
            mainFrame.Children.Add(new Label { Text = lines["EUR"] });
            mainFrame.Children.Add(new Label { Text = lines["USD"] });
        }


        private void InitFAB()
        {
            fab = new FloatingActionButtonView()
            {
                ImageName = "ic_add.png",
                ColorNormal = Color.FromHex("ff3498db"),
                ColorPressed = Color.Black,
                ColorRipple = Color.FromHex("ff3498db"),
                Clicked = OnAddButtonClicked
            };

            // Overlay the FAB in the bottom-right corner
            AbsoluteLayout.SetLayoutFlags(fab, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(fab, new Rectangle(1f, 0.9f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            absoluteLayout.Children.Add(fab);
        }

        protected async void OnAddButtonClicked(object sender, EventArgs args)
        {
            var addAccount = new AddTransaction();
            await Navigation.PushModalAsync(addAccount);
        }

    }
}