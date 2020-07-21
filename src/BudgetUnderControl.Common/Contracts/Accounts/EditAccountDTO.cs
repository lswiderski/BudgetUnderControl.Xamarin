using BudgetUnderControl.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Common.Contracts
{
    public class EditAccountDTO
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; }
        public int CurrencyId { get; set; }
        public string Currency { get; set; }
        public string CurrencySymbol { get; set; }
        public int AccountGroupId { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public decimal Amount { get; set; }
        public bool IsIncludedInTotal { get; set; }
        public bool IsActive { get; set; }
        public string Comment { get; set; }
        public string Icon { get; set; }

        public AccountType Type { get; set; }
        public int? ParentAccountId { get; set; }
        public int Order { get; set; }

        public string AmountWithCurrency
        {
            get
            {
                return string.Format("{0}{1}", Amount, CurrencySymbol);
            }
        }
    }
}
