using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.Mobile.Services
{
    public class BalanceDto
    {
        public decimal Value { get; set; }

        public string Currency { get; set; }

        public bool IsExchanged { get; set; }
    }
}
