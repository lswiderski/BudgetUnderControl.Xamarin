using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.ViewModel
{
    public interface IChartsViewModel
    {
        TransactionsFilter Filter { get; set; }

        public ObservableCollection<CategoryPieChartItemDto> CategoryPieChartData { get; set; }
        Task LoadCategoryPieChartAsync();

        ObservableCollection<ExpensesColumnChartSeriesDto> ExpensesColumnChartData { get; set; }
        Task LoadExpensesColumnChartAsync();
    }
}
