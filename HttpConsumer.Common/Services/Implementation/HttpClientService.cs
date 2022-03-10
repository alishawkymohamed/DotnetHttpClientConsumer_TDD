using HttpConsumer.Common.Services.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpConsumer.Common.Services.Implementation
{
    public class HttpClientService : IHttpClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient _httpClient;

        public HttpClientService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();
        }

        public async Task<HttpResponseMessage> SendAsync()
        {
            var result = await _httpClient.SendAsync(new HttpRequestMessage());
        }
    }
}
