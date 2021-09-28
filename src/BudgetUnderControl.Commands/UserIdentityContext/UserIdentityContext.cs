using System;
using System.Collections.Generic;
using System.Text;
using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.Common.Extensions;

namespace BudgetUnderControl.CommonInfrastructure
{
    public class UserIdentityContext : IUserIdentityContext
    {
        public int UserId { get; set; }

        public Guid ExternalId { get; set; }

    }
}
