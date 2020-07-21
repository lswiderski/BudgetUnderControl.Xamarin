using BudgetUnderControl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.Mobile.Services
{
    public interface IIconService
    {
        List<SelectIconDto> GetAvailableAccountIcons();
        SelectIconDto GetSelectIcon(string id);
        IconDto GetIcon(string id);
    }
}
