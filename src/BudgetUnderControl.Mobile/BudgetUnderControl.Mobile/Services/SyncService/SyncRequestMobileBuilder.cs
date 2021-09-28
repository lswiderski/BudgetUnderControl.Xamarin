using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.CommonInfrastructure;
using BudgetUnderControl.CommonInfrastructure.Commands;
using BudgetUnderControl.CommonInfrastructure.Settings;
using BudgetUnderControl.MobileDomain.Repositiories;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Mobile.Services
{
    public class SyncRequestMobileBuilder : ISyncRequestBuilder
    {
        private readonly ILogger logger;
        private readonly ITransactionRepository transactionRepository;
        private readonly IAccountMobileRepository accountRepository;
        private readonly ICurrencyRepository currencyRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IUserRepository userRepository;
        private readonly ISynchronizationRepository synchronizationRepository;
        private readonly IUserIdentityContext userIdentityContext;
        private readonly ITagRepository tagRepository;
        private readonly IFileRepository fileRepository;
        private readonly IFileHelper fileHelper;
        private readonly GeneralSettings settings;
        public SyncRequestMobileBuilder(ITransactionRepository transactionRepository,
            IAccountMobileRepository accountRepository,
            ICurrencyRepository currencyRepository,
            ICategoryRepository categoryRepository,
            IUserRepository userRepository,
            ISynchronizationRepository synchronizationRepository,
            IUserIdentityContext userIdentityContext,
            ITagRepository tagRepository,
            ILogger logger,
            IFileRepository fileRepository,
            IFileHelper fileHelper,
            GeneralSettings settings)
        {
            this.transactionRepository = transactionRepository;
            this.accountRepository = accountRepository;
            this.currencyRepository = currencyRepository;
            this.categoryRepository = categoryRepository;
            this.userRepository = userRepository;
            this.synchronizationRepository = synchronizationRepository;
            this.userIdentityContext = userIdentityContext;
            this.settings = settings;
            this.tagRepository = tagRepository;
            this.logger = logger;
            this.fileRepository = fileRepository;
            this.fileHelper = fileHelper;
        }

        public async Task<SyncRequest> CreateSyncRequestAsync(SynchronizationComponent source, SynchronizationComponent target)
        {
            //get
            var synchronizations = await this.synchronizationRepository.GetSynchronizationsAsync();
            var synchronization = synchronizations.Where(x => x.Component == target && x.UserId == userIdentityContext.UserId).FirstOrDefault();

            var request = new SyncRequest
            {
                Component = source,
                ComponentId = new Guid(settings.ApplicationId),
                UserId = userIdentityContext.ExternalId,
                LastSync = synchronization != null ? synchronization.LastSyncAt : new DateTime(),
            };

            //collect collections to send to update
            //modified on || created on > LastSync

            request = await this.GetCollectionsToSyncAsync(request);

            return request;
        }

        private async Task<SyncRequest> GetCollectionsToSyncAsync(SyncRequest syncRequest)
        {
            syncRequest.Transactions = await this.GetTransactionsToSyncAsync(syncRequest.LastSync);
            syncRequest.Transfers = await this.GetTransfersToSyncAsync(syncRequest.LastSync);
            syncRequest.Accounts = await this.GetAccountsToSyncAsync(syncRequest.LastSync);
            syncRequest.Users = await this.GetUsersToSyncAsync(syncRequest.LastSync);
            syncRequest.Categories = await this.GetCategoriesToSyncAsync(syncRequest.LastSync);
            syncRequest.Tags = await this.GetTagsToSyncAsync(syncRequest.LastSync);
            syncRequest.ExchangeRates = await this.GetExhangeRatesToSyncAsync();
            syncRequest.Files = await this.GetFilesToSyncAsync(syncRequest.LastSync);

            return syncRequest;
        }

        private async Task<IEnumerable<TransactionSyncDTO>> GetTransactionsToSyncAsync(DateTime changedSince)
        {
            var transactionsQuery = (await transactionRepository.GetTransactionsAsync(new TransactionsFilter { ChangedSince = changedSince, IncludeDeleted = true }));
            var transactions = transactionsQuery
                .Select(x => new TransactionSyncDTO
                {
                    Name = x.Name,
                    Comment = x.Comment,
                    AccountId = x.AccountId,
                    Amount = x.Amount,
                    CategoryId = x.CategoryId,
                    CreatedOn = x.CreatedOn,
                    Date = x.Date,
                    Id = x.Id,
                    ExternalId = Guid.Parse(x.ExternalId),
                    ModifiedOn = x.ModifiedOn,
                    Type = x.Type,
                    IsDeleted = x.IsDeleted,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    Tags = x.TagsToTransaction.Select(y => new TagSyncDTO
                    {
                        ExternalId = Guid.Parse(y.Tag.ExternalId),
                        Id = y.Tag.Id,
                        IsDeleted = y.Tag.IsDeleted,
                        ModifiedOn = y.Tag.ModifiedOn,
                        Name = y.Tag.Name
                    }).ToList(),
                    Files = x.FilesToTransaction.Select(y => new FileToTransactionSyncDTO
                    {
                        ExternalId = Guid.Parse(y.ExternalId),
                        FileId = Guid.Parse(y.File.ExternalId),
                        IsDeleted = y.IsDeleted,
                        ModifiedOn = y.ModifiedOn,
                        TransactionId = y.TransactionId,
                    }).ToList(),
                }).ToList();

            var accounts = (await this.accountRepository.GetAccountsAsync()).ToDictionary(x => x.Id, x => x.ExternalId);
            var categories = (await this.categoryRepository.GetCategoriesAsync()).ToDictionary(x => x.Id, x => x.ExternalId);


            foreach (var transaction in transactions)
            {
                transaction.AccountExternalId = Guid.Parse(accounts[transaction.AccountId]);
                if (transaction.CategoryId.HasValue)
                {
                    transaction.CategoryExternalId = Guid.Parse(categories[transaction.CategoryId.Value]);
                }
            }

            return transactions;
        }

        private async Task<IEnumerable<TransferSyncDTO>> GetTransfersToSyncAsync(DateTime changedSince)
        {
            var transfers = (await this.transactionRepository.GetTransfersModifiedSinceAsync(changedSince)).Select(x => new TransferSyncDTO
            {
                Id = x.Id,
                FromTransactionId = x.FromTransactionId,
                Rate = x.Rate,
                ToTransactionId = x.ToTransactionId,
                ExternalId = Guid.Parse(x.ExternalId),
                IsDeleted = x.IsDeleted,
                ModifiedOn = x.ModifiedOn,
                ToTransactionExternalId = Guid.Parse(x.ToTransaction.ExternalId),
                FromTransactionExternalId = Guid.Parse(x.FromTransaction.ExternalId),
                 
            }).ToList();

            return transfers;
        }

        private async Task<IEnumerable<AccountSyncDTO>> GetAccountsToSyncAsync(DateTime changedSince)
        {
            var accounts = (await this.accountRepository.GetAccountsAsync())
                .Where(x => x.ModifiedOn >= changedSince)
                .Select(x => new AccountSyncDTO
            {
                Id = x.Id,
                ExternalId = Guid.Parse(x.ExternalId),
                Comment = x.Comment,
                CurrencyId = x.CurrencyId,
                IsIncludedToTotal = x.IsIncludedToTotal,
                Name = x.Name,
                Order = x.Order,
                ParentAccountId = x.ParentAccountId,
                Type = x.Type,
                ModifiedOn = x.ModifiedOn,
                Icon = x.Icon,
                IsDeleted = x.IsDeleted,
                IsActive = x.IsActive,
            }).ToList();

            var allAccounts = (await this.accountRepository.GetAccountsAsync()).ToDictionary(x => x.Id, x => x.ExternalId);
            foreach (var account in accounts)
            {
                if (account.ParentAccountId.HasValue)
                {
                    account.ParentAccountExternalId = Guid.Parse(allAccounts[account.ParentAccountId.Value]);
                }
            }

            return accounts;
        }

        private async Task<IEnumerable<UserSyncDTO>> GetUsersToSyncAsync(DateTime changedSince)
        {
            //temporary I do not support multi users
            var user = await this.userRepository.GetFirstUserAsync();
            var result = new List<UserSyncDTO>();

            result.Add(new UserSyncDTO
            {
                ExternalId = Guid.Parse(user.ExternalId),
                ModifiedOn = user.ModifiedOn,
                CreatedAt = user.CreatedAt,
                IsDeleted = user.IsDeleted,
            });
            

            return result;
        }

        private async Task<IEnumerable<CategorySyncDTO>> GetCategoriesToSyncAsync(DateTime changedSince)
        {
            var categories = (await this.categoryRepository.GetCategoriesAsync())
                .Where(x => x.ModifiedOn >= changedSince)
                .Select(x => new CategorySyncDTO
                {
                    Id = x.Id,
                    ExternalId = Guid.Parse(x.ExternalId),
                    Name = x.Name,
                    ModifiedOn = x.ModifiedOn,
                    IsDeleted = x.IsDeleted,
                    Icon = x.Icon,
                }).ToList();

            var userExternalId = (await this.userRepository.GetFirstUserAsync()).ExternalId;

            foreach (var category in categories)
            {
                category.OwnerExternalId = Guid.Parse(userExternalId);
            }
            return categories;
        }

        private async Task<IEnumerable<TagSyncDTO>> GetTagsToSyncAsync(DateTime changedSince)
        {
            var tags = (await this.tagRepository.GetAsync())
                .Where(x => x.ModifiedOn >= changedSince)
                .Select(x => new TagSyncDTO
                {
                    Id = x.Id,
                    ExternalId = Guid.Parse(x.ExternalId),
                    Name = x.Name,
                    ModifiedOn = x.ModifiedOn,
                    IsDeleted = x.IsDeleted
                }).ToList();

            return tags;
        }

        private async Task<IEnumerable<ExchangeRateSyncDTO>> GetExhangeRatesToSyncAsync()
        {
            var rates = (await this.currencyRepository.GetExchangeRatesAsync())
                .Select(x => new ExchangeRateSyncDTO
                {
                    Date = x.Date,
                    Rate = x.Rate,
                    FromCurrency = x.FromCurrency.Code,
                    ToCurrency = x.ToCurrency.Code,
                    IsDeleted = x.IsDeleted,
                    ExternalId = Guid.Parse(x.ExternalId),
                    ModifiedOn = x.ModifiedOn
                }).ToList();

            return rates;
        }

        private async Task<IEnumerable<FileSyncDTO>> GetFilesToSyncAsync(DateTime changedSince)
        {
            var files = (await this.fileRepository.GetAsync())
                .Where(x => x.ModifiedOn >= changedSince)
                .ToList()
                .Select(x => new FileSyncDTO
                {
                    Id = Guid.Parse(x.ExternalId),
                    ExternalId = Guid.Parse(x.ExternalId),
                    FileName = x.FileName,
                    ContentType = x.MimeType,
                    UserId = userIdentityContext.ExternalId,
                    CreatedOn = x.CreatedOn,
                    ModifiedOn = x.ModifiedOn,
                    IsDeleted = x.IsDeleted,
                }).ToList();

            for (int i = 0; i < files.Count; i++)
            {
                files[i].Content = await fileHelper.LoadFileBytesAsync(files[i].Id.ToString());
            }

            return files;
        }
    }
}
