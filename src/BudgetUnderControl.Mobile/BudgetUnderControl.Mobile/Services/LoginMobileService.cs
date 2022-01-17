using BudgetUnderControl.MobileDomain.Repositiories;
using BudgetUnderControl.Mobile.Keys;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Essentials;
using BudgetUnderControl.CommonInfrastructure.Settings;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using BudgetUnderControl.ViewModel;
using NLog;

namespace BudgetUnderControl.Mobile.Services
{
    public class LoginMobileService : ILoginMobileService
    {

        private readonly IApiHttpClient httpClient;
        private readonly GeneralSettings settings;
        private readonly ILogger logger;
        private readonly ISettingsViewModel settingsViewModel;
        private readonly ISyncMobileService syncMobileService;
        private readonly IUserRepository userRepository;

        public LoginMobileService(GeneralSettings settings,
            ISettingsViewModel settingsViewModel,
            ISyncMobileService syncMobileService,
            IUserRepository userRepository,
            IApiHttpClient apiHttpClient,
            ILogger logger)
        {
            this.httpClient = apiHttpClient;
            this.settings = settings;
            this.settingsViewModel = settingsViewModel;
            this.syncMobileService = syncMobileService;
            this.userRepository = userRepository;
            this.logger = logger;
        }

        public async Task<bool> LoginAsync(string username, string password, bool clearLocalData)
        {
            // login
            var token = await RemoteLoginAsync(username, password);
            //if logged
            if (!string.IsNullOrEmpty(token))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenValidationParameters = new TokenValidationParameters();
                var readToken = tokenHandler.ReadJwtToken(token);
                var claim = readToken.Claims.First(c => c.Type == "unique_name");

                var userId = Guid.Parse(claim.Value);
                Preferences.Set(PreferencesKeys.JWTTOKEN, token);
                Preferences.Set(PreferencesKeys.IsUserLogged, true);
                settingsViewModel.RefreshUserButtons();

                if (clearLocalData)
                {
                    //clear DB
                    await syncMobileService.CleanDataBaseAsync();
                    await this.CreateNewUserAsync();
                }

                //set externalId
                var user = await this.userRepository.GetFirstUserAsync();

                if (user == null)
                {
                    await this.CreateNewUserAsync();
                    user = await this.userRepository.GetFirstUserAsync();
                }
                user.EditExternalId(userId.ToString());
                await userRepository.UpdateUserAsync(user);
                await SecureStorage.SetAsync(SecurityStorageKeys.UserExternalId, user.ExternalId);
                //sync
                //await syncMobileService.SyncAsync();

                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task CreateNewUserAsync()
        {
            //clear DB
            await syncMobileService.CleanDataBaseAsync();
            await this.syncMobileService.CreateNewUserAsync();
            var user = await this.userRepository.GetFirstUserAsync();
            await SecureStorage.SetAsync(SecurityStorageKeys.UserExternalId, user.ExternalId);
        }

        public async Task LogoutAsync()
        {
            Preferences.Set(PreferencesKeys.IsUserLogged, false);
            Preferences.Remove(PreferencesKeys.JWTTOKEN);
            settingsViewModel.RefreshUserButtons();
            await Task.CompletedTask;
        }

        private async Task<string> RemoteLoginAsync(string username, string password)
        {
            //call api
            try
            {
                var url = "login/mobile";
                var dataAsString = JsonConvert.SerializeObject(new { Username = username, Password = password });
                var content = new StringContent(dataAsString);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                content.Headers.Add("Api-Key", settings.ApiKey);

                var response = await httpClient.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    return string.Empty;
                }
                var statusCode = response.EnsureSuccessStatusCode();

                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var reader = new StreamReader(stream))
                {
                    var token = await reader.ReadToEndAsync();
                    return token;
                }
            }
            catch (Exception e)
            {
                //just for development purpose
                logger.Error(e);
                return string.Empty;
            }
        }
    }
}
