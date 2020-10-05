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
        [StringLength(50)]
        public string Username { get; protected set; }
        public string Role { get; protected set; }
        [StringLength(150)]
        public string Email { get; protected set; }
        public string Password { get; protected set; }
        public string Salt { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime? ModifiedOn { get; protected set; }
        public string ExternalId { get; protected set; }
        public bool IsDeleted { get; protected set; }

        public List<Account> Accounts { get; protected set; }
        public List<AccountGroup> AccountGroups { get; protected set; }
        public List<Transaction> Transactions { get; protected set; }
        public List<Goal> Goals { get; protected set; }

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
    }
}
