using Autofac;
using BudgetUnderControl.CommonInfrastructure;
using BudgetUnderControl.Mobile.IoC;
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
                Category.Create("Food", userId).SetIcon("Utensils.FAS"),
                Category.Create("Transport", userId).SetIcon("Bus.FAS"),
                Category.Create("Other", userId).SetDefault().SetIcon("StickyNote.FAR"),
                Category.Create("Salary", userId).SetIcon("HandHoldingUsd.FAS"),
                Category.Create("Taxes", userId).SetIcon("Receipt.FAS"),             
                Category.Create("Entertainment", userId).SetIcon("GlassCheers.FAS"),
                Category.Create("Health", userId).SetIcon("Heartbeat.FAS"),
                Category.Create("Interest", userId).SetIcon("MapMarkerAlt.FAS"),
                Category.Create("Home", userId).SetIcon("Home.FAS"),
                Category.Create("Beauty", userId).SetIcon("Smile.FAR"),
                Category.Create("Groceries", userId).SetIcon("ShoppingBasket.FAS"),
                Category.Create("Loans", userId).SetIcon("MoneyCheckAlt.FAS"),
                Category.Create("Gift", userId).SetIcon("Gift.FAS"),
                Category.Create("Accommodation", userId).SetIcon("Bed.FAS"),
                Category.Create("Sightseeing", userId).SetIcon("Landmark.FAS"),
                Category.Create("Shopping", userId).SetIcon("Tshirt.FAS"),
                Category.Create("Professional Costs", userId).SetIcon("Suitcase.FAS"),
            };
            await categoryRepository.AddCategoriesAsync(categories);
        }
    }
}
