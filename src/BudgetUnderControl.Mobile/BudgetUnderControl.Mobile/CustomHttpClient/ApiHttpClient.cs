using Autofac;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Mobile
{
    internal class ApiHttpClient : IApiHttpClient
    {
        private HttpClient _httpClient;
        private IOptionService _optionService;

        public ApiHttpClient(IOptionService optionService)
        {
            _httpClient = App.Container.ResolveNamed<HttpClient>("api");
            _optionService = optionService;
        }
        public HttpClient GetClient()
        {
            return _httpClient;
        }

        public void SetAuthorization(AuthenticationHeaderValue authenticationHeaderValue)
        {
            _httpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            var baseUrl = _optionService.GetApiUrl();
            var response = await _httpClient.PostAsync($"{baseUrl}{requestUri}", content);
            return response;
        }
    }
}
