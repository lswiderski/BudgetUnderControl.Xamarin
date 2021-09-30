using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.CommonInfrastructure;
using BudgetUnderControl.CommonInfrastructure.Commands;
using BudgetUnderControl.CommonInfrastructure.Settings;
using BudgetUnderControl.Mobile.Services;
using BudgetUnderControl.MobileDomain;
using BudgetUnderControl.MobileDomain.Repositiories;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xamarin.Essentials;

namespace BudgetUnderControl.Mobile.Services
{
    public class MobileSynchroniser : ISynchroniser
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
        private readonly ITransactionService transactionService;
        private readonly IFileRepository fileRepository;
        private readonly IFileHelper fileHelper;
        private readonly GeneralSettings settings;
        private Dictionary<string, int> _tags;

        public MobileSynchroniser(ITransactionRepository transactionRepository,
            IAccountMobileRepository accountRepository,
            ICurrencyRepository currencyRepository,
            ICategoryRepository categoryRepository,
            IUserRepository userRepository,
            ISynchronizationRepository synchronizationRepository,
            Func<IUserIdentityContext> userIdentityContextFunc,
            ITagRepository tagRepository,
            ITransactionService transactionService,
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
            this.userIdentityContext = userIdentityContextFunc();
            this.settings = settings;
            this.tagRepository = tagRepository;
            this.transactionService = transactionService;
            this.logger = logger;
            this.fileRepository = fileRepository;
            this.fileHelper = fileHelper;
        }

        public async Task SynchroniseAsync(SyncRequest syncRequest)
        {
           
            await this.UpdateUsersAsync(syncRequest.Users);
            await this.UpdateTagsAsync(syncRequest.Tags);
            await this.UpdateCategoriesAsync(syncRequest.Categories);
            await this.UpdateAccountsAsync(syncRequest.Accounts);
            await this.UpdateFilesAsync(syncRequest.Files);
            _tags = (await this.tagRepository.GetAsync()).ToDictionary(x => x.ExternalId, x => x.Id, StringComparer.InvariantCultureIgnoreCase);
            await this.UpdateTransactionsAsync(syncRequest.Transactions);
            await this.UpdateTransfersAsync(syncRequest.Transfers);
            await this.UpdateExchangeRatesAsync(syncRequest.ExchangeRates);
            await this.UpdateLastSyncDateAsync(syncRequest);
            SecureStorage.Remove("Balance_Total");
        }

        private async Task UpdateLastSyncDateAsync(SyncRequest syncRequest)
        {
            var userId =this.userIdentityContext.UserId;
            var syncObject = await this.synchronizationRepository.GetSynchronizationAsync(syncRequest.Component, syncRequest.ComponentId.ToString(), userId);

            if (syncObject != null)
            {
                syncObject.LastSyncAt = DateTime.UtcNow;
                await this.synchronizationRepository.UpdateAsync(syncObject);
            }
            else
            {
                syncObject = new Synchronization
                {
                    LastSyncAt = DateTime.UtcNow,
                    Component = syncRequest.Component,
                    ComponentId = syncRequest.ComponentId.ToString(),
                    UserId = userId,
                };

                await this.synchronizationRepository.AddSynchronizationAsync(syncObject);
            }
        }

