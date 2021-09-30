using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.MobileDomain;
using BudgetUnderControl.MobileDomain.Repositiories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgetUnderControl.CommonInfrastructure;

namespace BudgetUnderControl.Mobile.Repositories
{
    public class TransactionRepository : BaseModel, ITransactionRepository
    {
        private readonly IAccountMobileRepository accountRepository;
       
        private Func<IUserIdentityContext> userIdentityContextFunc;
        private IUserIdentityContext userIdentityContext
        {
            get
            {
                return this.userIdentityContextFunc();
            }
        }

        public TransactionRepository(IContextFacade context, IAccountMobileRepository accountRepository,
            Func<IUserIdentityContext> userIdentityContextFunc) : base(context)
        {
            this.accountRepository = accountRepository;
            this.userIdentityContextFunc = userIdentityContextFunc;
        }

        public async Task AddTransactionAsync(Transaction transaction)
        {
            this.Context.Transactions.Add(transaction);
            await this.Context.SaveChangesAsync();
        }

        public async Task AddTransactionsAsync(IEnumerable<Transaction> transactions)
        {
            this.Context.Transactions.AddRange(transactions);
            await this.Context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            transaction.UpdateModify();
            this.Context.Transactions.Update(transaction);
            await this.Context.SaveChangesAsync();
        }

        public async Task UpdateAsync(IEnumerable<Transaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                transaction.UpdateModify();
            }

            this.Context.Transactions.UpdateRange(transactions);
            await this.Context.SaveChangesAsync();
        }

        public async Task AddTransferAsync(Transfer transfer)
        {
            this.Context.Transfers.Add(transfer);
            await this.Context.SaveChangesAsync();
        }

        public async Task AddTransfersAsync(IEnumerable<Transfer> transfers)
        {
            this.Context.Transfers.AddRange(transfers);
            await this.Context.SaveChangesAsync();
        }

        public async Task UpdateTransferAsync(Transfer transfer)
        {
            transfer.UpdateModify();
            this.Context.Transfers.Update(transfer);
            await this.Context.SaveChangesAsync();
        }

        public async Task RemoveTransactionAsync(Transaction transaction)
        {
            transaction.Delete();
            await this.Context.SaveChangesAsync();
        }

        public async Task RemoveTransferAsync(Transfer transfer)
        {
            transfer.Delete();
            await this.Context.SaveChangesAsync();
        }

        public async Task HardRemoveTransactionsAsync(IEnumerable<Transaction> transactions)
        {
            this.Context.Transactions.RemoveRange(transactions);
            await this.Context.SaveChangesAsync();
        }

        public async Task HardRemoveTransfersAsync(IEnumerable<Transfer> transfers)
        {
            this.Context.Transfers.RemoveRange(transfers);
            await this.Context.SaveChangesAsync();
        }

