using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.CommonInfrastructure.Commands;
using BudgetUnderControl.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.ViewModel
{
    public interface IAddAccountViewModel
    {
        string Name { get; set; }
        string Number { get; set; }
        string Comment { get; set; }
        bool IsInTotal { get; set; }
        string Amount { get; set; }
        string Order { get; set; }
        int SelectedCurrencyIndex { get; set; }
        int SelectedAccountIndex { get; set; }
        int SelectedAccountTypeIndex { get; set; }
        List<CurrencyDTO> Currencies { get; }
        List<AccountListItemDTO> Accounts { get; }
        List<AccountTypeDTO> AccountTypes { get; }
        List<SelectIconDto> Icons { get; }
        SelectIconDto SelectedIcon { get; }

        Task AddAccount();
        void ClearParentAccountCombo();
    }
}
