using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Common.Contracts
{
    public class AccountListItemDTO
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; }
        public int CurrencyId { get; set; }
        public string Currency { get; set; }
        public string CurrencySymbol { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public bool IsIncludedInTotal { get; set; }
        public int? ParentAccountId { get; set; }
        public IconDto Icon { get; set; }
        public string AmountWithCurrency
        {
            get
            {
                return string.Format("{0}{1}", Balance, CurrencySymbol);
            }
        }
    }
}
