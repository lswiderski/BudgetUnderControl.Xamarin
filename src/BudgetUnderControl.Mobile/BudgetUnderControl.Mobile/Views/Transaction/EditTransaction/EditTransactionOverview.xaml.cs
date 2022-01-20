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
    public partial class EditTransactionOverview : ContentPage
    {
        IEditTransactionViewModel vm;

        public EditTransactionOverview()
        {
           
            InitializeComponent();
        }

        public void SetContext(IEditTransactionViewModel model)
        {
            this.BindingContext = vm = model;
        }

        async void OnEditButtonClicked(object sender, EventArgs args)
        {
            await vm.EditTransactionAsync();
            await Navigation.PopAsync();
        }

        async void OnDeleteButtonClicked(object sender, EventArgs args)
        {
            var remove = await this.DisplayAlert("Remove", "Do you want to remove this Transaction?", "Yes", "No");
            if (!remove) return;
            await vm.DeleteTransactionAsync();
            await Navigation.PopAsync();
        }

       
    }
}