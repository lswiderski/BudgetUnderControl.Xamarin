using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Common.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.CommonInfrastructure.Commands
{
    public class EditAccount : ICommand
    {
        public int Id { get; set; }
        public int CurrencyId { get; set; }
        public string Currency { get; set; }
        public string CurrencySymbol { get; set; }
        public int AccountGroupId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public bool IsIncludedInTotal { get; set; }
        public bool IsActive { get; set; }
        public string Comment { get; set; }
        public Guid ExternalId { get; set; }
        public string Icon { get; set; }
        public string Number { get; set; }

        public AccountType Type { get; set; }
        public int? ParentAccountId { get; set; }
        public int Order { get; set; }
    }
}
