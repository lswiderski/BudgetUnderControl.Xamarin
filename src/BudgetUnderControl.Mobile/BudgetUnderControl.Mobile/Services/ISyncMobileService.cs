using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Mobile.Services
{
    public interface ISyncMobileService
    {
        Task<string> GetBackUpJSONAsync();
        Task SaveBackupFileAsync();
        Task LoadBackupFileAsync();
        Task ExportCSVAsync();
        Task ExportDBAsync();
        Task SyncAsync();
        Task TaskClearSyncDB();
        Task CleanDataBaseAsync();
        Task CreateNewUserAsync();
    }
}
