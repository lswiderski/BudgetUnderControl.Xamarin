using BudgetUnderControl.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Common.Contracts
{
    public class AccountSyncDTO
    {
        public int Id { get; set; }
        public Guid? ExternalId { get; set; }
        public string Name { get; set; }
        public int CurrencyId { get; set; }
        public int AccountGroupId { get; set; }
        public bool IsIncludedToTotal { get; set; }
        public string Comment { get; set; }
        public int Order { get; set; }
        public AccountType Type { get; set; }
        public int? ParentAccountId { get; set; }
        public string Icon { get; set; }

        public Guid AccountGroupExternalId { get; set; }
        public Guid? ParentAccountExternalId { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }

    }
}
