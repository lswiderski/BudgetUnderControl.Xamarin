using BudgetUnderControl.Mobile.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.Mobile.Services
{
    public class GoalService : IGoalService
    {

        private readonly IGoalRepository goalRepository;

        public GoalService(IGoalRepository goalRepository)
        {
            this.goalRepository = goalRepository;
        }
    }
}
