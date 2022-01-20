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
    public partial class EditTransaction : TabbedPage, ITagSelectablePage
    {
        public IEditTransactionViewModel vm;
        Guid transactionId;
        bool isLoaded = false;
        public EditTransaction(Guid transactionId)
        {
            using (var scope = App.Container.BeginLifetimeScope())
            {
                this.BindingContext = vm = scope.Resolve<IEditTransactionViewModel>();
            }
            this.transactionId = transactionId;
            InitializeComponent();
        }

        async void OnEditButtonClicked(object sender, EventArgs args)
        {
            await vm.EditTransactionAsync();
            await Navigation.PopAsync();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (!isLoaded)
            {
                await vm.GetTransactionAsync(transactionId);
                isLoaded = true;
                editTransactionOverview.SetContext(vm);
                editTransactionExtra.SetContext(vm);
                editTransactionFile.SetContext(vm);
            }
        }

        async void OnDeleteButtonClicked(object sender, EventArgs args)
        {
            var remove = await this.DisplayAlert("Remove", "Do you want to remove this Transaction?", "Yes", "No");
            if (!remove) return;
            await vm.DeleteTransactionAsync();
            await Navigation.PopAsync();
        }

        async void OnSelectTagsButtonClicked(object sender, EventArgs args)
        {
            var selectTags = new SelectTags(this);
            await Navigation.PushAsync(selectTags);
        }

        async void OnDeleteTagButtonClicked(object sender, SelectedItemChangedEventArgs e)
        {
            Guid tagId = vm.SelectedTag.ExternalId;
            await vm.RemoveTagFromListAsync(tagId);
        }

        public async void AddTagToList(Guid tagId)
        {
            await vm.AddTagAsync(tagId);
        }

        async void OnGetLocationButtonClicked(object sender, System.EventArgs e)
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    vm.Latitude = location.Latitude;
                    vm.Longitude = location.Longitude;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Faild", fnsEx.Message, "OK");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Faild", pEx.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Faild", ex.Message, "OK");
            }

            GetRealLocationAsync();
        }

        async void GetRealLocationAsync()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    vm.Latitude = location.Latitude;
                    vm.Longitude = location.Longitude;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Faild", fnsEx.Message, "OK");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                await DisplayAlert("Faild", fneEx.Message, "OK");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Faild", pEx.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Faild", ex.Message, "OK");
            }
        }

        async void OnAddImageButtonClicked(object sender, EventArgs e)
        {
            (sender as Button).IsEnabled = false;

            var pickerResult = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
            Stream stream = pickerResult?.Stream;
            if (stream != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    vm.ImageByteArray = ms.ToArray();
                }

                Stream stream2 = new MemoryStream(vm.ImageByteArray);
                vm.HasImage = true;
                vm.HasNoImage = false;
                stream2.Position = 0;
                vm.ImageSource = ImageSource.FromStream(() => stream2);
            }
             (sender as Button).IsEnabled = true;
        }

        async void OnDeleteImageButtonClicked(object sender, EventArgs e)
        {
            (sender as Button).IsEnabled = false;

            vm.ImageSource = null;
            vm.ImageByteArray = null;
            vm.HasImage = false;
            vm.HasNoImage = true;
            (sender as Button).IsEnabled = true;
        }

    }
}