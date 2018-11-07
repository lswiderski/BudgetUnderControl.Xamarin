﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.ViewModel
{
    public interface IOverviewViewModel
    {
        Task<Dictionary<string, decimal>> GetTotalsAsync();
    }
}
