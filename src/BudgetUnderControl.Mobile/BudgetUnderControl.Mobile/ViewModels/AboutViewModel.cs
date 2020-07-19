using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BudgetUnderControl.Mobile.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        
        public ICommand OpenWebCommand { get; }

        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://github.com/lswiderski/budget-under-control"));
        }
    }

}
