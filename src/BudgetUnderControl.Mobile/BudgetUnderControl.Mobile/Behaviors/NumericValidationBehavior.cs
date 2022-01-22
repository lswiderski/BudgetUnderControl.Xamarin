using BudgetUnderControl.Mobile.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BudgetUnderControl.Behaviors
{
    public class NumericValidationBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            double result;
            bool isValid = double.TryParse(args.NewTextValue, out result);
            var isDarkModeOn = Preferences.Get(PreferencesKeys.IsDarkModeOn, false);
            var defaultColor = isDarkModeOn ? (Color)App.Current.Resources["FontColorLight"] : (Color)App.Current.Resources["FontColorDark"];
            ((Entry)sender).TextColor = isValid ? defaultColor : Color.Red;
        }
    }
}
