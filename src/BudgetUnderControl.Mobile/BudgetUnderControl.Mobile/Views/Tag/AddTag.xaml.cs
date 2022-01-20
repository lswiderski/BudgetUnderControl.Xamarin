using Autofac;
using BudgetUnderControl.Mobile.ViewModels;
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
	public partial class AddTag : ContentPage
	{
        ITagViewModel vm;
        public AddTag()
        {
            InitializeComponent();
            using (var scope = App.Container.BeginLifetimeScope())
            {
                this.BindingContext = vm = scope.Resolve<ITagViewModel>();
            }
        }

        protected async void OnAddButtonClicked(object sender, EventArgs args)
        {
            await vm.AddTagAsync();
            await Navigation.PopAsync();
        }

    }
}