        private async Task UpdateTransactionsAsync(IEnumerable<TransactionSyncDTO> transactions)
        {
            if (transactions == null || !transactions.Any())
            {
                return;
            }

            var transactionsToUpdate = new List<MobileDomain.Transaction>();
            var transactionsToAdd = new List<MobileDomain.Transaction>();

            const int packageSize = 200;
            var categories = (await this.categoryRepository.GetCategoriesAsync()).GroupBy(x => x.ExternalId).Select(x => x.FirstOrDefault()).ToList();
            var dictCategories = categories
                .ToDictionary(c => c.ExternalId, c => c.Id, StringComparer.InvariantCultureIgnoreCase);

            var accounts = (await this.accountRepository.GetAccountsAsync()).Distinct().ToList();
            var dictAccounts = accounts
                .ToDictionary(c => c.ExternalId, c => c.Id, StringComparer.InvariantCultureIgnoreCase);

            while (transactions.Any())
            {
                var package = this.GetPartOfTransactions(transactions, packageSize);
                transactions = transactions.Skip(packageSize);

                foreach (var transaction in package)
                {
                    int? categoryId = transaction.CategoryExternalId.HasValue ? dictCategories.ContainsKey(transaction.CategoryExternalId.Value.ToString()) ? dictCategories[transaction.CategoryExternalId.Value.ToString()] : (int?)null : (int?)null;
                    var accountId = dictAccounts[transaction.AccountExternalId.Value.ToString()];
                    var transactionToUpdate = await this.transactionRepository.GetTransactionAsync(transaction.ExternalId.Value.ToString());

                    if (transactionToUpdate != null)
                    {
                        if (transactionToUpdate.ModifiedOn < transaction.ModifiedOn)
                        {
                            transactionToUpdate.Edit(accountId, transaction.Type, transaction.Amount, transaction.Date, transaction.Name, transaction.Comment, this.userIdentityContext.UserId, transaction.IsDeleted, categoryId, transaction.Latitude, transaction.Longitude);
                            transactionToUpdate.SetCreatedOn(transaction.CreatedOn);
                            transactionToUpdate.SetModifiedOn(transaction.ModifiedOn);
                            transactionsToUpdate.Add(transactionToUpdate);
                            await this.DealWithTransactionToTagsAsync(transactionToUpdate.Id, transaction.Tags);
                            await this.DealWithTransactionToFilesAsync(transactionToUpdate.Id, transaction.Files);
                        }
                    }
                    else
                    {
                        var transactionToAdd = MobileDomain.Transaction.Create(accountId, transaction.Type, transaction.Amount, transaction.Date, transaction.Name, transaction.Comment, this.userIdentityContext.UserId, false, categoryId, transaction.ExternalId.ToString(), transaction.Latitude, transaction.Longitude);
                        transactionToAdd.SetCreatedOn(transaction.CreatedOn);
                        transactionToAdd.SetModifiedOn(transaction.ModifiedOn);
                        transactionsToAdd.Add(transactionToAdd);
                    }
                }

                if (transactionsToUpdate.Any())
                {
                    await this.transactionRepository.UpdateAsync(transactionsToUpdate);
                    transactionsToUpdate.Clear();
                }

                if (transactionsToAdd.Any())
                {
                    await this.transactionRepository.AddTransactionsAsync(transactionsToAdd);
                    foreach (var item in transactionsToAdd)
                    {
                        var tags = package.Where(x => x.ExternalId?.ToString() == item.ExternalId).Select(x => x.Tags).FirstOrDefault();
                        await this.DealWithTransactionToTagsAsync(item.Id, tags);
                        var f2t = package.Where(x => x.ExternalId?.ToString() == item.ExternalId).Select(x => x.Files).FirstOrDefault();
                        await this.DealWithTransactionToFilesAsync(item.Id, f2t);
                    }
                    transactionsToAdd.Clear();
                }
            }
        }

        private async Task DealWithTransactionToTagsAsync(int transactionId, List<TagSyncDTO> tagsToSync)
        {
            var tagIds = tagsToSync.Select(x => _tags[x.ExternalId.ToString()]).ToList(); 
            var tags2Transactions = await this.tagRepository.GetTagToTransactionsAsync(transactionId);
            var tags2Add = tagIds.Except(tags2Transactions.Select(t => t.TagId));
            var tags2Remove = tags2Transactions.Select(t => t.TagId).Except(tagIds);
            var tags2Transactions2Remove = tags2Transactions.Where(t => tags2Remove.Contains(t.TagId));
            //add new
            if(tags2Add.Any())
            {
                await transactionService.CreateTagsToTransaction(tags2Add, transactionId);
            }
            
            //delete removed
            if(tags2Transactions2Remove.Any())
            {
                await this.tagRepository.RemoveAsync(tags2Transactions2Remove);
            }
            
        }

        private IEnumerable<TransactionSyncDTO> GetPartOfTransactions(IEnumerable<TransactionSyncDTO> transactions, int packageSize)
        {
            return transactions.Take(packageSize);
        }

        private async Task UpdateTransfersAsync(IEnumerable<TransferSyncDTO> transfers)
        {
            if (transfers == null || !transfers.Any())
            {
                return;
            }

            foreach (var transfer in transfers)
            {
                var transferToUpdate = await this.transactionRepository.GetTransferAsync(transfer.ExternalId.Value.ToString());
                if (transferToUpdate != null)
                {
                    if(transferToUpdate.ModifiedOn < transfer.ModifiedOn)
                    {
                        transferToUpdate.SetRate(transfer.Rate);
                        transferToUpdate.Delete(transfer.IsDeleted);
                        transferToUpdate.SetModifiedOn(transfer.ModifiedOn);
                        await this.transactionRepository.UpdateTransferAsync(transferToUpdate);
                    }
                }
                else
                {
                    var toTransferId = (await this.transactionRepository.GetTransactionAsync(transfer.ToTransactionExternalId.ToString()))?.Id;
                    var fromTransferId = (await this.transactionRepository.GetTransactionAsync(transfer.FromTransactionExternalId.ToString()))?.Id;
                    if(toTransferId != null && fromTransferId != null)
                    {
                        var transferToAdd = Transfer.Create(fromTransferId.Value, toTransferId.Value, transfer.Rate, transfer.ExternalId.ToString());
                        transferToAdd.Delete(transfer.IsDeleted);
                        transferToAdd.SetModifiedOn(transfer.ModifiedOn);
                        await this.transactionRepository.AddTransferAsync(transferToAdd);
                    }
                }
            }
        }

