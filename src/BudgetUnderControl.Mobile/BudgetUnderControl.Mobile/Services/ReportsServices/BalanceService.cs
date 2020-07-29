using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.CommonInfrastructure;
using BudgetUnderControl.MobileDomain;
using BudgetUnderControl.MobileDomain.Repositiories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;
using static MoreLinq.Extensions.MinByExtension;

namespace BudgetUnderControl.Mobile.Services
{
    public class BalanceService : IBalanceService
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly ICurrencyRepository currencyRepository;
        private readonly IAccountService accountService;
        private readonly ITransactionService transactionService;

        public BalanceService(ITransactionRepository transactionRepository, ICurrencyRepository currencyRepository,
            IAccountService accountService, ITransactionService transactionService)
        {
            this.transactionRepository = transactionRepository;
            this.currencyRepository = currencyRepository;
            this.accountService = accountService;
            this.transactionService = transactionService;
        }

        public async Task<BalanceResultDto> GetBalanceAsync(ICollection<TransactionListItemDTO> transactions)
        {
            return await this.GetBalanceAsync(transactions, "PLN");
        }

        public async Task<BalanceResultDto> GetBalanceAsync(ICollection<TransactionListItemDTO> transactions, string mainCurrency)
        {
            var result = new BalanceResultDto();
            List<ExchangeRate> exchangeRates = (await this.currencyRepository.GetExchangeRatesAsync()).ToList();

            result.Expenses = this.GetExpenseOrIncome(transactions, false).Select(x => new BalanceDto { Currency = x.Key, Value = x.Value, IsExchanged = false }).ToList();
            result.Incomes = this.GetExpenseOrIncome(transactions, false, true).Select(x => new BalanceDto { Currency = x.Key, Value = x.Value, IsExchanged = false }).ToList();

            var totalExpenses = this.GetTotalExpenseOrIncome(transactions, mainCurrency, exchangeRates, false);
            var totalIncomes = this.GetTotalExpenseOrIncome(transactions, mainCurrency, exchangeRates, false, true);

            result.Expenses.Add(new BalanceDto { Currency = mainCurrency, Value = totalExpenses, IsExchanged = true });
            result.Incomes.Add(new BalanceDto { Currency = mainCurrency, Value = totalIncomes, IsExchanged = true });
            return result;
        }

        public async Task<BalanceResultDto> GetBalanceAsync(TransactionsFilter filter, string mainCurrency)
        {
            var transactions = await this.transactionService.GetTransactionsAsync(filter);

            var result = await this.GetBalanceAsync(transactions, mainCurrency);
            return result;
        }

        public async Task<BalanceResultDto> GetTotalCurrentBalanceAsync()
        {
            return await this.GetTotalCurrentBalanceAsync("PLN");
        }

        public async Task<BalanceResultDto> GetTotalCurrentBalanceAsync(string mainCurrency)
        {
            var accounts = await accountService.GetAccountsWithBalanceAsync();
            List<ExchangeRate> exchangeRates = (await this.currencyRepository.GetExchangeRatesAsync()).ToList();

            var currentStatus = this.CalculateActualStatus(accounts);
            var total = this.CalculateTotalSum(currentStatus, exchangeRates, mainCurrency);

            var result = new BalanceResultDto
            {
                Total = currentStatus
            };

            result.Total.Add(new BalanceDto { Currency = mainCurrency, IsExchanged = true, Value = total });

            return result;
        }

        private Dictionary<string, decimal> GetExpenseOrIncome(ICollection<TransactionListItemDTO> transactions, bool includeTransfers, bool isIncome = false)
        {
            var selectedTransactions = transactions
                .Where(x => !x.IsTransfer.Value);
            Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
            selectedTransactions.ForEach(x =>
            {
                if (isIncome ? x.Value > 0 : x.Value < 0)
                {
                    if (!dict.ContainsKey(x.CurrencyCode))
                    {
                        dict.Add(x.CurrencyCode, x.Value);
                    }
                    else
                    {
                        dict[x.CurrencyCode] += x.Value;
                    }
                    
                }

            });

            return dict;
        }

        private decimal GetTotalExpenseOrIncome(ICollection<TransactionListItemDTO> transactions, string currencyCode, List<ExchangeRate> exchangeRates, bool includeTransfers, bool isIncome = false)
        {
            var selectedTransactions = transactions
                .Where(x => !x.IsTransfer.Value);

            decimal sum = 0;
            selectedTransactions.ForEach(x =>
            {
                if (isIncome ? x.Value > 0 : x.Value < 0)
                {
                    sum += this.GetValueInCurrency(exchangeRates, x.CurrencyCode, currencyCode, x.Value, x.JustDate);
                }

            });

            return sum;
        }

        private List<BalanceDto> CalculateActualStatus(ICollection<AccountListItemDTO> accounts)
        {
            Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
            foreach (var account in accounts)
            {
                if (!account.ParentAccountId.HasValue)
                {
                    if (!dict.ContainsKey(account.Currency))
                    {
                        dict.Add(account.Currency, account.Balance);
                    }
                    else
                    {
                        dict[account.Currency] += account.Balance;
                    }
                }

            }

            return dict.Select(x => new BalanceDto { Value = x.Value, Currency = x.Key }).ToList();
        }

        private decimal CalculateTotalSum(List<BalanceDto> currentStatus, List<ExchangeRate> exchangeRates, string userMainCurrency)
        {
            decimal sum = 0;

            foreach (var currency in currentStatus)
            {
                sum += this.GetValueInCurrency(exchangeRates, currency.Currency, userMainCurrency, currency.Value, DateTime.Now);
            }
            return sum;
        }

        private decimal GetValueInCurrency(IList<ExchangeRate> rates, string currentCurrency, string targetCurrency, decimal value, DateTime date)
        {
            if (currentCurrency == targetCurrency)
            {
                return value;
            }

            var exchangeRate = rates.Where(x => x.ToCurrency.Code == currentCurrency || x.FromCurrency.Code == currentCurrency)
                                   .Where(x => x.ToCurrency.Code == targetCurrency || x.FromCurrency.Code == targetCurrency)
                                   .MinBy(x => Math.Abs((x.Date - date).Ticks))
                                    .FirstOrDefault();
            decimal result = 0;

            if (exchangeRate == null)
            {
                result = 0;
            }
            else if (exchangeRate.FromCurrency.Code == currentCurrency)
            {
                result = value * (decimal)exchangeRate.Rate;
            }
            else if (exchangeRate.ToCurrency.Code == currentCurrency)
            {
                result = value / ((decimal)exchangeRate.Rate != 0 ? (decimal)exchangeRate.Rate : 1);
            }

            return result;
        }

        
    }
}
