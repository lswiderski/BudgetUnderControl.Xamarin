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
    public partial class PieChart : ContentPage
    {
        IChartsViewModel vm;
        public PieChart()
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

            await vm.LoadCategoryPieChartAsync();
        }

    }
}