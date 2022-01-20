using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.Mobile
{
    public interface IThemeChanger
    {
        void ApplyTheme(bool darkmodeOn, bool refresh);
    }
}
