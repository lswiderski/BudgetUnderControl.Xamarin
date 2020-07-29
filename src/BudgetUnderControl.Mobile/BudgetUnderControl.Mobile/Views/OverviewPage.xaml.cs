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

            var balance = await vm.GetCurrentBalanceAsync();

            var totalBalance = balance.Where(x => x.IsExchanged).FirstOrDefault();
            mainFrame.Children.Add(new Label { Text = $"Total: {totalBalance.Value}" });
            mainFrame.Children.Add(new Label { Text = string.Empty });
            List<string> curenciesToShow = new List<string> { "PLN", "EUR", "USD" };
            foreach (var item in balance.Where(x => !x.IsExchanged && x.Value != 0 && curenciesToShow.Contains(x.Currency)).OrderBy(x => x.Currency))
            {
                mainFrame.Children.Add(new Label { Text = $"{item.Currency}: {item.Value}" });
            }
           
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