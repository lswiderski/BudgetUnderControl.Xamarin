﻿using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Mobile.Services;
using BudgetUnderControl.Mobile.ViewModels;
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
        public ChartsViewModel(ICategoryReportService categoryReportService)
        {
            this.categoryReportService = categoryReportService;

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
    }
}