        public async Task<ICollection<Transaction>> GetTransactionsAsync(TransactionsFilter filter = null)
        {
            //var userId = userIdentityContext.UserId;
            var query = this.Context.Transactions
                        .Include(p => p.Category)
                        .Include(p => p.Account)
                            .ThenInclude(p => p.Currency)
                        .Include(p => p.TagsToTransaction)
                            .ThenInclude(p => p.Tag)
                        .Include(p => p.ToTransfers)
                        .Include(p => p.FromTransfers)
                         .Include(p => p.FilesToTransaction)
                            .ThenInclude(p => p.File)
                        .AsQueryable();


            if (filter != null && filter.AccountsIds != null && filter.AccountsIds.Any())
            {
                var accounts = await this.accountRepository.GetSubAccountsAsync(filter.AccountsIds);
                accounts.AddRange(filter.AccountsIds);
                accounts = accounts.Distinct().ToList();

                query = query.Where(q => accounts.Contains(q.AccountId)).AsQueryable();
            }
            else if (filter != null && filter.AccountsExternalIds != null && filter.AccountsExternalIds.Any())
            {
                var accounts = await this.accountRepository.GetSubAccountsAsync(filter.AccountsExternalIds.Select(x => x.ToString()).ToList());
                accounts.AddRange(filter.AccountsExternalIds.Select(x => x.ToString()).ToList());
                accounts = accounts.Distinct().ToList();

                query = query.Where(q => accounts.Contains(q.Account.ExternalId)).AsQueryable();
            }
            else
            {
                query = query.Where(q => q.Account.OwnerId == userIdentityContext.UserId).AsQueryable();
            }

            if (filter != null && filter.CategoryIds != null && filter.CategoryIds.Any())
            {
                query = query.Where(q => q.CategoryId.HasValue && filter.CategoryIds.Contains(q.CategoryId.Value)).AsQueryable();
            }

            if (filter != null && filter.TagIds != null && filter.TagIds.Any())
            {
                query = (from transaction in query
                         join t2t in this.Context.TagsToTransactions on transaction.Id equals t2t.TransactionId
                         where filter.TagIds.Contains(t2t.TagId)
                         select transaction).AsQueryable();
            }


            if (filter != null)
            {
                if (!filter.IncludeDeleted)
                {
                    query = query.Where(q => q.IsDeleted == false).AsQueryable();
                }


                if (filter.FromDate != null)
                {
                    query = query.Where(q => q.Date >= filter.FromDate).AsQueryable();
                }

                if (filter.ToDate != null)
                {
                    query = query.Where(q => q.Date <= filter.ToDate).AsQueryable();
                }

                if (filter.ChangedSince != null)
                {
                    query = query.Where(q => q.CreatedOn >= filter.ChangedSince || q.ModifiedOn >= filter.ChangedSince).AsQueryable();
                }
            }
            else
            {
                query = query.Where(q => q.IsDeleted == false).AsQueryable();
            }

           // var temporary = await query.ToListAsync();

            var transactionsWithExtraProperty = (await (from t in query
                                                        orderby t.Date descending
                                                        select new
                                                        {
                                                            t,
                                                            IsTransfer = t.ToTransfers.Any() || t.FromTransfers.Any(),
                                                        })
                                                   .ToListAsync());

            transactionsWithExtraProperty.ForEach(x => x.t.IsTransfer = x.IsTransfer);
            var transactions = transactionsWithExtraProperty.Select(x => x.t).ToList();

            if (filter != null && !string.IsNullOrWhiteSpace(filter.SearchQuery))
            {
                transactions = transactions
                    .Where(x => (x.Name != null && x.Name.Contains(filter.SearchQuery, StringComparison.InvariantCultureIgnoreCase))
                || x.Amount.ToString().Contains(filter.SearchQuery, StringComparison.InvariantCultureIgnoreCase)
                || (x.Comment != null && x.Comment.Contains(filter.SearchQuery, StringComparison.InvariantCultureIgnoreCase))
                || (x.Category != null && x.Category.Name.Contains(filter.SearchQuery, StringComparison.InvariantCultureIgnoreCase))
                || (x.Account != null && x.Account.Name.Contains(filter.SearchQuery, StringComparison.InvariantCultureIgnoreCase))
                ).ToList();
            }
            return transactions;



        }

        public async Task<Transaction> GetTransactionAsync(int id)
        {
            var transaction = await this.Context.Transactions
                            .Include(p => p.FilesToTransaction)
                                .ThenInclude(p => p.File)
                            .Where(t => t.Id == id).SingleOrDefaultAsync();
            return transaction;
        }

        public async Task<Transaction> GetTransactionAsync(string id)
        {
            var transaction = await this.Context.Transactions
                .Include(p => p.FilesToTransaction)
                                .ThenInclude(p => p.File)
                                .Where(t => t.ExternalId == id).SingleOrDefaultAsync();
            return transaction;
        }

        public async Task<Transfer> GetTransferAsync(int transactionId)
        {
            var transfer = await this.Context.Transfers.Where(t => t.FromTransactionId == transactionId || t.ToTransactionId == transactionId).SingleOrDefaultAsync();
            return transfer;
        }

        public async Task<Transfer> GetTransferAsync(string transactionId)
        {
            var transfer = await this.Context.Transfers.Where(t => t.ExternalId == transactionId).SingleOrDefaultAsync();
            return transfer;
        }


        public async Task<ICollection<Transfer>> GetTransfersAsync()
        {
            var transfers = await this.Context.Transfers
                .Include(t => t.FromTransaction)
                .Include(t => t.ToTransaction)
                .ToListAsync();

            return transfers;
        }

        public async Task<ICollection<Transfer>> GetTransfersModifiedSinceAsync(DateTime changedSince)
        {
            var transfers = await this.Context.Transfers
                .Include(t => t.FromTransaction)
                .Include(t => t.ToTransaction)
                .Where(q => q.ModifiedOn >= changedSince)
                .ToListAsync();

            return transfers;
        }
    }
}
