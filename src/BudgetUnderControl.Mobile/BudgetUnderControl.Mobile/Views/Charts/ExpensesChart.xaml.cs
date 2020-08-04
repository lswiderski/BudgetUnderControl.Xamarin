using BudgetUnderControl.ViewModel;
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
    public partial class ExpensesChart : ContentPage
    {

        IChartsViewModel vm;
        public ExpensesChart()
        {
            InitializeComponent();
        }

        public void SetContext(IChartsViewModel model)
        {
            this.BindingContext = vm = model;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await vm.LoadExpensesColumnChartAsync();
            this.RebuildStackedChart();
        }

        public void RebuildStackedChart()
        {
            expensesStackedChart.Series.Clear();

            vm.ExpensesColumnChartData.ForEach(x =>
            {
                var serie = new StackingColumnSeries()
                {
                    ItemsSource = x.Data,
                    Label = x.Name,
                    XBindingPath = "XAxis",
                    YBindingPath = "YAxis",
                    EnableTooltip = true,
                };
                expensesStackedChart.Series.Add(serie);
            });
        }
    }
}