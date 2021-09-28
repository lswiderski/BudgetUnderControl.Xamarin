using BudgetUnderControl.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.CommonInfrastructure
{
    public interface IUserIdentityContext
    {
        int UserId { get; }

        Guid ExternalId { get; }
    }
}
