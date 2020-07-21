using BudgetUnderControl.Common.Contracts;
using FontAwesome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BudgetUnderControl.Mobile.Services
{
    public class IconService : IIconService
    {

        private static List<SelectIconDto> icons;

        public IconService()
        {
            icons = IconService.GetAllAvailableIcons();
        }

        public List<SelectIconDto> GetAvailableAccountIcons()
        {
            return icons;
        }

        public static List<SelectIconDto> GetAllAvailableIcons()
        {
            var result = new List<SelectIconDto>();

            result.Add(new SelectIconDto { Id = IconNames.CreditCard_FAS, Glyph = FontAwesomeIcons.CreditCard, Name = "CreditCard Solid", FontFamily = "FAS" });
            result.Add(new SelectIconDto {Id = IconNames.CreditCard_FAR, Glyph = FontAwesomeIcons.CreditCard, Name = "CreditCard Regular", FontFamily = "FAR" });

            result.Add(new SelectIconDto {Id = IconNames.CcVisa_FAB, Glyph = FontAwesomeIcons.CcVisa, Name = nameof(FontAwesomeIcons.CcVisa), FontFamily = "FAB" });
            result.Add(new SelectIconDto {Id = IconNames.CcPaypal_FAB, Glyph = FontAwesomeIcons.CcPaypal, Name = nameof(FontAwesomeIcons.CcPaypal), FontFamily = "FAB" });
            result.Add(new SelectIconDto {Id = IconNames.CcMastercard_FAB, Glyph = FontAwesomeIcons.CcMastercard, Name = nameof(FontAwesomeIcons.CcMastercard), FontFamily = "FAB" });
            result.Add(new SelectIconDto {Id = IconNames.Wallet_FAS, Glyph = FontAwesomeIcons.Wallet, Name = nameof(FontAwesomeIcons.Wallet), FontFamily = "FAS" });
            result.Add(new SelectIconDto {Id = IconNames.PiggyBank_FAS, Glyph = FontAwesomeIcons.PiggyBank, Name = nameof(FontAwesomeIcons.PiggyBank), FontFamily = "FAS" });
            result.Add(new SelectIconDto {Id = IconNames.University_FAS, Glyph = FontAwesomeIcons.University, Name = nameof(FontAwesomeIcons.University), FontFamily = "FAS" });
            result.Add(new SelectIconDto {Id = IconNames.Landmark_FAS, Glyph = FontAwesomeIcons.Landmark, Name = nameof(FontAwesomeIcons.Landmark), FontFamily = "FAS" });
            result.Add(new SelectIconDto {Id = IconNames.MoneyCheck_FAS, Glyph = FontAwesomeIcons.MoneyCheck, Name = nameof(FontAwesomeIcons.MoneyCheck), FontFamily = "FAS" });
            result.Add(new SelectIconDto {Id = IconNames.MoneyCheckAlt_FAS, Glyph = FontAwesomeIcons.MoneyCheckAlt, Name = nameof(FontAwesomeIcons.MoneyCheckAlt), FontFamily = "FAS" });
            result.Add(new SelectIconDto {Id = IconNames.MoneyBillAlt_FAS, Glyph = FontAwesomeIcons.MoneyBillAlt, Name = "MoneyBillAlt Solid", FontFamily = "FAS" });
            result.Add(new SelectIconDto {Id = IconNames.MoneyBillAlt_FAR, Glyph = FontAwesomeIcons.MoneyBillAlt, Name = "MoneyBillAlt Regular", FontFamily = "FAR" });
            result.Add(new SelectIconDto {Id = IconNames.Coins_FAS, Glyph = FontAwesomeIcons.Coins, Name = nameof(FontAwesomeIcons.Coins), FontFamily = "FAS" });
            result.Add(new SelectIconDto {Id = IconNames.HandHoldingUsd_FAS, Glyph = FontAwesomeIcons.HandHoldingUsd, Name = nameof(FontAwesomeIcons.HandHoldingUsd), FontFamily = "FAS" });
            result.Add(new SelectIconDto {Id = IconNames.DollarSign_FAS, Glyph = FontAwesomeIcons.DollarSign, Name = nameof(FontAwesomeIcons.DollarSign), FontFamily = "FAS" });
            result.Add(new SelectIconDto {Id = IconNames.EuroSign_FAS, Glyph = FontAwesomeIcons.EuroSign, Name = nameof(FontAwesomeIcons.EuroSign), FontFamily = "FAS" });
            result.Add(new SelectIconDto {Id = IconNames.LiraSign_FAS, Glyph = FontAwesomeIcons.LiraSign, Name = nameof(FontAwesomeIcons.LiraSign), FontFamily = "FAS" });
            result.Add(new SelectIconDto {Id = IconNames.PoundSign_FAS, Glyph = FontAwesomeIcons.PoundSign, Name = nameof(FontAwesomeIcons.PoundSign), FontFamily = "FAS" });
            result.Add(new SelectIconDto {Id = IconNames.RupeeSign_FAS, Glyph = FontAwesomeIcons.RupeeSign, Name = nameof(FontAwesomeIcons.RupeeSign), FontFamily = "FAS" });
            result.Add(new SelectIconDto {Id = IconNames.RubleSign_FAS, Glyph = FontAwesomeIcons.RubleSign, Name = nameof(FontAwesomeIcons.RubleSign), FontFamily = "FAS" });
            result.Add(new SelectIconDto {Id = IconNames.ShekelSign_FAS, Glyph = FontAwesomeIcons.ShekelSign, Name = nameof(FontAwesomeIcons.ShekelSign), FontFamily = "FAS" });
            result.Add(new SelectIconDto {Id = IconNames.Tenge_FAS, Glyph = FontAwesomeIcons.Tenge, Name = nameof(FontAwesomeIcons.Tenge), FontFamily = "FAS" });
            result.Add(new SelectIconDto {Id = IconNames.WonSign_FAS, Glyph = FontAwesomeIcons.WonSign, Name = nameof(FontAwesomeIcons.WonSign), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Id = IconNames.YenSign_FAS, Glyph = FontAwesomeIcons.YenSign, Name = nameof(FontAwesomeIcons.YenSign), FontFamily = "FAS" });

            result.Add(new SelectIconDto { Id = IconNames.Utensils_FAS, Glyph = FontAwesomeIcons.Utensils, Name = nameof(FontAwesomeIcons.Utensils), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Id = IconNames.Bus_FAS, Glyph = FontAwesomeIcons.Bus, Name = nameof(FontAwesomeIcons.Bus), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Id = IconNames.StickyNote_FAR, Glyph = FontAwesomeIcons.StickyNote, Name = nameof(FontAwesomeIcons.StickyNote), FontFamily = "FAR" });
            result.Add(new SelectIconDto { Id = IconNames.HandHoldingUsd_FAS, Glyph = FontAwesomeIcons.HandHoldingUsd, Name = nameof(FontAwesomeIcons.HandHoldingUsd), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Id = IconNames.Receipt_FAS, Glyph = FontAwesomeIcons.Receipt, Name = nameof(FontAwesomeIcons.Receipt), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Id = IconNames.GlassCheers_FAS, Glyph = FontAwesomeIcons.GlassCheers, Name = nameof(FontAwesomeIcons.GlassCheers), FontFamily = "FAS" }); ;
            result.Add(new SelectIconDto { Id = IconNames.Heartbeat_FAS, Glyph = FontAwesomeIcons.Heartbeat, Name = nameof(FontAwesomeIcons.Heartbeat), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Id = IconNames.MapMarkerAlt_FAS, Glyph = FontAwesomeIcons.MapMarkerAlt, Name = nameof(FontAwesomeIcons.MapMarkerAlt), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Id = IconNames.Home_FAS, Glyph = FontAwesomeIcons.Home, Name = nameof(FontAwesomeIcons.Home), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Id = IconNames.Smile_FAR, Glyph = FontAwesomeIcons.Smile, Name = nameof(FontAwesomeIcons.Smile), FontFamily = "FAR" });
            result.Add(new SelectIconDto { Id = IconNames.ShoppingBasket_FAS, Glyph = FontAwesomeIcons.ShoppingBasket, Name = nameof(FontAwesomeIcons.ShoppingBasket), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Id = IconNames.MoneyCheckAlt_FAS, Glyph = FontAwesomeIcons.MoneyCheckAlt, Name = nameof(FontAwesomeIcons.MoneyCheckAlt), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Id = IconNames.Gift_FAS, Glyph = FontAwesomeIcons.Gift, Name = nameof(FontAwesomeIcons.Gift), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Id = IconNames.Bed_FAS, Glyph = FontAwesomeIcons.Bed, Name = nameof(FontAwesomeIcons.Bed), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Id = IconNames.Landmark_FAS, Glyph = FontAwesomeIcons.Landmark, Name = nameof(FontAwesomeIcons.Landmark), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Id = IconNames.Tshirt_FAS, Glyph = FontAwesomeIcons.Tshirt, Name = nameof(FontAwesomeIcons.Tshirt), FontFamily = "FAS" });
            result.Add(new SelectIconDto { Id = IconNames.Suitcase_FAS, Glyph = FontAwesomeIcons.Suitcase, Name = nameof(FontAwesomeIcons.Suitcase), FontFamily = "FAS" });

            return result;
        }

        public SelectIconDto GetSelectIcon(string id)
        {
            var icon = icons.Where(x => x.Id == id).FirstOrDefault();

            return icon;
        }

        public IconDto GetIcon(string id)
        {
            var icon = icons.Where(x => x.Id == id).Select(x => new IconDto { Id = x.Id, FontFamily = x.FontFamily, Glyph = x.Glyph }).FirstOrDefault();

            return icon;
        }
    }
}
