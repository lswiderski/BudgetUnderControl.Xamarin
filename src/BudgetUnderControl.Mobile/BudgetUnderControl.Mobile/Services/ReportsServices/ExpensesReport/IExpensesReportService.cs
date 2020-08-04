using BudgetUnderControl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Mobile.Services
{
    public interface IExpensesReportService
    {
        Task<List<ExpensesColumnChartSeriesDto>> GetExpensesChartDataAsync(TransactionsFilter filter);
    }
}
