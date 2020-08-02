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
    public partial class Charts : TabbedPage
    {
        IChartsViewModel vm;
        Filters filtersModal;
        public Charts()
        {
            using (var scope = App.Container.BeginLifetimeScope())
            {
                this.BindingContext = vm = scope.Resolve<IChartsViewModel>();
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
                await vm.LoadCategoryPieChartAsync();

                // remember to remove the event handler:
                App.Current.ModalPopping -= HandleModalPopping;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            pieChart.SetContext(vm);
        }
    }
}