using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.Common.Contracts
{
    public class CategorySyncDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }

        public Guid OwnerExternalId { get; set; } 
        public int OwnerId { get;  set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid ExternalId { get; set; }
        public bool IsDeleted { get; set; }

    }
}
