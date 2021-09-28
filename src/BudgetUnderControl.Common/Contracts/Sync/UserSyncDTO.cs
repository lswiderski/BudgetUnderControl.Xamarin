using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.Common.Contracts
{
    public class UserSyncDTO
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid ExternalId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
