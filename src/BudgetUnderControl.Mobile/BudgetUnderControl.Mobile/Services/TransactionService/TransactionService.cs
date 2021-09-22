using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetUnderControl.Common;
using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.MobileDomain;
using BudgetUnderControl.MobileDomain.Repositiories;
using BudgetUnderControl.CommonInfrastructure.Commands;
using BudgetUnderControl.Common.Extensions;
using BudgetUnderControl.CommonInfrastructure;
using FluentValidation;
using Xamarin.Essentials;

namespace BudgetUnderControl.Mobile.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly ITagRepository tagRepository;
        private readonly IFileRepository fileRepository;
        private readonly IUserRepository userRepository;
        private readonly IIconService iconService;
        private readonly ICurrencyService currencyService;
        private readonly ICurrencyRepository currencyRepository;
        public TransactionService(ITransactionRepository transactionRepository,
            IUserRepository userRepository,
            ITagRepository tagRepository,
            IFileRepository fileRepository,
            IIconService iconService,
            ICurrencyService currencyService,
            ICurrencyRepository currencyRepository)
        {
            this.transactionRepository = transactionRepository;
            this.userRepository = userRepository;
            this.tagRepository = tagRepository;
            this.fileRepository = fileRepository;
            this.iconService = iconService;
            this.currencyService = currencyService;
            this.currencyRepository = currencyRepository;
        }

        public async Task<ICollection<TransactionListItemDTO>> GetTransactionsAsync(TransactionsFilter filter = null)
        {
            var exchangeRates = (await this.currencyRepository.GetExchangeRatesAsync())
               .Select(x => new ExchangeRateDTO
               {
                   ToCurrencyCode = x.ToCurrency.Code,
                   FromCurrencyCode = x.FromCurrency.Code,
                   Date = x.Date,
                   Rate = x.Rate
               }).ToList();

            var mainCurrency = "PLN";

            var transactions = await this.transactionRepository.GetTransactionsAsync(filter);
            var dtos = transactions.Select(async t => new TransactionListItemDTO
            {
                AccountId = t.AccountId,
                Date = t.Date,
                Id = t.Id,
                Value = t.Amount,
                Account = t.Account.Name,
                ValueWithCurrency = t.Amount + t.Account.Currency.Symbol,
                ValueInMainCurrency = await this.currencyService.GetValueInCurrencyAsync(exchangeRates, t.Account.Currency.Code, mainCurrency, t.Amount, t.Date),
                Type = t.Type,
                Name = t.Name,
                CurrencyCode = t.Account.Currency.Code,
                IsTransfer = t.IsTransfer,
                ExternalId = Guid.Parse(t.ExternalId),
                ModifiedOn = t.ModifiedOn,
                CreatedOn = t.CreatedOn,
                CategoryId = t.CategoryId,
                Category = t.Category?.Name,
                CategoryIcon = iconService.GetIcon(t.Category?.Icon),
                AccountIcon = iconService.GetIcon(t.Account?.Icon),
                Tags = t.TagsToTransaction.Where(x => !x.Tag.IsDeleted).Select(x => new TagDTO { ExternalId = Guid.Parse(x.Tag.ExternalId), Id = x.Tag.Id, IsDeleted = x.Tag.IsDeleted, Name = x.Tag.Name }).ToList()
            })
                .Select(t => t.Result)
                .OrderByDescending(t => t.Date)
            .ToList();

            return dtos;
        }

        public async Task AddTransactionAsync(AddTransaction command)
        {
            var user = await userRepository.GetFirstUserAsync();

            if (command.Type == ExtendedTransactionType.Transfer)
            {
                if (command.Amount > 0)
                {
                    command.Amount *= (-1);
                }
                if (command.TransferAmount < 0)
                {
                    command.TransferAmount *= (-1);
                }
                var transactionExpense = Transaction.Create(command.AccountId, TransactionType.Expense, command.Amount, command.Date, command.Name, command.Comment, user.Id, false, command.CategoryId, command.ExternalId.ToString(), command.Latitude, command.Longitude);
                await transactionRepository.AddTransactionAsync(transactionExpense);

                var transactionIncome = Transaction.Create(command.TransferAccountId.Value, TransactionType.Income, command.TransferAmount, command.TransferDate, command.Name, command.Comment, user.Id, false, command.CategoryId, command.TransferExternalId.ToString(), command.Latitude, command.Longitude);
                await transactionRepository.AddTransactionAsync(transactionIncome);

                var transfer = Transfer.Create(transactionExpense.Id, transactionIncome.Id, command.Rate);
                await transactionRepository.AddTransferAsync(transfer);
                await this.CreateTagsToTransaction(command.Tags, transactionExpense.Id);
                await this.MergeFiles(command.FileGuid, transactionExpense);
            }
            else
            {
                var type = command.Type.ToTransactionType();
                if (type == TransactionType.Expense && command.Amount > 0)
                {
                    command.Amount *= (-1);
                }
                else if (type == TransactionType.Income && command.Amount < 0)
                {
                    command.Amount *= (-1);
                }
                var transaction = Transaction.Create(command.AccountId, type, command.Amount, command.Date, command.Name, command.Comment, user.Id, false, command.CategoryId, command.ExternalId.ToString(), command.Latitude, command.Longitude);
                await this.transactionRepository.AddTransactionAsync(transaction);
                await this.CreateTagsToTransaction(command.Tags, transaction.Id);
                await this.MergeFiles(command.FileGuid, transaction);
            }
            SecureStorage.Remove("Balance_Total");
        }

        public async Task CreateTagsToTransaction(IEnumerable<int> tagsId, int transactionId)
        {
            if (tagsId != null && tagsId.Any())
            {
                var tags2Transactions = tagsId.Select(x => new TagToTransaction
                {
                    TagId = x,
                    TransactionId = transactionId,
                });
                await this.tagRepository.AddAsync(tags2Transactions);
            }
        }
        private async Task MergeFiles(string fileGuid, Transaction firstTransaction)
        {
            var now = DateTime.UtcNow;
            var file = await this.fileRepository.GetAsync(fileGuid);
            var currentFile = firstTransaction.FilesToTransaction?.Where(x => !x.IsDeleted).Select(x => x.File).FirstOrDefault();

            if (file != null && currentFile != null)
            {
                if (file.ExternalId != currentFile.ExternalId)
                {
                    await this.fileRepository.RemoveAsync(currentFile);
                    await this.fileRepository.AddAsync(new FileToTransaction
                    {
                        FileId = file.Id,
                        TransactionId = firstTransaction.Id,
                        ModifiedOn = now,
                        IsDeleted = false,
                        ExternalId = Guid.NewGuid().ToString()
                    });

                    //remove local physical file
                }
            }
            else if (file != null && currentFile == null)
            {
                //assign to transaction
                await this.fileRepository.AddAsync(new FileToTransaction
                {
                    FileId = file.Id,
                    TransactionId = firstTransaction.Id,
                    ModifiedOn = now,
                    IsDeleted = false,
                    ExternalId = Guid.NewGuid().ToString()
                });
            }
            else if (file == null && currentFile != null)
            {
                //remove old
                await this.fileRepository.RemoveAsync(currentFile);
                //remove local physical file

                if (!string.IsNullOrWhiteSpace(fileGuid))
                {
                    //create new

                    file = new File
                    {
                        CreatedOn = now,
                        ModifiedOn = now,
                        IsDeleted = false,
                        ExternalId = fileGuid,
                        FileName = fileGuid,
                        MimeType = "image/jpeg",
                    };
                    await this.fileRepository.AddAsync(file);

                    await this.fileRepository.AddAsync(new FileToTransaction
                    {
                        FileId = file.Id,
                        TransactionId = firstTransaction.Id,
                        ModifiedOn = now,
                        IsDeleted = false,
                        ExternalId = Guid.NewGuid().ToString()
                    });
                }
            }
            else // both null
            {
                if (!string.IsNullOrWhiteSpace(fileGuid))
                {
                    //create new
                    file = new File
                    {
                        CreatedOn = now,
                        ModifiedOn = now,
                        IsDeleted = false,
                        ExternalId = fileGuid,
                        FileName = fileGuid,
                        MimeType = "image/jpeg",
                    };
                    await this.fileRepository.AddAsync(file);

                    await this.fileRepository.AddAsync(new FileToTransaction
                    {
                        FileId = file.Id,
                        TransactionId = firstTransaction.Id,
                        ModifiedOn = now,
                        IsDeleted = false,
                        ExternalId = Guid.NewGuid().ToString()
                    });
                }
            }
        }

        public async Task EditTransactionAsync(EditTransaction command)
        {
            var user = await userRepository.GetFirstUserAsync();
            var firstTransaction = await this.transactionRepository.GetTransactionAsync(command.Id);
            Transaction secondTransaction = null;

            await transactionRepository.UpdateAsync(firstTransaction);

            //merge tags
            var tags2Transactions = await this.tagRepository.GetTagToTransactionsAsync(command.Id);
            var tags2Add = command.Tags.Except(tags2Transactions.Select(t => t.TagId));
            var tags2Remove = tags2Transactions.Select(t => t.TagId).Except(command.Tags);
            var tags2Transactions2Remove = tags2Transactions.Where(t => tags2Remove.Contains(t.TagId));
            //add new
            await this.CreateTagsToTransaction(tags2Add, firstTransaction.Id);
            //delete removed
            await this.tagRepository.RemoveAsync(tags2Transactions2Remove);

            //merge files
            await this.MergeFiles(command.FileGuid, firstTransaction);


            var transfer = await transactionRepository.GetTransferAsync(command.Id);

            if (transfer != null)
            {
                var secondTransactionId = transfer.ToTransactionId != command.Id ? transfer.ToTransactionId : transfer.FromTransactionId;
                secondTransaction = await this.transactionRepository.GetTransactionAsync(transfer.ToTransactionId);
            }

            //remove transfer, no more transfer
            if (command.ExtendedType != ExtendedTransactionType.Transfer
                && transfer != null && secondTransaction != null)
            {
                await this.transactionRepository.RemoveTransferAsync(transfer);
                await this.transactionRepository.RemoveTransactionAsync(secondTransaction);
                firstTransaction.Edit(command.AccountId, command.ExtendedType.ToTransactionType(), command.Amount, command.Date, command.Name, command.Comment, user.Id, command.IsDeleted, command.CategoryId, command.Latitude, command.Longitude);
                await this.transactionRepository.UpdateAsync(firstTransaction);
            }
            //new Transfer, no transfer before
            else if (command.ExtendedType == ExtendedTransactionType.Transfer
                && transfer == null && secondTransaction == null)
            {
                firstTransaction.Edit(command.AccountId, TransactionType.Expense, command.Amount, command.Date, command.Name, command.Comment, user.Id, command.IsDeleted, command.CategoryId, command.Latitude, command.Longitude);
                await this.transactionRepository.UpdateAsync(firstTransaction);

                var transactionIncome = Transaction.Create(command.TransferAccountId.Value, TransactionType.Income, command.TransferAmount.Value, command.TransferDate.Value, command.Name, command.Comment, user.Id, command.IsDeleted, command.CategoryId, null, command.Latitude, command.Longitude);
                await transactionRepository.AddTransactionAsync(transactionIncome);

                var newTransfer = Transfer.Create(firstTransaction.Id, transactionIncome.Id, command.Rate.Value);
                await transactionRepository.AddTransferAsync(newTransfer);

            }

            //edit transfer
            else if (command.ExtendedType == ExtendedTransactionType.Transfer
                && transfer != null && secondTransaction != null)
            {
                firstTransaction.Edit(command.AccountId, TransactionType.Expense, command.Amount, command.Date, command.Name, command.Comment, user.Id, command.IsDeleted, command.CategoryId, command.Latitude, command.Longitude);
                await this.transactionRepository.UpdateAsync(firstTransaction);

                secondTransaction.Edit(command.TransferAccountId.Value, TransactionType.Income, command.TransferAmount.Value, command.TransferDate.Value, command.Name, command.Comment, user.Id, command.IsDeleted, command.CategoryId, command.Latitude, command.Longitude);
                await this.transactionRepository.UpdateAsync(firstTransaction);

                transfer.SetRate(command.Rate.Value);
                await transactionRepository.UpdateTransferAsync(transfer);
            }
            //just edit 1 transaction, no transfer before
            else if (command.ExtendedType != ExtendedTransactionType.Transfer
                && transfer == null && secondTransaction == null)
            {
                decimal amount = 0;

                if (command.ExtendedType == ExtendedTransactionType.Expense && command.Amount > 0)
                {
                    amount = command.Amount * (-1);
                }
                else if (command.ExtendedType == ExtendedTransactionType.Income && command.Amount < 0)
                {
                    amount = command.Amount * (-1);
                }
                else
                {
                    amount = command.Amount;
                }

                firstTransaction.Edit(command.AccountId, command.ExtendedType.ToTransactionType(), amount, command.Date, command.Name, command.Comment, user.Id, command.IsDeleted, command.CategoryId, command.Latitude, command.Longitude);
                await this.transactionRepository.UpdateAsync(firstTransaction);
            }
            SecureStorage.Remove("Balance_Total");
        }

        public async Task DeleteTransactionAsync(DeleteTransaction command)
        {
            Transaction firstTransaction = null;
            if (command.Id != null)
            {
                firstTransaction = await this.transactionRepository.GetTransactionAsync(command.Id.Value);
            }
            else if (command.ExternalId != null)
            {
                firstTransaction = await this.transactionRepository.GetTransactionAsync(command.ExternalId.Value.ToString());
            }
            else
            {
                throw new ArgumentException("No Transaction to remove");
            }
            await this.fileRepository.RemoveAsync(firstTransaction.FilesToTransaction);
            Transaction secondTransaction = null;
            var transfer = await this.transactionRepository.GetTransferAsync(firstTransaction.Id);
            var tags2Transaction = await this.tagRepository.GetTagToTransactionsAsync(firstTransaction.Id);
            await this.tagRepository.RemoveAsync(tags2Transaction);
            if (transfer != null)
            {
                var secondTRansactionId = transfer.ToTransactionId != firstTransaction.Id ? transfer.ToTransactionId : transfer.FromTransactionId;
                secondTransaction = await this.transactionRepository.GetTransactionAsync(secondTRansactionId);
                tags2Transaction = await this.tagRepository.GetTagToTransactionsAsync(secondTRansactionId);
                await this.tagRepository.RemoveAsync(tags2Transaction);
                await transactionRepository.RemoveTransferAsync(transfer);
                await transactionRepository.RemoveTransactionAsync(secondTransaction);
            }
            await transactionRepository.RemoveTransactionAsync(firstTransaction);
        }

        public async Task<EditTransactionDTO> GetTransactionAsync(Guid id)
        {
            var transaction = await this.transactionRepository.GetTransactionAsync(id.ToString());
            return await this.EntityToEditQueryAsync(transaction);
        }

        public async Task<EditTransactionDTO> GetTransactionAsync(int id)
        {
            var transaction = await this.transactionRepository.GetTransactionAsync(id);
            return await this.EntityToEditQueryAsync(transaction);
        }

        private async Task<EditTransactionDTO> EntityToEditQueryAsync(Transaction entity)
        {
            var transaction = new EditTransactionDTO
            {
                AccountId = entity.AccountId,
                Amount = entity.Amount,
                CategoryId = entity.CategoryId,
                Comment = entity.Comment,
                Date = entity.Date,
                Id = entity.Id,
                Name = entity.Name,
                Type = entity.Type,
                ExternalId = Guid.Parse(entity.ExternalId),
                ModifiedOn = entity.ModifiedOn,
                CreatedOn = entity.CreatedOn,
                IsDeleted = entity.IsDeleted,
                Longitude = entity.Longitude,
                Latitude = entity.Latitude,
                FileGuid = entity.FilesToTransaction.Where(x => !x.IsDeleted).Select(x => x.File.ExternalId).FirstOrDefault()
            };

            transaction.ExtendedType = transaction.Type == TransactionType.Income ? ExtendedTransactionType.Income : ExtendedTransactionType.Expense;

            var tags2Transactions = await this.tagRepository.GetTagToTransactionsAsync(transaction.Id);
            var tags = await this.tagRepository.GetAsync(tags2Transactions.Select(t => t.TagId).ToList());
            transaction.Tags = tags.Select(t => new TagDTO
            {
                Id = t.Id,
                Name = t.Name,
                IsDeleted = t.IsDeleted,
                ExternalId = Guid.Parse(t.ExternalId)
            }).ToList();

            var transfer = await this.transactionRepository.GetTransferAsync(transaction.Id);

            if (transfer != null)
            {
                int transferedTransactionId;
                if (transaction.Id == transfer.FromTransactionId)
                {
                    transferedTransactionId = transfer.ToTransactionId;
                }
                else
                {
                    transferedTransactionId = transfer.FromTransactionId;
                }
                var transferedTransaction = await this.transactionRepository.GetTransactionAsync(transferedTransactionId);

                transaction.Rate = transfer.Rate;
                transaction.TransferId = transfer.Id;

                if (transaction.Id == transfer.FromTransactionId)
                {
                    transaction.TransferAmount = transferedTransaction.Amount;
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

    }
}
