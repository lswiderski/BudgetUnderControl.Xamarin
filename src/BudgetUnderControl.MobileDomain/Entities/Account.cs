using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.MobileDomain
{
    public class Account : ISyncable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; protected set; }
        [StringLength(250)]
        public string Name { get; protected set; }
        public int CurrencyId { get; protected set; }
        public bool IsIncludedToTotal { get; protected set; }
        public string Comment { get; protected set; }
        public int Order { get; protected set; }
        public AccountType Type { get; protected set; }
        public int? ParentAccountId { get; protected set; }
        public bool IsActive { get; protected set; }
        public string Number { get; protected set; }
        public string Icon { get; protected set; }

        public int OwnerId { get; protected set; }
        public DateTime? ModifiedOn { get; protected set; }
        public string ExternalId { get; protected set; }
        public bool IsDeleted { get; protected set; }

        public Currency Currency { get; set; }
        public List<AccountSnapshot> AccountSnapshots { get; protected set; }
        public List<Transaction> Transactions { get; protected set; }
        public virtual User Owner { get; set; }


        protected Account()
        {

        }

        public static Account Create(string name, string number, int currencyId,
            bool isIncludedToTotal, string comment, int order, AccountType type,
            int? parentAccountId, bool isActive, bool isDeleted, int ownerId, string externalId = null, string icon = null)
        {
            return new Account()
            {
                Name = name,
                Number = number,
                CurrencyId = currencyId,
                IsActive = isActive,
                IsIncludedToTotal = isIncludedToTotal,
                Comment = comment,
                Order = order,
                Type = type,
                ParentAccountId = parentAccountId,
                ExternalId = !string.IsNullOrEmpty(externalId) ? externalId : Guid.NewGuid().ToString(),
                OwnerId = ownerId,
                ModifiedOn = DateTime.UtcNow,
                IsDeleted = isDeleted,
                Icon = icon,
            };
        }

        public void Edit(string name,string number, int currencyId,
            bool isIncludedToTotal, string comment, int order, AccountType type,
            int? parentAccountId, bool isActive, bool isDeleted, int? ownerId = null, string icon = null)
        {
            this.Name = name;
            this.Number = number;
            this.CurrencyId = currencyId;
            this.IsActive = isActive;
            this.IsIncludedToTotal = isIncludedToTotal;
            this.Comment = comment;
            this.Order = order;
            this.Type = type;
            this.ParentAccountId = parentAccountId;
            this.IsDeleted = isDeleted;
            if (ownerId != null)
            {
                this.OwnerId = ownerId.Value;
            }
            this.Icon = icon;

            this.UpdateModify();
        }

        /// <summary>
        /// Use for sync/imports
        /// </summary>
        /// <param name="id"></param>
        public void SetId(int id)
        {
            this.Id = id;
        }

        /// <summary>
        /// Use for sync/imports
        /// </summary>
        /// <param name="id"></param>
        public void SetParentAccountId(int? id)
        {
            this.ParentAccountId = id;
        }

        /// <summary>
        /// Use for sync/imports
        /// </summary>
        /// <param name="id"></param>
        public void SetActive(bool isActive)
        {
            this.IsActive = isActive;
            this.IsDeleted = !isActive;
            this.UpdateModify();
        }

        public void Delete(bool delete = true)
        {
            this.SetActive(!delete);
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
    }
}
