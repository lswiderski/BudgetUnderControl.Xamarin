using Autofac;
using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Mobile.Keys;
using BudgetUnderControl.ViewModel;
using Microsoft.Extensions.Caching.Memory;
using MoreLinq;
using Syncfusion.SfChart.XForms;
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
    public partial class Charts : TabbedPage
    {
        IChartsViewModel vm;
        private readonly IMemoryCache memoryCache;
        Filters filtersModal;
        public Charts()
        {
            using (var scope = App.Container.BeginLifetimeScope())
            {
                this.BindingContext = vm = scope.Resolve<IChartsViewModel>();
                memoryCache = scope.Resolve<IMemoryCache>();
            }
            InitializeComponent();
        }


        protected async void OnFilter_Clicked(object sender, EventArgs e)
        {
            App.Current.ModalPopping += HandleModalPopping;
            filtersModal = new Filters(vm.Filter);
            await Navigation.PushModalAsync(filtersModal);
        }

        private async void HandleModalPopping(object sender, ModalPoppingEventArgs e)
        {
            if (e.Modal == filtersModal)
            {

                // now we can retrieve that phone number:
                vm.Filter = filtersModal.vm.Filter;
                filtersModal = null;
                if(this.CurrentPage.Title == "Expenses")
                {
                    await vm.LoadExpensesColumnChartAsync();
                }
                else if(this.CurrentPage.Title == "Categories")
                {
                    await vm.LoadCategoryPieChartAsync();
                }
                
                expensesChart.RebuildStackedChart();
                // remember to remove the event handler:
                App.Current.ModalPopping -= HandleModalPopping;
            }
        }

       

        protected override void OnAppearing()
        {
            base.OnAppearing();

            pieChart.SetContext(vm);
            expensesChart.SetContext(vm);
            TryGetFilter();
        }

        public void TryGetFilter()
        {
            TransactionsFilter _filter;
            if (memoryCache.TryGetValue(CacheKeys.Filters, out _filter))
            {
                vm.Filter = _filter;
            }
        }
    }
}