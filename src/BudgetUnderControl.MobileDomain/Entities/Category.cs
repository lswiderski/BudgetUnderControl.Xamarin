using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.MobileDomain
{
    public class Category : ISyncable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public bool IsDefault { get; set; }

        public string Icon { get; protected set; }

        public int OwnerId { get; protected set; }
        public DateTime? ModifiedOn { get; protected set; }
        public string ExternalId { get; protected set; }
        public bool IsDeleted { get; protected set; }

        public virtual User Owner { get; set; }
        public List<Transaction> Transactions { get; set; }

        [NotMapped]
        public string ExternalIdString
        {
            get
            {
                return this.ExternalId.ToString();
            }
        }

        public static Category Create(string name, int ownerId, string externalId = null, string icon = null)
        {
            return new Category
            {
                Name = name,
                OwnerId = ownerId,
                ExternalId = !string.IsNullOrEmpty(externalId) ? externalId : Guid.NewGuid().ToString(),
                ModifiedOn = DateTime.UtcNow,
                IsDeleted = false,
                IsDefault = false,
                Icon = icon,
            };
        }

        public void Edit(string name, string icon = null)
        {
            this.Name = name;
            this.Icon = icon;
            this.UpdateModify();
        }

        public void Delete(bool delete = true)
        {
            this.IsDeleted = delete;
            this.UpdateModify();
        }

        public void SetModifiedOn(DateTime? dateTime)
        {
            this.ModifiedOn = dateTime;
        }

        public void UpdateModify()
        {
            this.ModifiedOn = DateTime.UtcNow;
        }

        public Category SetDefault(bool isDefault = true)
        {
            this.IsDefault = true;
            return this;
        }
    }
}
