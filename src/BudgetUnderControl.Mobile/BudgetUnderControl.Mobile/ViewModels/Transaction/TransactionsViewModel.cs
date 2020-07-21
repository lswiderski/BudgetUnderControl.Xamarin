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
                return string.Format("{0}-{1}", FromDate.ToString("dd.MM.yyyy"), ToDate.ToString("dd.MM.yyyy"));
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
        
        public TransactionsViewModel(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
            var now = DateTime.UtcNow;
            FromDate = new DateTime(now.Year, now.Month, 1,0,0,0);
            ToDate = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month), 23, 59, 59);
        }

        public async Task LoadTransactionsAsync()
        {
            var transactions = await transactionService.GetTransactionsAsync(new TransactionsFilter { FromDate = FromDate, ToDate= ToDate, SearchQuery = Search} );

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

            SetIncomeExpense();
        }

        public async Task SetNextMonth()
        {
            FromDate = new DateTime(FromDate.Year, FromDate.Month, 1, FromDate.Hour, FromDate.Minute, FromDate.Second).AddMonths(1);
            ToDate = new DateTime(FromDate.Year, FromDate.Month, DateTime.DaysInMonth(FromDate.Year, FromDate.Month), ToDate.Hour, ToDate.Minute, ToDate.Second);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ActualRange)));
            await LoadTransactionsAsync();
        }

        public async Task SetPreviousMonth()
        {
            FromDate = new DateTime(FromDate.Year, FromDate.Month, 1, FromDate.Hour, FromDate.Minute, FromDate.Second).AddMonths(-1);
            ToDate = new DateTime(FromDate.Year, FromDate.Month, DateTime.DaysInMonth(FromDate.Year, FromDate.Month), ToDate.Hour, ToDate.Minute, ToDate.Second);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ActualRange)));
            await LoadTransactionsAsync();
        }

        private void SetIncomeExpense()
        {
            decimal _income = 0m;
            decimal _expense = 0m;
            int noTransactions = 0;

            var trans = Transactions.SelectMany(x => x.Select(y => y)).ToList();
            foreach (var item in trans)
            {
                if(item.IsTransfer == false)
                {
                    decimal _value = 0;

                    switch (item.CurrencyCode)
                    {
                        case "PLN":
                            _value = item.Value * 1m;
                            break;
                        case "USD":
                            _value = item.Value * 3.66m;
                            break;
                        case "EUR":
                            _value = item.Value * 4.26m;
                            break;
                        case "THB":
                            _value = item.Value * 0.11m;
                            break;
                        default:
                            break;
                    }

                    if (item.Value > 0)
                    {
                        _income += _value;
                    }
                    else
                    {
                        _expense += _value;
                    }
                }
               
                noTransactions++;
            }
            NumberOfTransactions = noTransactions.ToString();
            Expense = _expense.ToString();
            Income = _income.ToString();
        }

    }
}
