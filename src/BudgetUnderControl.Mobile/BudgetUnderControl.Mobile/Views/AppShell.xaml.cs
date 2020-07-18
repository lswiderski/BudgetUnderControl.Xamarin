using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BudgetUnderControl;
using BudgetUnderControl.Mobile.Views;

namespace BudgetUnderControl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("login", typeof(Login));
            Routing.RegisterRoute("logout", typeof(Logout));
            Routing.RegisterRoute("exchangeRates", typeof(ExchangeRates));
            Routing.RegisterRoute("editTag", typeof(EditTag));
            Routing.RegisterRoute("accountDetails", typeof(AccountDetails));
        }
    }
}