using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.MobileDomain;
using BudgetUnderControl.MobileDomain.Repositiories;
using BudgetUnderControl.CommonInfrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using BudgetUnderControl.Common;
using NLog;
using BudgetUnderControl.CommonInfrastructure;

namespace BudgetUnderControl.Mobile.Services
{
    public class AccountMobileService : IAccountMobileService
    {
        private readonly IAccountMobileRepository accountRepository;
        private readonly IUserRepository userRepository;
        private readonly ILogger logger;
        private readonly IIconService iconService;

        public AccountMobileService(IAccountMobileRepository accountRepository, IUserRepository userRepository, ILogger logger, IIconService iconService)
        {
            this.accountRepository = accountRepository;
            this.userRepository = userRepository;
            this.logger = logger;
            this.iconService = iconService;
        }

        public async Task<EditAccountDTO> GetAccountAsync(Guid id)
        {
            var account = await accountRepository.GetAccountAsync(id.ToString());
            var balance = await accountRepository.GetActualBalanceAsync(account.Id);
            var dto = new EditAccountDTO
            {
                Id = account.Id,
                ExternalId = Guid.Parse(account.ExternalId),
                Name = account.Name,
                Number = account.Number,
                Comment = account.Comment,
                IsActive = account.IsActive,
                Type = account.Type,
                Order = account.Order,
                Amount = balance,
                Currency = account.Currency.Code,
                CurrencySymbol = account.Currency.Symbol,
                CurrencyId = account.CurrencyId,
                IsIncludedInTotal = account.IsIncludedToTotal,
                ParentAccountId = account.ParentAccountId,
                Icon = account.Icon
            };
            return dto;
        }

        public async Task<ICollection<AccountListItemDTO>> GetAccountsWithBalanceAsync()
        {
            var accounts = await accountRepository.GetAccountsAsync();

            var accountsWithBalance = accounts.Select(y => new AccountListItemDTO
            {
                Id = y.Id,
                ExternalId = Guid.Parse(y.ExternalId),
                Currency = y.Currency.Code,
                CurrencyId = y.CurrencyId,
                CurrencySymbol = y.Currency.Symbol,
                IsIncludedInTotal = y.IsIncludedToTotal,
                Name = y.Name,
                ParentAccountId = y.ParentAccountId,
                Icon = this.iconService.GetIcon(y.Icon),

            }).ToList();
            accountsWithBalance.ForEach(async x => { x.Balance = await accountRepository.GetActualBalanceAsync(x.Id); });
            return accountsWithBalance;
        }

        public async Task<ICollection<AccountListItemDTO>> GetAccountsForSelect(int? includeId = null)
        {
            var accounts = (await accountRepository.GetAccountsAsync(true)).ToList();

            if (includeId.HasValue && !accounts.Any(x => x.Id == includeId))
            {
                accounts.Add(await accountRepository.GetAccountAsync(includeId.Value));
            }

            var accountsWithBalance = accounts
                .OrderBy(x => x.Order)
                .Select(y => new AccountListItemDTO
                {
                    Id = y.Id,
                    ExternalId = Guid.Parse(y.ExternalId),
                    Currency = y.Currency.Code,
                    CurrencyId = y.CurrencyId,
                    CurrencySymbol = y.Currency.Symbol,
                    IsIncludedInTotal = y.IsIncludedToTotal,
                    Name = y.Name,
                    ParentAccountId = y.ParentAccountId,
                    Icon = this.iconService.GetIcon(y.Icon),

                }).ToList();
            accountsWithBalance.ForEach(async x => { x.Balance = await accountRepository.GetActualBalanceAsync(x.Id); });
            return accountsWithBalance;
        }

