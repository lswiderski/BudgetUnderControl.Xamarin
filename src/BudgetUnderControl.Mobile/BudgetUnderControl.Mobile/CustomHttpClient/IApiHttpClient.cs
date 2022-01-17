using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BudgetUnderControl.Mobile
{
    public interface IApiHttpClient
    {
        HttpClient GetClient();

        void SetAuthorization(AuthenticationHeaderValue authenticationHeaderValue);
        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);
    }
}
