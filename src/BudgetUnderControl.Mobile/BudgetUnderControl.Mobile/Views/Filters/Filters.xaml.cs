using Autofac;
using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.ViewModel;
using Syncfusion.XForms.ComboBox;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BudgetUnderControl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Filters : ContentPage
    {
        public IFiltersViewModel vm;
        public Filters(TransactionsFilter filter)
        {
            using (var scope = App.Container.BeginLifetimeScope())
            {
                this.BindingContext = vm = scope.Resolve<IFiltersViewModel>();
            }
            this.vm.SetFilter(filter);
            InitializeComponent();
        }

        protected async void OnSearchButtonClicked(object sender, EventArgs args)
        {
            vm.BuildFilter();
            await Navigation.PopModalAsync();
        }

        protected async void OnResetButtonClicked(object sender, EventArgs args)
        {
            vm.ResetFilter();
            await Navigation.PopModalAsync();
        }

        protected async void Tags_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            ObservableCollection<object> SelectedItems = new ObservableCollection<object>();
            SelectedItems = (sender as SfComboBox).SelectedItem as ObservableCollection<object>;
            vm.SelectedTags = new ObservableCollection<TagDTO>(SelectedItems.Select(x => (TagDTO)Convert.ChangeType(x, typeof(TagDTO))));
        }

        protected async void Category_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            ObservableCollection<object> SelectedItems = new ObservableCollection<object>();
            SelectedItems = (sender as SfComboBox).SelectedItem as ObservableCollection<object>;
            vm.SelectedCategories = new ObservableCollection<CategoryListItemDTO>(SelectedItems.Select(x => (CategoryListItemDTO)Convert.ChangeType(x, typeof(CategoryListItemDTO))));
        }

        protected async void Account_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            ObservableCollection<object> SelectedItems = new ObservableCollection<object>();
            SelectedItems = (sender as SfComboBox).SelectedItem as ObservableCollection<object>;
            vm.SelectedAccounts = new ObservableCollection<AccountListItemDTO>(SelectedItems.Select(x => (AccountListItemDTO)Convert.ChangeType(x, typeof(AccountListItemDTO))));
        }
    }
}