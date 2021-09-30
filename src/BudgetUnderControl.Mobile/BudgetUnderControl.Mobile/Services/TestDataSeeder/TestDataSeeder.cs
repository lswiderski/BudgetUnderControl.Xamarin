using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.MobileDomain;
using BudgetUnderControl.MobileDomain.Repositiories;
using BudgetUnderControl.CommonInfrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetUnderControl.CommonInfrastructure;

namespace BudgetUnderControl.Mobile.Services
{
    public class TestDataSeeder : ITestDataSeeder
    {
        private readonly IAccountService accountService;
        private readonly ITransactionService transactionService;
        private readonly IUserIdentityContext userContext;
        private readonly ICurrencyService currencyService;
        private readonly ICategoryService categoryService;
        private readonly ICommandDispatcher commandDispatcher;

        public TestDataSeeder(IAccountService accountService, ITransactionService transactionService,
            ICurrencyService currencyService, ICategoryService categoryService,
             Func<IUserIdentityContext> userIdentityContextFunc, ICommandDispatcher commandDispatcher)
        {
            this.accountService = accountService;
            this.transactionService = transactionService;
            this.currencyService = currencyService;
            this.categoryService = categoryService;
            this.userContext = userIdentityContextFunc();
            this.commandDispatcher = commandDispatcher;
        }

        public async Task SeedAsync()
        {
            var currencies = await this.currencyService.GetCurriencesAsync();
            var addAccountCommand = new AddAccount
            {
                Amount = 0,
                CurrencyId = currencies.First().Id,
                IsIncludedInTotal = true,
                Name = "Test Account",
                Order = 0,
                Type = AccountType.Account
            };
            await commandDispatcher.DispatchAsync(addAccountCommand);

            var account = (await accountService.GetAccountsWithBalanceAsync()).First();
            var categories = await categoryService.GetCategoriesAsync();
            Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                var amount = random.Next(-20, 20);
                var addTransactionCommand = new AddTransaction
                {
                    AccountId = account.Id,
                    Amount = amount,
                    Date = DateTime.UtcNow,
                    Type = amount < 0 ? ExtendedTransactionType.Expense : ExtendedTransactionType.Income,
                    CategoryId = categories.First().Id,
                    Name = Guid.NewGuid().ToString()
                };
                await commandDispatcher.DispatchAsync(addTransactionCommand);
            }
        }
    }
}