        private async Task UpdateUsersAsync(IEnumerable<UserSyncDTO> users)
        {
            if (users == null || !users.Any())
            {
                return;
            }

            foreach (var user in users)
            {
                //TODO
            }
        }

        private async Task UpdateTagsAsync(IEnumerable<TagSyncDTO> tags)
        {
            if (tags == null || !tags.Any())
            {
                return;
            }

            var userId = (await this.userRepository.GetFirstUserAsync()).Id;
            foreach (var tag in tags)
            {
               
                var tagToUpdate = await this.tagRepository.GetAsync(tag.ExternalId.ToString());
                if (tagToUpdate != null)
                {
                    if (tagToUpdate.ModifiedOn < tag.ModifiedOn)
                    {
                        tagToUpdate.Edit(tag.Name, userId, tag.IsDeleted);
                        tagToUpdate.Delete(tag.IsDeleted);
                        tagToUpdate.SetModifiedOn(tag.ModifiedOn);
                        await this.tagRepository.UpdateAsync(tagToUpdate);
                    }

                }
                else
                {
                    var tagToAdd = Tag.Create(tag.Name, userId, tag.IsDeleted ,tag.ExternalId.ToString());
                    tagToAdd.Delete(tag.IsDeleted);
                    tagToAdd.SetModifiedOn(tag.ModifiedOn);
                    await this.tagRepository.AddAsync(tagToAdd);
                }
            }
        }

        private async Task UpdateCategoriesAsync(IEnumerable<CategorySyncDTO> categories)
        {
            if (categories == null || !categories.Any())
            {
                return;
            }

            var userId = (await this.userRepository.GetFirstUserAsync()).Id;

            foreach (var category in categories)
            {
               
                var categoryToUpdate = await this.categoryRepository.GetCategoryByNameAsync(category.Name);
                if (categoryToUpdate != null)
                {
                    if(categoryToUpdate.ModifiedOn < category.ModifiedOn)
                    {
                        categoryToUpdate.Edit(category.Name, category.Icon);
                        categoryToUpdate.Delete(category.IsDeleted);
                        categoryToUpdate.SetModifiedOn(category.ModifiedOn);
                        await this.categoryRepository.UpdateAsync(categoryToUpdate);
                        logger.Info("Category Updated:" + category.ExternalId.ToString());
                    }
                    
                }
                else
                {
                    var categoryToAdd = Category.Create(category.Name, userId, category.ExternalId.ToString(), category.Icon);
                    categoryToAdd.Delete(category.IsDeleted);
                    categoryToAdd.SetModifiedOn(category.ModifiedOn);
                    await this.categoryRepository.AddCategoryAsync(categoryToAdd);
                    logger.Info("Category Created:" + category.ExternalId.ToString());
                }
            }
        }

        private async Task UpdateAccountsAsync(IEnumerable<AccountSyncDTO> accounts)
        {
            if (accounts == null || !accounts.Any())
            {
                return;
            }

            accounts = accounts.OrderBy(a => a.ParentAccountId);

            foreach (var account in accounts)
            {
                var parentAccountId = account.ParentAccountExternalId.HasValue ? (await this.accountRepository.GetAccountAsync(account.ParentAccountExternalId.Value.ToString())).Id : (int?)null;
                var userId = this.userIdentityContext.UserId;
                var accountToUpdate = await this.accountRepository.GetAccountAsync(account.ExternalId.Value.ToString());
                if (accountToUpdate != null)
                {
                    if(accountToUpdate.ModifiedOn < account.ModifiedOn)
                    {
                        accountToUpdate.Edit(account.Name,"", account.CurrencyId, account.IsIncludedToTotal, account.Comment, account.Order, account.Type, parentAccountId, account.IsActive ,account.IsDeleted, userId, account.Icon);
                        accountToUpdate.Delete(account.IsDeleted);
                        accountToUpdate.SetModifiedOn(account.ModifiedOn);
                        await this.accountRepository.UpdateAsync(accountToUpdate);
                    }
                }
                else
                {
                    var accountToAdd = Account.Create(account.Name,"", account.CurrencyId, account.IsIncludedToTotal, account.Comment, account.Order, account.Type, parentAccountId, account.IsActive, account.IsDeleted, userId, account.ExternalId.ToString(), account.Icon);
                    accountToAdd.Delete(account.IsDeleted);
                    accountToAdd.SetModifiedOn(account.ModifiedOn);
                    await this.accountRepository.AddAccountAsync(accountToAdd);
                }
            }
        }

