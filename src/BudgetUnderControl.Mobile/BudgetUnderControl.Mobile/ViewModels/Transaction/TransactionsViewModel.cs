using BudgetUnderControl.Common;
using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.CommonInfrastructure.Commands;
using BudgetUnderControl.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetUnderControl.CommonInfrastructure;
using Microsoft.Extensions.Caching.Memory;
using BudgetUnderControl.Mobile.Keys;

namespace BudgetUnderControl.ViewModel
{
    public class TransactionsViewModel : ITransactionsViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        ITransactionService transactionService;
        public TransactionListItemDTO SelectedTransaction { get; set; }

        ObservableCollection<ObservableGroupCollection<string, TransactionListItemDTO>> transactions;
        public ObservableCollection<ObservableGroupCollection<string, TransactionListItemDTO>> Transactions
        {
            get
            {
                return transactions;
            }
            set
            {
                if (transactions != value)
                {
                    transactions = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Transactions)));
                }

            }
        }

        public TransactionsFilter Filter { get; set; }

        private DateTime fromDate;
        public DateTime FromDate
    {
            get => fromDate;
            set
            {
                if (fromDate != value)
                {
                    fromDate = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FromDate)));
                }
            }
        }

        private DateTime toDate;
        public DateTime ToDate
        {
            get => toDate;
            set
            {
                if (toDate != value)
                {
                    toDate = new DateTime(value.Year, value.Month, DateTime.DaysInMonth(value.Year, value.Month), 23, 59, 59);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ToDate)));
                }
            }
        }

        public string Search { get; set; }

        public string ActualRange
        {
            get
            {
                return string.Format("{0}-{1}", Filter.FromDate?.ToString("dd.MM.yyyy"), Filter.ToDate?.ToString("dd.MM.yyyy"));
            }
        }

        private string income;
        public string Income
        {
            get => income;
            set
            {
                if (income != value)
                {
                    income = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Income)));
                }
            }
        }

        private string expense;
        public string Expense
        {
            get => expense;
            set
            {
                if (expense != value)
                {
                    expense = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Expense)));
                }
            }
        }

        private string otherIncome;
        public string OtherIncome
        {
            get => otherIncome;
            set
            {
                if (otherIncome != value)
                {
                    otherIncome = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OtherIncome)));
                }
            }
        }

        private string otherExpense;
        public string OtherExpense
        {
            get => otherExpense;
            set
            {
                if (otherExpense != value)
                {
                    otherExpense = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OtherExpense)));
                }
            }
        }

        private string numberOfTransactions;
        public string NumberOfTransactions
        {
            get => numberOfTransactions;
            set
            {
                if (numberOfTransactions != value)
                {
                    numberOfTransactions = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NumberOfTransactions)));
                }
            }
        }

        private readonly IBalanceService balanceService;
        private readonly IMemoryCache memoryCache;
        public TransactionsViewModel(ITransactionService transactionService, IBalanceService balanceService, IMemoryCache memoryCache)
        {
            this.transactionService = transactionService;
            this.balanceService = balanceService;
            this.memoryCache = memoryCache;

            var now = DateTime.UtcNow;
            FromDate = new DateTime(now.Year, now.Month, 1,0,0,0);
            ToDate = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month), 23, 59, 59);
            TransactionsFilter _filter;
            if (memoryCache.TryGetValue(CacheKeys.Filters, out _filter))
            {
                this.Filter = _filter;
            }
            else
            {
                this.Filter = new TransactionsFilter { FromDate = fromDate, ToDate = toDate, SearchQuery = Search };
            }
        }

        public async Task LoadTransactionsAsync()
        {
            memoryCache.Set(CacheKeys.Filters, this.Filter, DateTimeOffset.Now.AddMinutes(5));
            var transactions = await transactionService.GetTransactionsAsync(Filter);

            var dtos = transactions.Select(t => new TransactionListItemDTO
            {
                AccountId = t.AccountId,
                Date = t.Date,
                Id = t.Id,
                Value = t.Value,
                Account = t.Account,
                ValueWithCurrency = t.ValueWithCurrency,
                Type = t.Type,
                Name = t.Name,
                CurrencyCode = t.CurrencyCode,
                IsTransfer = t.IsTransfer,
                ExternalId = t.ExternalId,
                ModifiedOn = t.ModifiedOn,
                CreatedOn = t.CreatedOn,
                Category = t.Category,
                CategoryId = t.CategoryId,
                CategoryIcon = t.CategoryIcon,
                AccountIcon = t.AccountIcon,
                Tags = t.Tags
            }).OrderByDescending(x => x.Date)
                                .GroupBy(x => x.Date.ToString("d MMM yyyy"))
                                .Select(x => new ObservableGroupCollection<string, TransactionListItemDTO>(x))
                                .ToList();

            Transactions = new ObservableCollection<ObservableGroupCollection<string, TransactionListItemDTO>>(dtos);

            await SetIncomeExpense();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ActualRange)));
        }

        public async Task SetNextMonth()
        {
            FromDate = new DateTime(FromDate.Year, FromDate.Month, 1, FromDate.Hour, FromDate.Minute, FromDate.Second).AddMonths(1);
            ToDate = new DateTime(FromDate.Year, FromDate.Month, DateTime.DaysInMonth(FromDate.Year, FromDate.Month), ToDate.Hour, ToDate.Minute, ToDate.Second);
            this.Filter.FromDate = FromDate;
            this.Filter.ToDate = ToDate;
            await LoadTransactionsAsync();
        }

        public async Task SetPreviousMonth()
        {
            FromDate = new DateTime(FromDate.Year, FromDate.Month, 1, FromDate.Hour, FromDate.Minute, FromDate.Second).AddMonths(-1);
            ToDate = new DateTime(FromDate.Year, FromDate.Month, DateTime.DaysInMonth(FromDate.Year, FromDate.Month), ToDate.Hour, ToDate.Minute, ToDate.Second);
            this.Filter.FromDate = FromDate;
            this.Filter.ToDate = ToDate;
            await LoadTransactionsAsync();
        }

        private async Task SetIncomeExpense()
        {
            var trans = Transactions.SelectMany(x => x.Select(y => y)).ToList();

            if(Filter.AccountsIds != null && Filter.AccountsIds.Any())
            {
                NumberOfTransactions = trans.Count().ToString();
            }
            else
            {
                NumberOfTransactions = trans.Where(x => x.IsTransfer == false).Count().ToString();
            }
           

            var balance = await this.balanceService.GetBalanceAsync(trans);

            Expense = balance.Expenses.Where(x => x.IsExchanged).FirstOrDefault().Value.ToString();
            Income = balance.Incomes.Where(x => x.IsExchanged).FirstOrDefault().Value.ToString();

            if (balance.Expenses.Count > 2)
            {
                OtherExpense = string.Join(" ",balance.Expenses.Where(x => !x.IsExchanged).Select(x => $"{x.Value} {x.Currency}").ToList());
            }
            else
            {
                OtherExpense = string.Empty;
            }

            if (balance.Incomes.Count > 2)
            {
                OtherIncome = string.Join(" ", balance.Incomes.Where(x => !x.IsExchanged).Select(x => $"{x.Value} {x.Currency}").ToList());
            }
            else
            {
                OtherIncome = string.Empty;

            }
        }

    }
}
