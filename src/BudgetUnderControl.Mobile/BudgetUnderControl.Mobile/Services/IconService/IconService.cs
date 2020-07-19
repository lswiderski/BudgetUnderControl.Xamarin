using FontAwesome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BudgetUnderControl.Mobile.Services
{
    public class IconService : IIconService
    {

        public IconService()
        {

        }

        public List<SelectIconDto> GetAvailableAccountIcons()
        {
            var result = new List<SelectIconDto>();

            result.Add(new SelectIconDto { Glyph = FontAwesomeIcons.CreditCard, Name = "CreditCard Solid", FontFamily="FAS" });
            result.Add(new SelectIconDto { Glyph = FontAwesomeIcons.CreditCard, Name = "CreditCard Regular", FontFamily = "FAR" });
            result.Add(new SelectIconDto { Glyph = FontAwesomeIcons.CcVisa, Name = nameof(FontAwesomeIcons.CcVisa), FontFamily = "FAB" });
            result.Add(new SelectIconDto { Glyph = FontAwesomeIcons.CcPaypal, Name = nameof(FontAwesomeIcons.CcPaypal), FontFamily = "FAB" });
            result.Add(new SelectIconDto { Glyph = FontAwesomeIcons.CcMastercard, Name = nameof(FontAwesomeIcons.CcMastercard), FontFamily = "FAB" });
            result.Add(new SelectIconDto { Glyph = FontAwesomeIcons.Wallet, Name = nameof(FontAwesomeIcons.Wallet), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Glyph = FontAwesomeIcons.PiggyBank, Name = nameof(FontAwesomeIcons.PiggyBank), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Glyph = FontAwesomeIcons.University, Name = nameof(FontAwesomeIcons.University), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Glyph = FontAwesomeIcons.Landmark, Name = nameof(FontAwesomeIcons.Landmark), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Glyph = FontAwesomeIcons.MoneyCheck, Name = nameof(FontAwesomeIcons.MoneyCheck), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Glyph = FontAwesomeIcons.MoneyCheckAlt, Name = nameof(FontAwesomeIcons.MoneyCheckAlt), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Glyph = FontAwesomeIcons.MoneyBillAlt, Name = "MoneyBillAlt Solid", FontFamily = "FAS" });
            result.Add(new SelectIconDto { Glyph = FontAwesomeIcons.MoneyBillAlt, Name = "MoneyBillAlt Regular", FontFamily = "FAR" });
            result.Add(new SelectIconDto { Glyph = FontAwesomeIcons.Coins, Name = nameof(FontAwesomeIcons.Coins), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Glyph = FontAwesomeIcons.HandHoldingUsd, Name = nameof(FontAwesomeIcons.HandHoldingUsd), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Glyph = FontAwesomeIcons.DollarSign, Name = nameof(FontAwesomeIcons.DollarSign), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Glyph = FontAwesomeIcons.EuroSign, Name = nameof(FontAwesomeIcons.EuroSign), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Glyph = FontAwesomeIcons.LiraSign, Name = nameof(FontAwesomeIcons.LiraSign), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Glyph = FontAwesomeIcons.PoundSign, Name = nameof(FontAwesomeIcons.PoundSign), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Glyph = FontAwesomeIcons.RupeeSign, Name = nameof(FontAwesomeIcons.RupeeSign), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Glyph = FontAwesomeIcons.RubleSign, Name = nameof(FontAwesomeIcons.RubleSign), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Glyph = FontAwesomeIcons.ShekelSign, Name = nameof(FontAwesomeIcons.ShekelSign), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Glyph = FontAwesomeIcons.Tenge, Name = nameof(FontAwesomeIcons.Tenge), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Glyph = FontAwesomeIcons.WonSign, Name = nameof(FontAwesomeIcons.WonSign), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Glyph = FontAwesomeIcons.YenSign, Name = nameof(FontAwesomeIcons.YenSign), FontFamily = "FAS" });
            return result;
        }

        public SelectIconDto GetIcon(List<SelectIconDto> icons, string glyph, string font)
        {
            var icon = icons.Where(x => x.Glyph == glyph && x.FontFamily == font).FirstOrDefault();

            return icon;
        }
    }
}
