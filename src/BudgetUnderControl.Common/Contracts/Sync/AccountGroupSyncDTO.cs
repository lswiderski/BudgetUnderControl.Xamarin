using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.Common.Contracts
{
    public class AccountGroupSyncDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid ExternalId { get; set; }
        public bool IsDeleted { get; set; }

        public Guid OwnerExternalId { get; set; }
    }
}
