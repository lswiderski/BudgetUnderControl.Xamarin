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
	public partial class Tags : ContentPage
	{
        ITagViewModel vm;
        public Tags()
        {
            InitializeComponent();
            using (var scope = App.Container.BeginLifetimeScope())
            {
                this.BindingContext = vm = scope.Resolve<ITagViewModel>();
            }
        }

        protected async void OnAddButtonClicked(object sender, EventArgs args)
        {
            var addTag = new AddTag();
            await Navigation.PushModalAsync(addTag);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await vm.LoadTagsAsync();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Guid tagId = vm.SelectedTag.ExternalId;
            await Shell.Current.GoToAsync($"editTag?TagId={tagId}");
        }

    }
}