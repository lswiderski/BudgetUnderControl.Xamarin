using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BudgetUnderControl.MobileDomain
{
    public class User : ISyncable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime? ModifiedOn { get; protected set; }
        public string ExternalId { get; protected set; }
        public bool IsDeleted { get; protected set; }

        public List<Account> Accounts { get; protected set; }
        public List<Transaction> Transactions { get; protected set; }
        public List<Synchronization> Synchronizations { get; protected set; }

        public void Delete(bool delete = true)
        {
            this.IsDeleted = delete;
            this.UpdateModify();
        }

        public void UpdateModify()
        {
            this.ModifiedOn = DateTime.UtcNow;
        }

        public void EditExternalId(string newId)
        {
            this.ExternalId = newId;
            this.UpdateModify();
        }

        protected User()
        {

        }

        public static User Create(string externalId)
        {
            return new User()
            {
                ExternalId = !string.IsNullOrEmpty(externalId) ? externalId : Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
            };
        }

        public void SetModifiedOn(DateTime? dateTime)
        {
            this.ModifiedOn = dateTime;
        }
    }
}
