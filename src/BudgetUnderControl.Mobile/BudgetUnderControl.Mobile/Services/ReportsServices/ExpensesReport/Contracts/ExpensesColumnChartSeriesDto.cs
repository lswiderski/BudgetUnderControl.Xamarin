using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.Mobile.Services
{
    public class ExpensesColumnChartSeriesDto
    {
        public List<ExpensesColumnChartItemDto> Data { get; set; }

        public string Name { get; set; }

    }
}
