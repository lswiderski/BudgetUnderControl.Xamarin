using BudgetUnderControl.MobileDomain;
using BudgetUnderControl.MobileDomain.Repositiories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Mobile.Services
{
    public class SeedService : ISeedService
    {
        private readonly IUserRepository userRepository;
        private readonly ICategoryRepository categoryRepository;
        public SeedService(ICategoryRepository categoryRepository, IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.categoryRepository = categoryRepository;
        }

        public async Task CreateNewUserAsync()
        {

            var user = User.Create(Guid.NewGuid().ToString());
            await userRepository.AddUserAsync(user);
            await this.SeedCategoriesAsync(user.Id);
        }

        private async Task SeedCategoriesAsync(int userId)
        {
            var categories = new List<Category>()
            {
                Category.Create("Food", userId),
                Category.Create("Transport", userId),
                Category.Create("Other", userId).SetDefault(),
                Category.Create("Salary", userId),
                Category.Create("Taxes", userId),             
                Category.Create("Entertainment", userId),
                Category.Create("Health", userId),
                Category.Create("Interest", userId),
                Category.Create("Home", userId),
                Category.Create("Beauty", userId),
                Category.Create("Groceries", userId),
                Category.Create("Loans", userId),
                Category.Create("Gift", userId),
                Category.Create("Accommodation", userId),
                Category.Create("Sightseeing", userId),
                Category.Create("Shopping", userId),
                Category.Create("Professional Costs", userId),
            };
            await categoryRepository.AddCategoriesAsync(categories);
        }
    }
}