        private async Task UpdateExchangeRatesAsync(IEnumerable<ExchangeRateSyncDTO> rates)
        {
            if (rates == null || !rates.Any())
            {
                return;
            }

            var localRates = (await this.currencyRepository.GetExchangeRatesAsync());

            var userId = (await this.userRepository.GetFirstUserAsync()).Id;
            var currenciesDict = (await this.currencyRepository.GetCurriencesAsync())
                             .ToDictionary(x => x.Code, x => x.Id);
          
            foreach (var rate in rates)
            {

                var localRate = localRates.Where(x => x.ExternalId == rate.ExternalId.ToString()).FirstOrDefault();
                var fromCurrencyId = currenciesDict.ContainsKey(rate.FromCurrency) ? currenciesDict[rate.FromCurrency] : (int?)null;
                var toCurrencyId = currenciesDict.ContainsKey(rate.ToCurrency) ? currenciesDict[rate.ToCurrency] : (int?)null;
                if (fromCurrencyId.HasValue && toCurrencyId.HasValue)
                {
                    if (localRate == null)
                    {
                        var exchangeRate = ExchangeRate.Create(fromCurrencyId.Value, toCurrencyId.Value, rate.Rate, rate.ExternalId.ToString(), rate.IsDeleted, rate.Date);
                        await this.currencyRepository.AddExchangeRateAsync(exchangeRate);
                    }
                    else
                    {
                        localRate.Edit(fromCurrencyId.Value, toCurrencyId.Value, rate.Rate, rate.Date, rate.IsDeleted);
                        await this.currencyRepository.UpdateExchangeRateAsync(localRate);
                    }
                }
            }
        }
        private async Task UpdateFilesAsync(IEnumerable<FileSyncDTO> files)
        {
            if (files == null || !files.Any())
            {
                return;
            }

            foreach (var file in files)
            {

                var fileToUpdate = await this.fileRepository.GetAsync(file.ExternalId.ToString());
                if (fileToUpdate != null)
                {
                    if (fileToUpdate.ModifiedOn < file.ModifiedOn)
                    {
                        if (file.IsDeleted)
                        {
                            await this.fileHelper.TaskRemoveFileAsync(file.Id.ToString());
                        }
                        fileToUpdate.FileName = file.FileName;
                        fileToUpdate.MimeType = file.ContentType;
                        fileToUpdate.Delete(file.IsDeleted);
                        fileToUpdate.SetModifiedOn(file.ModifiedOn);
                        await this.fileRepository.UpdateAsync(fileToUpdate);
                    }
                }
                else
                {
                    var fileToAdd = new File
                    {
                        MimeType = file.ContentType,
                        IsDeleted = file.IsDeleted,
                        CreatedOn = file.CreatedOn,
                        ExternalId = file.ExternalId.ToString(),
                        FileName = file.FileName,
                        ModifiedOn = file.ModifiedOn,
                        
                    };
                    fileToAdd.Delete(file.IsDeleted);
                    fileToAdd.SetModifiedOn(file.ModifiedOn);
                    await this.fileRepository.AddAsync(fileToAdd);
                    if (!file.IsDeleted)
                    {
                        await fileHelper.SaveToLocalFolderAsync(file.Content, fileToAdd.ExternalId);
                    }

                }
            }
        }

        private async Task DealWithTransactionToFilesAsync(int transactionId, List<FileToTransactionSyncDTO> filesToSync)
        {
            if (filesToSync == null || !filesToSync.Any())
            {
                return;
            }

            var currentFiles2Transactions = await this.fileRepository.GetFileToTransactionsAsync(transactionId); 

            foreach (var f2t in filesToSync)
            {
                var entity = currentFiles2Transactions.Where(x => x.ExternalId == f2t.ExternalId.ToString()).FirstOrDefault();
                if (entity != null)
                {
                    if (f2t.IsDeleted)
                    {
                        entity.Delete(f2t.IsDeleted);
                    }
                    entity.SetModifiedOn(f2t.ModifiedOn);
                    await this.fileRepository.UpdateAsync(entity);
                }
                else
                {
                    var file = await this.fileRepository.GetAsync(f2t.FileId.ToString());
                    if(file!= null)
                    {
                        entity = new FileToTransaction
                        {
                            TransactionId = transactionId,
                            ExternalId = f2t.ExternalId.ToString(),
                            ModifiedOn = f2t.ModifiedOn,
                            IsDeleted = f2t.IsDeleted,
                            FileId = file.Id,
                        };
                        await this.fileRepository.AddAsync(entity);
                    }
                    
                }
            }
        }
    }
}
