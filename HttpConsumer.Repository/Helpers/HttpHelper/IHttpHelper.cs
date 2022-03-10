using HttpConsumer.Domain.DTOs;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpConsumer.Repository.Helpers.HttpHelper
{
    public interface IHttpHelper
    {
        Task<HttpResponse<T>> SendAsync<T>(HttpRequestMessage message);
    }
}
