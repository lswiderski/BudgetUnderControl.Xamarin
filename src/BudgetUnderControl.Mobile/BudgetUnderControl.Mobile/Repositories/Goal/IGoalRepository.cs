using BudgetUnderControl.MobileDomain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Mobile.Repositories
{
    public interface IGoalRepository
    {

        Task AddAsync(Goal goal);
        Task AddAsync(List<Goal> goals);
        Task<ICollection<Goal>> GetAsync();
        Task<ICollection<Goal>> GetAsync(List<int> goalIds);
        Task<ICollection<Goal>> GetAsync(List<string> goalIds);
        Task<Goal> GetAsync(int id);
        Task<Goal> GetAsync(string id);
        Task UpdateAsync(Goal goal);
        Task RemoveAsync(Goal goal);
        Task RemoveAsync(IEnumerable<Goal> goals);

        Task AddAsync(GoalCondition goalCondition);
        Task AddAsync(IEnumerable<GoalCondition> goalsConditions);
        Task<ICollection<GoalCondition>> GetGoalConditionsAsync();
        Task<ICollection<GoalCondition>> GetGoalConditionsAsync(int goalId);
        Task<GoalCondition> GetGoalConditionAsync(int goalConditionId);
        Task UpdateAsync(GoalCondition goalCondition);
        Task RemoveAsync(GoalCondition goalCondition);
        Task RemoveAsync(IEnumerable<GoalCondition> goalConditions);
    }
}
