using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.CommonInfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Mobile.Services
{
    public class ExpensesReportService : IExpensesReportService
    {

        private readonly ITransactionService transactionService;

        public ExpensesReportService(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        public async Task<List<ExpensesColumnChartSeriesDto>> GetExpensesChartDataAsync(TransactionsFilter filter)
        {
            var transactions = await this.transactionService.GetTransactionsAsync(filter);

            var filteredTransactions = transactions.Where(x => !x.IsTransfer.Value && x.Type == Common.Enums.TransactionType.Expense).ToList();
            var dates = filteredTransactions.Select(x => x.JustDate.ToShortDateString()).Distinct().ToList();
            var _maxNumberOfColumns = 5;
            var groupByMonths = dates.Count > _maxNumberOfColumns;
            var months = filteredTransactions.Select(x => $"{x.JustDate.Month}/{x.JustDate.Year}").Distinct().ToList();

            var grouped = filteredTransactions
                .GroupBy(x => x.Category)
                .ToDictionary(x => x.Key,
                    x => groupByMonths ?
                         x.GroupBy(x => new { x.JustDate.Year, x.JustDate.Month })
                            .Select(x => new ExpensesColumnChartItemDto
                            {
                                XAxis = $"{x.Key.Month}/{x.Key.Year}",
                                YAxis = x.Select(y => Math.Abs(y.ValueInMainCurrency)).Sum()
                            })
                            :
                         x.GroupBy(x => x.JustDate)
                            .Select(x => new ExpensesColumnChartItemDto
                            {
                                XAxis = x.Key.ToShortDateString(),
                                YAxis = x.Select(y => Math.Abs(y.ValueInMainCurrency)).Sum()
                            })
                            )
                .Select(x => new ExpensesColumnChartSeriesDto { Name = x.Key, Data = x.Value.ToList() })
                .ToList();

            foreach (var group in grouped)
            {
                if (groupByMonths)
                {
                    var missingMonths = months.Except(group.Data.Select(x => x.XAxis));
                    group.Data.AddRange(missingMonths.Select(x => new ExpensesColumnChartItemDto { XAxis = x, YAxis = 0 }));
                    group.Data = group.Data.OrderBy(x => x.XAxis).ToList();
                }
                else
                {
                    var missingDates = dates.Except(group.Data.Select(x => x.XAxis));
                    group.Data.AddRange(missingDates.Select(x => new ExpensesColumnChartItemDto { XAxis = x, YAxis = 0 }));
                    group.Data = group.Data.OrderBy(x => x.XAxis).ToList();
                }
            }
            return grouped;
        }
    }
}
