using Autofac;
using BudgetUnderControl.ViewModel;
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
    public partial class DebugMenu : ContentPage
    {
        ISettingsViewModel vm;
        public DebugMenu()
        {
            using (var scope = App.Container.BeginLifetimeScope())
            {
                this.BindingContext = vm = scope.Resolve<ISettingsViewModel>();
            }

            InitializeComponent();
            apiUrl.Unfocused += (object sender, FocusEventArgs e) => { vm.OnApiUrlChange(); };
        }

        private async void ExportButton_Clicked(object sender, EventArgs e)
        {
            await vm.ExportBackupAsync();
        }

        private async void ImportButton_Clicked(object sender, EventArgs e)
        {
            vm.IsLoading = true;

            vm.ImportBackupAsync().Wait();
            vm.IsLoading = false;
        }

        private async void ExportCSVButton_Clicked(object sender, EventArgs e)
        {
            await vm.ExportCSVAsync();
        }

        private async void ExportSQLDBButton_Clicked(object sender, EventArgs e)
        {
            await vm.ExportDBAsync();
        }

        private async void SyncButton_Clicked(object sender, EventArgs e)
        {
            await vm.SyncAsync();
        }

        private async void ClearSyncDBButton_Clicked(object sender, EventArgs e)
        {
            await vm.ClearSyncDB();
        }

        private async void ClearLocalDataButton_Clicked(object sender, EventArgs e)
        {
            await vm.ClearLocalData();
        }

    }
}