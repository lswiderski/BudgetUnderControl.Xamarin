using Autofac;
using BudgetUnderControl.CommonInfrastructure.Commands;
using BudgetUnderControl.Mobile.Services;
using BudgetUnderControl.Mobile.Keys;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using BudgetUnderControl.CommonInfrastructure.Settings;

namespace BudgetUnderControl.ViewModel
{
    public class SettingsViewModel : ISettingsViewModel, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private bool isLogged;
        public bool IsLogged
        {
            get => isLogged;
            set
            {
                    isLogged = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLogged)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsNotLogged)));
            }
        }

        public void RefreshUserButtons()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLogged)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsNotLogged)));
        }

        public bool IsNotLogged
        {
            get => !IsLogged;
            set
            {
                if (IsLogged != value)
                {
                    IsLogged = !value;
                }
            }
        }

        private string apiUrl;
        public string ApiUrl
        {
            get => apiUrl;
            set
            {
                if (apiUrl != value)
                {
                    apiUrl = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ApiUrl)));
                }
            }
        }

        private bool isLoading;
        public bool IsLoading
        {
            get => isLoading;
            set
            {
                if (isLoading != value)
                {
                    isLoading = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLoading)));
                }
            }
        }


        private bool isDarkModeOn;
        public bool IsDarkModeOn
        {
            get => isDarkModeOn;
            set
            {
                if (isDarkModeOn != value)
                {
                    isDarkModeOn = value;
                    Preferences.Set(PreferencesKeys.IsDarkModeOn, value);
                    ((Xamarin.Forms.Application.Current) as App).ThemeChanger.ApplyTheme(value);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsDarkModeOn)));
                }
            }
        }

        private bool isLocationEnabled;
        public bool IsLocationEnabled
        {
            get => isLocationEnabled;
            set
            {
                if (isLocationEnabled != value)
                {
                    isLocationEnabled = value;
                    Preferences.Set(PreferencesKeys.IsLocationEnabled, value);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLocationEnabled)));
                }
            }
        }

        ISyncMobileService syncMobileService;

        public SettingsViewModel(ISyncMobileService syncMobileService, GeneralSettings settings)
        {
            this.syncMobileService = syncMobileService;
            var url = Preferences.Get(PreferencesKeys.APIURL, string.Empty);
            apiUrl = string.IsNullOrEmpty(url) || string.IsNullOrWhiteSpace(url) ? settings.ApiBaseUri : url;
            isDarkModeOn = Preferences.Get(PreferencesKeys.IsDarkModeOn, false);
            isLocationEnabled = Preferences.Get(PreferencesKeys.IsLocationEnabled, false);
        }

        public async Task ExportBackupAsync()
        {
            IsLoading = true;
            await syncMobileService.SaveBackupFileAsync();
            IsLoading = false;
        }

        public async Task ImportBackupAsync()
        {
            await syncMobileService.LoadBackupFileAsync();
        }


        public async Task ExportCSVAsync()
        {
            IsLoading = true;
            await syncMobileService.ExportCSVAsync();
            IsLoading = false;
        }

        public async Task ExportDBAsync()
        {
            IsLoading = true;
            await syncMobileService.ExportDBAsync();
            IsLoading = false;
        }

        public async Task ClearSyncDB()
        {
            IsLoading = true;
            await syncMobileService.TaskClearSyncDB();
            IsLoading = false;
        }

        public async Task ClearLocalData()
        {
            IsLoading = true;
            await syncMobileService.CleanDataBaseAsync();
            IsLoading = false;
        }

        public async Task SyncAsync()
        {
            IsLoading = true;
            await syncMobileService.SyncAsync();
            IsLoading = false;
        }

        public void OnApiUrlChange()
        {
            Preferences.Set(PreferencesKeys.APIURL, ApiUrl);
        }
    }
}
