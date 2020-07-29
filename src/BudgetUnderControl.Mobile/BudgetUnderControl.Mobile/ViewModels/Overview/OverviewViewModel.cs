using BudgetUnderControl.CommonInfrastructure.Commands;
using BudgetUnderControl.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetUnderControl.CommonInfrastructure;
using BudgetUnderControl.CommonInfrastructure.Settings;

namespace BudgetUnderControl.ViewModel
{
    public class OverviewViewModel : IOverviewViewModel
    {
        IAccountService accountService;
        ICurrencyService currencyService;
        private readonly GeneralSettings settings;
        private readonly IBalanceService balanceService;
        public OverviewViewModel(IAccountService accountModel, ICurrencyService currencyService, IBalanceService balanceService, GeneralSettings settings)
        {
            this.accountService = accountModel;
            this.currencyService = currencyService;
            this.balanceService = balanceService;
            this.settings = settings;
            this.AdUnitId = settings.AdMobAdId;
        }

        public string AdUnitId { get; set; }

        public async Task<IEnumerable<BalanceDto>> GetCurrentBalanceAsync()
        {
            var balance = await this.balanceService.GetTotalCurrentBalanceAsync();

            return balance.Total;
        }
     
    }
}