        public async Task<AccountDetailsDTO> GetAccountDetailsAsync(TransactionsFilter filter)
        {

            Account account;
            if (filter != null && filter.AccountsIds != null && filter.AccountsIds.Any())
            {
                account = await accountRepository.GetAccountAsync(filter.AccountsIds.First());
            }
            else if (filter != null && filter.AccountsExternalIds != null && filter.AccountsExternalIds.Any())
            {
                account = await accountRepository.GetAccountAsync(filter.AccountsExternalIds.First().ToString());
            }
            else
            {
                throw new ArgumentNullException();
            }

            var dto = new AccountDetailsDTO
            {
                Currency = account.Currency.Code,
                CurrencyId = account.CurrencyId,
                CurrencySymbol = account.Currency.Symbol,
                Id = account.Id,
                ExternalId = Guid.Parse(account.ExternalId),
                IsIncludedInTotal = account.IsIncludedToTotal,
                Name = account.Name,
                Comment = account.Comment,
                Type = account.Type,
                ParentAccountId = account.ParentAccountId,
                Order = account.Order,
                Income = await accountRepository.GetIncomeAsync(account.Id, filter.FromDate, filter.ToDate),
                Expense = await accountRepository.GetExpenseAsync(account.Id, filter.FromDate, filter.ToDate),
                Amount = await accountRepository.GetActualBalanceAsync(account.Id),
            };

            return dto;
        }

        public async Task AddAccountAsync(AddAccount command)
        {
            var user = await userRepository.GetFirstUserAsync();
            var account = Account.Create(command.Name, "", command.CurrencyId, command.IsIncludedInTotal, command.Comment, command.Order, command.Type, command.ParentAccountId, true, false, user.Id, command.ExternalId.ToString(), icon: command.Icon);
            await accountRepository.AddAccountAsync(account);

            if (account.Id <= 0)
            {
                throw new Exception();
            }

            if (account.Type != AccountType.Card)
            {
                await this.accountRepository.BalanceAdjustmentAsync(account.Id, command.Amount);
            }
        }

        public async Task EditAccountAsync(EditAccount command)
        {
            var account = await accountRepository.GetAccountAsync(command.Id);
            account.Edit(command.Name, "", command.CurrencyId, command.IsIncludedInTotal, command.Comment, command.Order, command.Type, command.ParentAccountId, command.IsActive, false, icon: command.Icon);
            await accountRepository.UpdateAsync(account);

            if (account.Type != AccountType.Card)
            {
                await this.accountRepository.BalanceAdjustmentAsync(account.Id, command.Amount);
            }
        }

        public async Task ActivateAccountAsync(int id)
        {
            var account = await accountRepository.GetAccountAsync(id);
            account.SetActive(true);

            await accountRepository.UpdateAsync(account);
        }

        public async Task DeactivateAccountAsync(int id)
        {
            var account = await accountRepository.GetAccountAsync(id);
            account.SetActive(false);

            await accountRepository.UpdateAsync(account);
        }

        public async Task DeactivateAccountAsync(string id)
        {
            var account = await accountRepository.GetAccountAsync(id);
            account.SetActive(false);

            await accountRepository.UpdateAsync(account);
        }

        public async Task DeleteAccountAsync(DeleteAccount command)
        {
            //temporary no removing AccountAvailable
            await this.DeactivateAccountAsync(command.Id.ToString());
            /*
             var transactions = this.Context.Transactions.Where(x => x.AccountId == id).ToList();

           if(IsSubCardAccount(id))
           {
               var parentAccountId = GetParentAccountId(id).Value;
               foreach (var transaction in transactions)
               {
                   transaction.AccountId = parentAccountId;
               }
           }
           else
           {
               this.Context.RemoveRange(transactions);
           }


           this.Context.SaveChanges();
           var account = await this.Context.Accounts.Where(x => x.Id == id).FirstOrDefaultAsync();
           this.Context.Remove<Account>(account);
           this.Context.SaveChanges();
            */
        }

        public async Task<bool> IsValidAsync(int accountId)
        {
            var accounts = await this.accountRepository.GetAccountsAsync(true);
            return accounts.Any(x => x.Id == accountId);
        }
    }
}
