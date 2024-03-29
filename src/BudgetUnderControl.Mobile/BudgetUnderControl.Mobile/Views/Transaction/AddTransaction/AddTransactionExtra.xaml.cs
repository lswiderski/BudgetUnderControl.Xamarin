﻿using Autofac;
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
    public partial class AddTransactionExtra : ContentPage, ITagSelectablePage
    {
        IAddTransactionViewModel vm;
        bool isLoaded = false;
        public AddTransactionExtra()
        {
            InitializeComponent();
        }

        public async void SetContextAsync(IAddTransactionViewModel model)
        {
            this.BindingContext = vm = model;
            await GetLocationAsync();
        }

        protected override async void OnAppearing()
        {
            if(!isLoaded)
            {
                await GetLocationAsync();
                isLoaded = true;
            }
            
        }

        async void OnSelectTagsButtonClicked(object sender, EventArgs args)
        {
            var selectTags = new SelectTags(this);
            await Navigation.PushModalAsync(selectTags);
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
            await GetLocationAsync();
        }

        async Task GetLocationAsync()
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
    }

}