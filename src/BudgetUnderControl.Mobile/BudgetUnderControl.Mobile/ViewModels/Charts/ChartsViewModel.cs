using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Mobile.Services;
using BudgetUnderControl.Mobile.ViewModels;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.ViewModel
{
    public class ChartsViewModel : BaseViewModel, IChartsViewModel
    {

        TransactionsFilter filter;
        public TransactionsFilter Filter
        {
            get { return filter; }
            set { SetProperty(ref filter, value); }
        }

        private readonly ICategoryReportService categoryReportService;
        private readonly IExpensesReportService expensesReportService;

        public ChartsViewModel(ICategoryReportService categoryReportService, IExpensesReportService expensesReportService)
        {
            this.categoryReportService = categoryReportService;
            this.expensesReportService = expensesReportService;
            var now = DateTime.UtcNow;
            var fromDate = new DateTime(now.Year, now.Month, 1, 0, 0, 0);
            var toDate = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month), 23, 59, 59);

            this.Filter = new TransactionsFilter { FromDate = fromDate, ToDate = toDate };
            
        }

        

        public async Task LoadCategoryPieChartAsync()
        {
            var data = await this.categoryReportService.GetCategoryPieChartDataAsync(this.Filter);
            this.CategoryPieChartData = new ObservableCollection<CategoryPieChartItemDto>(data);
        }


        ObservableCollection<CategoryPieChartItemDto> categoryPieChartData;
        public ObservableCollection<CategoryPieChartItemDto> CategoryPieChartData
        {
            get { return categoryPieChartData; }
            set { SetProperty(ref categoryPieChartData, value); }
        }

        public async Task LoadExpensesColumnChartAsync()
        {
            var data = await this.expensesReportService.GetExpensesChartDataAsync(this.Filter);
            this.ExpensesColumnChartData = new ObservableCollection<ExpensesColumnChartSeriesDto> (data);
        }

        ObservableCollection<ExpensesColumnChartSeriesDto> expensesColumnChartData;
        public ObservableCollection<ExpensesColumnChartSeriesDto> ExpensesColumnChartData
        {
            get { return expensesColumnChartData; }
            set { SetProperty(ref expensesColumnChartData, value); }
        }
    }
}
