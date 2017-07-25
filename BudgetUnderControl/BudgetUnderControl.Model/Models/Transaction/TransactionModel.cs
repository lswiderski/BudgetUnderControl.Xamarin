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
    public class TransactionModel : BaseModel, ITransactionModel
    {
        IContextConfig contextConfig;
        public TransactionModel(IContextConfig contextConfig) : base(contextConfig)
        {
            this.contextConfig = contextConfig;
        }

        public void AddTransaction(AddTransactionDTO arg)
        {
            var transaction = new Transaction
            {
                 AccountId = arg.AccountId,
                 Amount = arg.Amount,
                 CategoryId = arg.CategoryId,
                 Comment = arg.Comment,
                 Name = arg.Name,
                 Type = arg.Type,
                 Date = arg.CreatedOn,    
            };

            this.Context.Transactions.Add(transaction);
            this.Context.SaveChanges();
        }

        public async Task<ICollection<TransactionListItemDTO>> GetTransactions()
        {
            var transactions = (from t in this.Context.Transactions
                                join a in this.Context.Accounts on t.AccountId equals a.Id
                                join c in this.Context.Currencies on a.CurrencyId equals c.Id
                                orderby t.Date descending
                                select new TransactionListItemDTO
                                {
                                    AccountId = t.AccountId,
                                    Date = t.Date,
                                    Id = t.Id,
                                    Value = t.Amount,
                                    Account = a.Name,
                                    ValueWithCurrency = t.Amount + c.Symbol,
                                    Type = t.Type,
                                    Name = t.Name,
                                }
                                ).ToListAsync();

            return await transactions;
        }

        public async Task<ICollection<TransactionListItemDTO>> GetTransactions(int accountId)
        {
            var transactions = (from t in this.Context.Transactions
                                join a in this.Context.Accounts on t.AccountId equals a.Id
                                join c in this.Context.Currencies on a.CurrencyId equals c.Id
                                where a.Id == accountId
                                orderby t.Date descending
                                select new TransactionListItemDTO
                                {
                                    AccountId = t.AccountId,
                                    Date = t.Date,
                                    Id = t.Id,
                                    Value = t.Amount,
                                    Account = a.Name,
                                    ValueWithCurrency = t.Amount + c.Symbol,
                                    Type = t.Type,
                                    Name = t.Name,
                                }
                                ).ToListAsync();

            return await transactions;
        }

        public void AddTransfer(AddTransferDTO arg)
        {
            var transactionExpense = new Transaction
            {
                AccountId = arg.AccountId,
                Amount = arg.Amount,
                CategoryId = arg.CategoryId,
                Comment = arg.Comment,
                Name = arg.Name,
                Type =  Common.Enums.TransactionType.Expense,
                Date = arg.Date,
            };

            var transactionIncome = new Transaction
            {
                AccountId = arg.AccountId,
                Amount = arg.TransferAmount,
                CategoryId = arg.CategoryId,
                Comment = arg.Comment,
                Name = arg.Name,
                Type = Common.Enums.TransactionType.Income,
                Date = arg.TransferDate,
            };

            this.Context.Transactions.Add(transactionExpense);
            this.Context.Transactions.Add(transactionIncome);
            this.Context.SaveChanges();

            var transfer = new Transfer
            {
                FromTransactionId = transactionExpense.Id,
                ToTransactionId = transactionIncome.Id,
                Rate = arg.Rate
            };
            this.Context.Transfers.Add(transfer);
            this.Context.SaveChanges();
        }

        public EditTransactionDTO GetEditTransaction(int id)
        {
            var transaction = (from t in this.Context.Transactions
                                join a in this.Context.Accounts on t.AccountId equals a.Id
                                join c in this.Context.Currencies on a.CurrencyId equals c.Id
                                where t.Id == id
                                select new EditTransactionDTO
                                {
                                    AccountId = t.AccountId,
                                    Date = t.Date,
                                    Id = t.Id,
                                    Amount = t.Amount,
                                    CategoryId = c.Id,
                                    Comment = t.Comment,
                                    Name = t.Name,
                                    Type = t.Type
                                }
                               ).FirstOrDefault();

            transaction.ExtendedType = transaction.Type == TransactionType.Income ? ExtendedTransactionType.Income : ExtendedTransactionType.Expense;

            var transfer = this.Context.Transfers.Where(x => x.FromTransactionId == transaction.Id 
            || x.ToTransactionId == transaction.Id)
            .Select(x => new
            {
                x.FromTransactionId,
                x.ToTransactionId,
                x.Rate,
                x.Id
            })
            .FirstOrDefault();

            if(transfer != null)
            {
                int transferedTransactionId;
                if(transaction.Id == transfer.FromTransactionId)
                {
                    transferedTransactionId = transfer.ToTransactionId;
                }
                else
                {
                    transferedTransactionId = transfer.FromTransactionId;
                }

                var transferedTransaction = (from t in this.Context.Transactions
                                   join a in this.Context.Accounts on t.AccountId equals a.Id
                                   join c in this.Context.Currencies on a.CurrencyId equals c.Id
                                   where t.Id == transferedTransactionId
                                   select new EditTransactionDTO
                                   {
                                       AccountId = t.AccountId,
                                       Date = t.Date,
                                       Id = t.Id,
                                       Amount = t.Amount,
                                       CategoryId = c.Id,
                                       Comment = t.Comment,
                                       Name = t.Name,
                                       Type = t.Type
                                   }
                              ).FirstOrDefault();

                transaction.Rate = transfer.Rate;
                transaction.TransferId = transfer.Id;

                if (transaction.Id == transfer.FromTransactionId)
                {
                    transaction.TransferAmount = transferedTransaction.TransferAmount;
                    transaction.TransferDate = transferedTransaction.Date;
                    transaction.TransferTransactionId = transferedTransaction.Id;
                    transaction.TransferAccountId = transferedTransaction.AccountId;
                    
                }
                else
                {
                    transaction.TransferAmount = transaction.Amount;
                    transaction.Amount = transferedTransaction.Amount;

                    transaction.TransferDate = transaction.Date;
                    transaction.Date = transferedTransaction.Date;

                    transaction.TransferTransactionId = transaction.Id;
                    transaction.Id = transferedTransaction.Id;


                    transaction.TransferAccountId = transaction.AccountId;
                    transaction.AccountId = transferedTransaction.AccountId;
                }
                transaction.ExtendedType = ExtendedTransactionType.Transfer;

            }

            return transaction;
        }

        public void EditTransaction(EditTransactionDTO arg)
        {
            var firstTransaction = this.Context.Transactions.Where(x => x.Id == arg.Id).FirstOrDefault();
            Transaction secondTransaction = null;
            var transfer = this.Context.Transfers.Where(x => x.FromTransactionId == arg.Id)

           .FirstOrDefault();

            if (transfer != null)
            {
                secondTransaction = this.Context.Transactions.Where(x => x.Id == transfer.ToTransactionId).FirstOrDefault();
            }


            //remove transfer, no more transfer
            if (arg.ExtendedType != Common.Enums.ExtendedTransactionType.Transfer 
                && transfer != null && secondTransaction != null)
            {
                this.Context.Remove<Transfer>(transfer);
                this.Context.Remove<Transaction>(secondTransaction);

                firstTransaction.AccountId = arg.AccountId;
                firstTransaction.Amount = arg.Amount;
                firstTransaction.CategoryId = arg.CategoryId;
                firstTransaction.Comment = arg.Comment;
                firstTransaction.Name = arg.Name;
                firstTransaction.Type = arg.Type;
                firstTransaction.Date = arg.Date;
            }
            //new Transfer, no transfer before
            else if(arg.ExtendedType == Common.Enums.ExtendedTransactionType.Transfer
                && transfer == null && secondTransaction == null)
            {
                firstTransaction.AccountId = arg.AccountId;
                firstTransaction.Amount = arg.Amount;
                firstTransaction.CategoryId = arg.CategoryId;
                firstTransaction.Comment = arg.Comment;
                firstTransaction.Name = arg.Name;
                firstTransaction.Type = TransactionType.Expense;
                firstTransaction.Date = arg.Date;

                var transactionIncome = new Transaction
                {
                    AccountId = arg.TransferAccountId.Value,
                    Amount = arg.TransferAmount.Value,
                    CategoryId = arg.CategoryId,
                    Comment = arg.Comment,
                    Name = arg.Name,
                    Type = TransactionType.Income,
                    Date = arg.TransferDate.Value,
                };

                this.Context.Transactions.Add(transactionIncome);
                this.Context.SaveChanges();

                var newTransfer = new Transfer
                {
                    FromTransactionId = firstTransaction.Id,
                    ToTransactionId = transactionIncome.Id,
                    Rate = arg.Rate.Value
                };
                this.Context.Transfers.Add(newTransfer);

            }
            //edit transfer
            else if (arg.ExtendedType == Common.Enums.ExtendedTransactionType.Transfer
                && transfer != null && secondTransaction != null)
            {
                firstTransaction.AccountId = arg.AccountId;
                firstTransaction.Amount = arg.Amount;
                firstTransaction.CategoryId = arg.CategoryId;
                firstTransaction.Comment = arg.Comment;
                firstTransaction.Name = arg.Name;
                firstTransaction.Type = TransactionType.Expense;
                firstTransaction.Date = arg.Date;


                secondTransaction.AccountId = arg.TransferAccountId.Value;
                secondTransaction.Amount = arg.TransferAmount.Value;
                secondTransaction.CategoryId = arg.CategoryId;
                secondTransaction.Comment = arg.Comment;
                secondTransaction.Name = arg.Name;
                secondTransaction.Type = TransactionType.Income;
                secondTransaction.Date = arg.TransferDate.Value;

                transfer.Rate = arg.Rate.Value;
            }
            //just edit 1 transaction, no transfer before
            else if(arg.ExtendedType != Common.Enums.ExtendedTransactionType.Transfer
                && transfer == null && secondTransaction == null)
            {
                firstTransaction.AccountId = arg.AccountId;
                firstTransaction.Amount = arg.Amount;
                firstTransaction.CategoryId = arg.CategoryId;
                firstTransaction.Comment = arg.Comment;
                firstTransaction.Name = arg.Name;
                firstTransaction.Type = arg.Type;
                firstTransaction.Date = arg.Date;
            }

            this.Context.SaveChanges();
        }
    }
}