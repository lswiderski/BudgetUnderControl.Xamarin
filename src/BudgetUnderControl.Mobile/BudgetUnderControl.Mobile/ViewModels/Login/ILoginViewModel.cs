using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Mobile.ViewModels
{
    public interface ILoginViewModel
    {
        string Username { get; set; }

        string Password { get; set; }

        bool ClearLocalData { get; set; }

        bool IsError { get; set; }

        Task LoginAsync();
        Task LogoutAsync();
    }
}
