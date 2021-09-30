using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.MobileDomain;
using BudgetUnderControl.MobileDomain.Repositiories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Mobile.Repositories
{
    public class SynchronizationRepository : BaseModel, ISynchronizationRepository
    {
        public SynchronizationRepository(IContextFacade context) : base(context)
        {

        }

        public async Task<IEnumerable<Synchronization>> GetSynchronizationsAsync()
        {
            var list = await this.Context.Synchronizations.ToListAsync();

            return list;
        }

        public async Task AddSynchronizationAsync(Synchronization synchronization)
        {
            this.Context.Synchronizations.Add(synchronization);
            await this.Context.SaveChangesAsync();
        }

        public async Task<Synchronization> GetSynchronizationAsync(SynchronizationComponent component, string componentId, int userId)
        {
            var currency = await this.Context.Synchronizations.FirstOrDefaultAsync(x => x.Component == component && x.ComponentId == componentId && x.UserId == userId);

            return currency;
        }

        public async Task UpdateAsync(Synchronization synchronization)
        {
            this.Context.Synchronizations.Update(synchronization);
            this.Context.SetEntityState(synchronization, EntityState.Modified);
            await this.Context.SaveChangesAsync();
        }

        public void Update(Synchronization synchronization)
        {
            this.Context.Synchronizations.Update(synchronization);
            this.Context.SetEntityState(synchronization, EntityState.Modified);
            this.Context.SaveChanges();
        }

        public async Task ClearSynchronizationAsync()
        {
            var entities = await this.Context.Synchronizations.ToListAsync();
            if(entities.Count> 0 )
            {
                this.Context.Synchronizations.RemoveRange(entities);
                await this.Context.SaveChangesAsync();
            }
           
        }
    }
}
