using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.MobileDomain
{
    public class Tag : ISyncable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }

        public int OwnerId { get; protected set; }
        public DateTime? ModifiedOn { get; protected set; }
        public string ExternalId { get; protected set; }
        public bool IsDeleted { get; protected set; }

        public virtual User Owner { get; set; }
        public ICollection<TagToTransaction> TagToTransactions { get; set; }
        public List<GoalCondition> GoalConditions { get; set; }

        public void Delete(bool delete = true)
        {
            this.IsDeleted = delete;
            this.UpdateModify();
        }

        public void UpdateModify()
        {
            this.ModifiedOn = DateTime.UtcNow;
        }

        /// <summary>
        /// Use for sync/imports
        /// </summary>
        /// <param name="date"></param>
        public void SetModifiedOn(DateTime? date)
        {
            this.ModifiedOn = date;
        }

        public static Tag Create(string name, int ownerId, bool isDeleted, string externalId)
        {
            return new Tag
            {
                Name = name,
                ExternalId = !string.IsNullOrEmpty(externalId) ? externalId : Guid.NewGuid().ToString(),
                OwnerId = ownerId,
                IsDeleted = isDeleted,
                ModifiedOn = DateTime.UtcNow,
            };
        }

        public void Edit(string name, int ownerId, bool isDeleted)
        {
            this.Name = name;
            this.OwnerId = ownerId;
            this.IsDeleted = isDeleted;
            this.UpdateModify();
        }
    }
}
