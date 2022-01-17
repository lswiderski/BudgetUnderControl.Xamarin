using BudgetUnderControl.CommonInfrastructure.Settings;
using BudgetUnderControl.Mobile.Keys;
using Xamarin.Essentials;

namespace BudgetUnderControl.Mobile
{
    internal class OptionService : IOptionService
    {
        private readonly GeneralSettings _settings;

        public OptionService(GeneralSettings settings)
        {
            _settings = settings;
        }

        public string GetApiUrl()
        {
            var url = Preferences.Get(PreferencesKeys.APIURL, string.Empty);
            var apiUrl = string.IsNullOrEmpty(url) || string.IsNullOrWhiteSpace(url) ? _settings.ApiBaseUri : url;

            return apiUrl;
        }
    }
}
