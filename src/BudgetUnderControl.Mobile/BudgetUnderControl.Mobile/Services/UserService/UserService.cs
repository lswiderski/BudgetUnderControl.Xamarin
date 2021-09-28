using BudgetUnderControl.CommonInfrastructure;
using BudgetUnderControl.MobileDomain.Repositiories;
using System;

namespace BudgetUnderControl.Mobile.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public IUserIdentityContext CreateUserIdentityContext()
        {
            var user =  this.userRepository.GetFirstUserAsync().Result;
            var externalId = Guid.NewGuid();
            if (!string.IsNullOrWhiteSpace(user.ExternalId))
            {
                externalId = Guid.Parse(user.ExternalId);
            }

            var context = new UserIdentityContext
            {
                UserId = user.Id,
                ExternalId = externalId,
            };
            return context;
        }

        public long GetIdOf1stUser()
        {
            var user = this.userRepository.GetFirstUserAsync().Result;
            return user.Id;
        }
    }
}
