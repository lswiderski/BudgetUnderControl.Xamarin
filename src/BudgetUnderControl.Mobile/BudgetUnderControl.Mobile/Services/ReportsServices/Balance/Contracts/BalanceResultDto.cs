using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.Mobile.Services
{
    public class BalanceResultDto
    {
        public List<BalanceDto> Incomes { get; set; }

        public List<BalanceDto> Expenses { get; set; }

        public List<BalanceDto> Total { get; set; }
    }
}
