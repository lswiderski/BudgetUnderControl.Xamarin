using BudgetUnderControl.MobileDomain;
using BudgetUnderControl.MobileDomain.Repositiories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Mobile.Repositories
{
    public class UserRepository : BaseModel, IUserRepository
    {
        public UserRepository(IContextFacade context) : base(context)
        {
        }

        public async Task<User> GetFirstUserAsync()
        {
            var user = await this.Context.Users.FirstOrDefaultAsync();

            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            user.UpdateModify();
            this.Context.Users.Update(user);
            await this.Context.SaveChangesAsync();
        }

        public async Task AddUserAsync(User user)
        {
            this.Context.Users.Add(user);
            await this.Context.SaveChangesAsync();
        }

        public async Task RemoveLocalUsersAsync()
        {
            var allUsers = await this.Context.Users.ToListAsync();
            this.Context.Users.RemoveRange(allUsers);

            await this.Context.SaveChangesAsync();
        }

        public async Task<bool> IsUserExistAsync()
        {
             return await this.GetFirstUserAsync() != null;
        }
    }
}
