using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.CommonInfrastructure;
using BudgetUnderControl.MobileDomain.Repositiories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Mobile.Services
{
    public class CategoryReportService : ICategoryReportService
    {

        private readonly ITransactionService transactionService;

        public CategoryReportService(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        public async Task<List<CategoryPieChartItemDto>> GetCategoryPieChartDataAsync(TransactionsFilter filter)
        {
            var transactions = await this.transactionService.GetTransactionsAsync(filter);

            var grouped = transactions
                .GroupBy(x => x.Category)
                .ToDictionary(x => x.Key, x => x.Select(x => x.ValueInMainCurrency).Sum())
                .Select(x => new CategoryPieChartItemDto { Value = x.Value, CategoryName = x.Key })
                .ToList();

            return grouped;
        }
    }
}
