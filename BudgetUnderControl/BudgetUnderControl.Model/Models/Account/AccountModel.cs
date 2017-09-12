﻿using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model
{
    public class AccountModel : BaseModel, IAccountModel
    {

        IContextConfig contextConfig;
        ITransactionModel transactionModel;
        public AccountModel(IContextConfig contextConfig, ITransactionModel transactionModel) : base(contextConfig)
        {
            this.contextConfig = contextConfig;

            this.transactionModel = transactionModel;
        }

        
        public ICollection<AccountListItemDTO> GetAccounts()
        {
            var list = (from account in this.Context.Accounts
                        join currency in this.Context.Currencies on account.CurrencyId equals currency.Id
                        select new
                        {
                            Currency = currency.Code,
                            CurrencyId = currency.Id,
                            CurrencySymbol = currency.Symbol,
                            Id = account.Id,
                            IsIncludedInTotal = account.IsIncludedToTotal,
                            Name = account.Name,
                            Type = account.Type,
                            Order = account.Order,
                            subAccounts = this.Context.Accounts.Where(x => x.ParentAccountId == account.Id)
                                        .Select(x => x.Id)
                                .ToList(),
        }
                        )
                        .OrderBy(x => x.Order)
                        .ToList()
                        .Select(y => new AccountListItemDTO
                        {
                            Currency = y.Currency,
                            CurrencyId = y.CurrencyId,
                            CurrencySymbol = y.CurrencySymbol,
                            Id = y.Id,
                            IsIncludedInTotal = y.IsIncludedInTotal,
                            Name = y.Name,
                            Balance = y.Type == AccountType.Card ? 0 : this.Context.Transactions.Where(x => x.AccountId == y.Id || y.subAccounts.Contains(x.AccountId)).Sum(x => x.Amount),
                        }).ToList();

            return  list;
        }

        public async void AddAccount(AddAccountDTO vm)
        {
            var account = new Account
            {
                AccountGroupId = vm.AccountGroupId,
                Comment = vm.Comment,
                CurrencyId = vm.CurrencyId,
                IsIncludedToTotal = vm.IsIncludedInTotal,
                Name = vm.Name,
                Type = vm.Type,
                ParentAccountId = vm.ParentAccountId,
                Order = vm.Order
            };
            this.Context.Accounts.Add(account);
            await this.Context.SaveChangesAsync();
            if (account.Type != AccountType.Card)
            {
                this.BalanceAdjustment(account.Id, vm.Amount);
            }
        }

        public async Task<EditAccountDTO> GetAccount(int id)
        {
            var accounts = transactionModel.GetSubAccounts(id);
            accounts.Add(id);
            accounts = accounts.Distinct().ToList();

            var acc = (from account in this.Context.Accounts
                       join currency in this.Context.Currencies on account.CurrencyId equals currency.Id
                       where account.Id == id
                       select new EditAccountDTO
                       {
                           Currency = currency.Code,
                           CurrencyId = currency.Id,
                           CurrencySymbol = currency.Symbol,
                           Id = account.Id,
                           IsIncludedInTotal = account.IsIncludedToTotal,
                           Name = account.Name,
                           Comment = account.Comment,
                           AccountGroupId = account.AccountGroupId,
                           Amount = account.Type == AccountType.Card ? 0 : this.Context.Transactions.Where(x => x.AccountId == account.Id || accounts.Contains(x.AccountId)).Sum(x => x.Amount),
                           Type = account.Type,
                           ParentAccountId = account.ParentAccountId,
                           Order = account.Order,
                       }
                        ).FirstOrDefaultAsync();

            return await acc;
        }

        public async void EditAccount(EditAccountDTO vm)
        {
            var account = await this.Context.Accounts.Where(x => x.Id == vm.Id).FirstOrDefaultAsync();
            account.AccountGroupId = vm.AccountGroupId;
            account.Comment = vm.Comment;
            account.CurrencyId = vm.CurrencyId;
            account.IsIncludedToTotal = vm.IsIncludedInTotal;
            account.Name = vm.Name;
            account.Type = vm.Type;
            account.ParentAccountId = vm.ParentAccountId;
            account.Order = vm.Order;
            this.Context.SaveChanges();

            if(account.Type != AccountType.Card)
            {
                this.BalanceAdjustment(account.Id, vm.Amount);
            }
            
        }

        public async void RemoveAccount(int id)
        {
            var transactions = this.Context.Transactions.Where(x => x.AccountId == id).ToList();
            this.Context.RemoveRange(transactions);
            this.Context.SaveChanges();
            var account = await this.Context.Accounts.Where(x => x.Id == id).FirstOrDefaultAsync();
            this.Context.Remove<Account>(account);
            this.Context.SaveChanges();
        }

        private decimal GetActualBalance(int accountId)
        {
            var accounts = transactionModel.GetSubAccounts(accountId);
            accounts.Add(accountId);
            accounts = accounts.Distinct().ToList();

            var balance = this.Context.Transactions.Where(x => accounts.Contains(x.AccountId)).Sum(x => x.Amount);
            return balance;
        }

        private void BalanceAdjustment(int accountId, decimal targetBalance)
        {
            var actualBalance = GetActualBalance(accountId);

            if(!decimal.Equals(actualBalance, targetBalance))
            {
                var amount = targetBalance - actualBalance;

                Math.Sign(amount);
                var transactionDTO = new AddTransactionDTO
                {
                    AccountId = accountId,
                    CreatedOn = DateTime.UtcNow,
                    Comment = string.Empty,
                    Name = "BalanceAdjustment",
                    Amount = amount,
                    Type = Math.Sign(amount) < 0 ? TransactionType.Expense : TransactionType.Income,
                };

                transactionModel.AddTransaction(transactionDTO);
            }
           
        }

    }
}
