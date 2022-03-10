using HttpConsumer.Domain.DTOs;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpConsumer.Repository.Helpers.HttpHelper
{
    public class HttpHelper : IHttpHelper
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpHelper(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<HttpResponse<T>> SendAsync<T>(HttpRequestMessage message)
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var result = await client.SendAsync(message);
                string response = string.Empty;

                switch (result.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        response = await result.Content.ReadAsStringAsync();
                        var errors = JsonConvert.DeserializeObject<ErrorResponseDTO>(response);
                        // Follow business requirments .. Maybe throw exception or handle that case
                        return new HttpResponse<T> { IsSuccess = false };

                    case HttpStatusCode.Created:
                        response = await result.Content.ReadAsStringAsync();
                        return new HttpResponse<T> { IsSuccess = true, ResponseBody = JsonConvert.DeserializeObject<T>(response) };

                    case HttpStatusCode.NoContent:
                        return new HttpResponse<T> { IsSuccess = true, ResponseBody = default(T) };

                    case HttpStatusCode.NotFound:
                        return new HttpResponse<T> { IsSuccess = false, ResponseBody = default(T) };

                    default:
                        return new HttpResponse<T> { IsSuccess = false };
                }
            }
        }
    }
}
