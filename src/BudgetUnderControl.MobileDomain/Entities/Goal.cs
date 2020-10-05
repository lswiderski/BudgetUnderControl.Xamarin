using BudgetUnderControl.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BudgetUnderControl.MobileDomain
{
    public class Goal : ISyncable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ExternalId { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime UntilDate { get; set; }

        public string Name { get; set; }

        public GoalConnectiveOfConditionsType ConnectiveOfConditions { get; set; }

        public decimal? ValueIfFail { get; set; }

        public bool Repeat { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsReached { get; set; }

        public int UserId { get; set; }

        public virtual User Owner { get; set; }

        public List<GoalCondition> GoalConditions { get; set; }


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
