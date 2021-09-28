using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.MobileDomain.Repositiories
{
    public interface IUserRepository
    {
        Task<User> GetFirstUserAsync();
        Task UpdateUserAsync(User user);
        Task AddUserAsync(User user);
    }
}
