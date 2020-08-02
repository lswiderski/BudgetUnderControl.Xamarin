using BudgetUnderControl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Mobile.Services
{
    public interface IBalanceService
    {
        Task<BalanceResultDto> GetBalanceAsync(ICollection<TransactionListItemDTO> transactions);

        Task<BalanceResultDto> GetBalanceAsync(ICollection<TransactionListItemDTO> transactions, string mainCurrency);

        Task<BalanceResultDto> GetBalanceAsync(TransactionsFilter filter, string mainCurrency);

        Task<BalanceResultDto> GetTotalCurrentBalanceAsync();
    }
}
