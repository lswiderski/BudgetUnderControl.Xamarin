using BudgetUnderControl.CommonInfrastructure.Commands;
using BudgetUnderControl.Mobile.Services;
using BudgetUnderControl.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BudgetUnderControl.Mobile.ViewModels
{
    public class LoginViewModel : ILoginViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string username;
        public string Username
        {
            get => username;
            set
            {
                if (username != value)
                {
                    username = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Username)));
                }
            }
        }

        private string password;
        public string Password
        {
            get => password;
            set
            {
                if (password != value)
                {
                    password = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
                }
            }
        }

        private bool clearLocalData;
        public bool ClearLocalData
        {
            get => clearLocalData;
            set
            {
                if (clearLocalData != value)
                {
                    clearLocalData = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ClearLocalData)));
                }
            }
        }

        private bool isError;
        public bool IsError
        {
            get => isError;
            set
            {
                if (isError != value)
                {
                    isError = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsError)));
                }
            }
        }

        private readonly ILoginMobileService loginMobileService;
        private readonly ICommandDispatcher commandDispatcher;

        public LoginViewModel(ILoginMobileService loginMobileService, ICommandDispatcher commandDispatcher)
        {
            this.commandDispatcher = commandDispatcher;
            this.loginMobileService = loginMobileService;
        }

        public async Task LoginAsync()
        {
            var result = await loginMobileService.LoginAsync(Username, Password, ClearLocalData);
            if(result)
            {
                await Shell.Current.GoToAsync("///Overview");
            }
            else
            {
                IsError = true;
            }
        }

        public async Task CreateNewUserAsync()
        {
            await loginMobileService.CreateNewUserAsync();
            await Shell.Current.GoToAsync("..");
        }

        public async Task LogoutAsync()
        {
            await loginMobileService.LogoutAsync();
        }
    }
}
