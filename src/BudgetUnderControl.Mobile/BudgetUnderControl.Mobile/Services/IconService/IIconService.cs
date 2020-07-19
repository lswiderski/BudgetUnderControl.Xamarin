using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.Mobile.Services
{
    public interface IIconService
    {
        List<SelectIconDto> GetAvailableAccountIcons();
        SelectIconDto GetIcon(List<SelectIconDto> icons, string glyph, string font);
    }
}
