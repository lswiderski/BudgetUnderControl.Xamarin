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
        public int AccountGroupId { get; protected set; }
        public bool IsIncludedToTotal { get; protected set; }
        public string Comment { get; protected set; }
        public int Order { get; protected set; }
        public AccountType Type { get; protected set; }
        public int? ParentAccountId { get; protected set; }
        public bool IsActive { get; protected set; }
        public string Number { get; protected set; }
        public string IconGlyph { get; protected set; }
        public string IconFont { get; protected set; }
        public string IconColor { get; protected set; }
        public string IconBackgroundColor { get; protected set; }

        public int OwnerId { get; protected set; }
        public DateTime? ModifiedOn { get; protected set; }
        public string ExternalId { get; protected set; }
        public bool IsDeleted { get; protected set; }

        public AccountGroup AccountGroup { get; protected set; }
        public Currency Currency { get; set; }
        public List<AccountSnapshot> AccountSnapshots { get; protected set; }
        public List<Transaction> Transactions { get; protected set; }
        public virtual User Owner { get; set; }


        protected Account()
        {

        }

        public static Account Create(string name, string number, int currencyId, int accountGroupId,
            bool isIncludedToTotal, string comment, int order, AccountType type,
            int? parentAccountId, bool isActive, int ownerId, string externalId = null, IconDto icon = null)
        {
            return new Account()
            {
                Name = name,
                Number = number,
                CurrencyId = currencyId,
                AccountGroupId = accountGroupId,
                IsActive = isActive,
                IsIncludedToTotal = isIncludedToTotal,
                Comment = comment,
                Order = order,
                Type = type,
                ParentAccountId = parentAccountId,
                ExternalId = !string.IsNullOrEmpty(externalId) ? externalId : Guid.NewGuid().ToString(),
                OwnerId = ownerId,
                ModifiedOn = DateTime.UtcNow,
                IsDeleted = !isActive,
                IconBackgroundColor = icon?.BackGround,
                IconColor = icon?.Color,
                IconFont = icon?.FontFamily,
                IconGlyph = icon?.Glyph,
            };
        }

        public void Edit(string name,string number, int currencyId, int accountGroupId,
            bool isIncludedToTotal, string comment, int order, AccountType type,
            int? parentAccountId, bool isActive, int? ownerId = null, IconDto icon = null)
        {
            this.Name = name;
            this.Number = number;
            this.CurrencyId = currencyId;
            this.AccountGroupId = accountGroupId;
            this.IsActive = isActive;
            this.IsIncludedToTotal = isIncludedToTotal;
            this.Comment = comment;
            this.Order = order;
            this.Type = type;
            this.ParentAccountId = parentAccountId;
            this.IsDeleted = !isActive;
            if (ownerId != null)
            {
                this.OwnerId = ownerId.Value;
            }

            this.IconBackgroundColor = icon?.BackGround;
            this.IconColor = icon?.Color;
            this.IconFont = icon?.FontFamily;
            this.IconGlyph = icon?.Glyph;

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
            this.IsDeleted = isActive;
            this.UpdateModify();
        }

        public void Delete(bool delete = true)
        {
            this.SetActive(delete);
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
