using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.ViewModel
{
    public interface ISettingsViewModel
    {
        Task ExportBackupAsync();

        Task ImportBackupAsync();

        Task ExportCSVAsync();

        Task ExportDBAsync();

        Task SyncAsync();

        Task ClearLocalData();

        Task ClearSyncDB();

        string ApiUrl { get; set; }

        void OnApiUrlChange();

        bool IsLogged { get; set; }

        bool IsLoading { get; set; }

        bool IsNotLogged { get; set; }

        bool IsDarkModeOn { get; set; }

        void RefreshUserButtons();
    }
}
