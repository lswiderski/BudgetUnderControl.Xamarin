using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Mobile.Services
{
    public interface ILoginMobileService
    {
        Task<bool> LoginAsync(string username, string password, bool clearLocalData);
        Task LogoutAsync();
    }
}
