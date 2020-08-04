using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.CommonInfrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.CommonInfrastructure
{
    public interface ICurrencyService
    {
        Task<ICollection<CurrencyDTO>> GetCurriencesAsync();
        Task<CurrencyDTO> GetCurrencyAsync(int id);
        Task<CurrencyDTO> GetCurrencyAsync(string code);
        Task<bool> IsValidAsync(int currencyId);

        Task<IEnumerable<ExchangeRateDTO>> GetExchangeRatesAsync();
        Task AddExchangeRateAsync(AddExchangeRate command);
        Task<decimal> TransformAmountAsync(decimal amount, int fromCurrencyId, int toCurrencyId);
        Task<decimal> TransformAmountAsync(decimal amount, string fromCurrencyCode, string toCurrencyCode);

        Task<decimal> GetValueInCurrencyAsync(IList<ExchangeRateDTO> rates, string currentCurrency, string targetCurrency, decimal value, DateTime date);
    }
}
