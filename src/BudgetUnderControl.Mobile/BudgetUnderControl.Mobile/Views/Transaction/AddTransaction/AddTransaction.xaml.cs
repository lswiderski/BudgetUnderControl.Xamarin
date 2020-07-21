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
    public partial class AddTransaction : TabbedPage
    {
        IAddTransactionViewModel vm;
        public AddTransaction()
        {
            using (var scope = App.Container.BeginLifetimeScope())
            {
                this.BindingContext = vm = scope.Resolve<IAddTransactionViewModel>();
            }
            InitializeComponent();
            

        }

        public AddTransaction(string amount, string title) : this()
        {
            vm.Amount = amount;
            vm.Name = title;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            addTransactionOverview.SetContext(vm);
            addTransactionExtra.SetContextAsync(vm);
            addTransactionFile.SetContext(vm);
        }
    }
}