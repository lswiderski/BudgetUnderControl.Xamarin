using BudgetUnderControl.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BudgetUnderControl.MobileDomain
{
    public class GoalCondition : ISyncable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ExternalId { get; set; }
        public bool IsDeleted { get; set; }

        public int GoalId { get; set; }
        public string GoalExternalId { get; set; }

        public int? AccountId { get; set; }
        public string AccountExternalId { get; set; }
        public GoalConditionType ConditionType { get; set; }
        public decimal? Value { get; set; }
        public int? TagId { get; set; }
        public string TagExternalId { get; set; }
        public int Until { get; set; }
        public GoalConditionTimePeriod UntilTimePeriod { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime UntilDate { get; set; }
        public bool IsPercent { get; set; }
        public TransactionType TransactionType { get; set; }
        public GoalConditionComparerType Comparer { get; set; }

        public Goal Goal { get; set; }
        public Account Account { get; set; }
        public Tag? Tag { get; set; }

        public void Delete(bool delete = true)
        {
            this.IsDeleted = delete;
            this.UpdateModify();
        }

        public void UpdateModify()
        {
            this.SetModifiedOn(DateTime.UtcNow);
        }

        /// <summary>
        /// Use for sync/imports
        /// </summary>
        /// <param name="date"></param>
        public void SetModifiedOn(DateTime? date)
        {
            this.ModifiedOn = date;
        }
    }
}
