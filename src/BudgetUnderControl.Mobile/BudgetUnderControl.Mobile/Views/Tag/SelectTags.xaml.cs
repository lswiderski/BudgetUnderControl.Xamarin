using Autofac;
using BudgetUnderControl.Mobile.Markers;
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
	public partial class SelectTags : ContentPage
	{
        ITagViewModel vm;
        ITagSelectablePage parentPage;
        public SelectTags(ITagSelectablePage parentPage)
        {
            InitializeComponent();
            using (var scope = App.Container.BeginLifetimeScope())
            {
                this.BindingContext = vm = scope.Resolve<ITagViewModel>();
            }

            this.parentPage = parentPage;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await vm.LoadActiveTagsAsync();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        async void OnItemSelectedAsync(object sender, SelectedItemChangedEventArgs e)
        {
            Guid tagId = vm.SelectedTag.ExternalId;
            parentPage.AddTagToList(tagId);
           await Navigation.PopAsync();
        }
    }
}