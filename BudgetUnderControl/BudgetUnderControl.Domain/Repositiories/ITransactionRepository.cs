﻿using BudgetUnderControl.Common;
using BudgetUnderControl.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Domain.Repositiories
{
    public interface ITransactionRepository
    {
        Task<ICollection<Transaction>> GetTransactionsAsync(TransactionsFilter filter = null);
        Task<Transaction> GetTransactionAsync(int id);
        Task AddTransactionAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task AddTransferAsync(Transfer transfer);
        Task UpdateTransferAsync(Transfer transfer);
        Task RemoveTransactionAsync(Transaction transaction);
        Task RemoveTransferAsync(Transfer transfer);
        Task<Transfer> GetTransferAsync(int transactionId);
    }
}