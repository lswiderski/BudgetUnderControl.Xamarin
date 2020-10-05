using BudgetUnderControl.CommonInfrastructure;
using BudgetUnderControl.MobileDomain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Mobile.Repositories
{
    public class GoalRepository : BaseModel, IGoalRepository
    {
        private readonly IUserIdentityContext userIdentityContext;

        public GoalRepository(IContextFacade context, IUserIdentityContext userIdentityContext) : base(context)
        {
            this.userIdentityContext = userIdentityContext;
        }

        public async Task AddAsync(Goal goal)
        {
            await this.Context.Goals.AddAsync(goal);
            await this.Context.SaveChangesAsync();
        }

        public async Task AddAsync(List<Goal> goals)
        {
            await this.Context.Goals.AddRangeAsync(goals);
            await this.Context.SaveChangesAsync();
        }

        public async Task<ICollection<Goal>> GetAsync()
        {
            var list = await this.Context.Goals
                .Where(t => t.UserId == userIdentityContext.UserId)
                .OrderByDescending(t => t.ModifiedOn)
                .ToListAsync();

            return list;
        }

        public async Task<ICollection<Goal>> GetAsync(List<int> goalIds)
        {
            var list = await this.Context.Goals
                .Where(t => t.UserId == userIdentityContext.UserId
                && goalIds.Contains(t.Id))
                .OrderByDescending(t => t.ModifiedOn)
                .ToListAsync();

            return list;
        }

        public async Task<ICollection<Goal>> GetAsync(List<string> goalIds)
        {
            var list = await this.Context.Goals
                .Where(t => t.UserId == userIdentityContext.UserId
                && goalIds.Contains(t.ExternalId))
                .OrderByDescending(t => t.ModifiedOn)
                .ToListAsync();

            return list;
        }

        public async Task<Goal> GetAsync(int id)
        {
            var goal = await this.Context.Goals
                .Where(t => t.UserId == userIdentityContext.UserId)
                .FirstOrDefaultAsync(x => x.Id == id);

            return goal;
        }

        public async Task<Goal> GetAsync(string id)
        {
            var goal = await this.Context.Goals.Where(t => t.UserId == userIdentityContext.UserId).FirstOrDefaultAsync(x => x.ExternalId == id);

            return goal;
        }

        public async Task UpdateAsync(Goal goal)
        {
            this.Context.Goals.Update(goal);
            await this.Context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Goal goal)
        {
            this.Context.Goals.Remove(goal);
            await this.Context.SaveChangesAsync();
        }

        public async Task RemoveAsync(IEnumerable<Goal> goals)
        {
            var goalIds = goals.Select(x => x.Id).ToList();
            var t2t = await this.Context.GoalConditions.Where(t => goalIds.Contains(t.GoalId)).ToListAsync();
            await this.RemoveAsync(t2t);
            this.Context.Goals.RemoveRange(goals);
            await this.Context.SaveChangesAsync();
        }

        public async Task<GoalCondition> GetGoalConditionAsync(int goalConditionId)
        {
            var goalCondition = await this.Context.GoalConditions.FirstOrDefaultAsync(x => x.Id == goalConditionId);
            return goalCondition;
        }

        public async Task<ICollection<GoalCondition>> GetGoalConditionsAsync()
        {
            var list = await this.Context.GoalConditions.ToListAsync();
            return list;
        }

        public async Task<ICollection<GoalCondition>> GetGoalConditionsAsync(int goalId)
        {
            var list = await this.Context.GoalConditions
                .Where(t => t.GoalId == goalId)
                .ToListAsync();
            return list;
        }

        public async Task AddAsync(GoalCondition goalCondition)
        {
            await this.Context.GoalConditions.AddAsync(goalCondition);
            await this.Context.SaveChangesAsync();
        }

        public async Task AddAsync(IEnumerable<GoalCondition> tagsToTransaction)
        {
            await this.Context.GoalConditions.AddRangeAsync(tagsToTransaction);
            await this.Context.SaveChangesAsync();
        }

        public async Task UpdateAsync(GoalCondition goalCondition)
        {
            this.Context.GoalConditions.Update(goalCondition);
            await this.Context.SaveChangesAsync();
        }

        public async Task RemoveAsync(GoalCondition goalCondition)
        {
            this.Context.GoalConditions.Remove(goalCondition);
            await this.Context.SaveChangesAsync();
        }

        public async Task RemoveAsync(IEnumerable<GoalCondition> tagsToTransaction)
        {
            this.Context.GoalConditions.RemoveRange(tagsToTransaction);
            await this.Context.SaveChangesAsync();
        }
    }
}
