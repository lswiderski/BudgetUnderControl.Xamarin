using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Mobile.Services
{
    public interface ISeedService
    {
        Task CreateNewUserAsync();
    }
}
