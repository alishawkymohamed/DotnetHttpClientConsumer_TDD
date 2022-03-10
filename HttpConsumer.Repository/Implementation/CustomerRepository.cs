using HttpConsumer.Domain.DTOs;
using HttpConsumer.Repository.Helpers.HttpHelper;
using HttpConsumer.Repository.Interfaces;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpConsumer.Repository.Implementation
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IHttpHelper _httpHelper;

        // Could be moved to appsettings.json file
        private string _baseUrl = "https://nakdbaseserviceapi20211025120549.azurewebsites.net/api/customer/v1/customers";
        public CustomerRepository(IHttpHelper httpHelper)
        {
            _httpHelper = httpHelper;
        }

        public async Task<ResourceCreatedDTO> AddCustomer(CreateCustomerDTO customer)
        {
            if (customer == null)
                return null;

            var message = PrepareHttpRequestMessage(customer, _baseUrl, HttpMethod.Post);
            var result = await _httpHelper.SendAsync<ResourceCreatedDTO>(message);

            return result.ResponseBody;
        }


        // I assumed system design requirments need to return bool iundicate successful adding process
        public async Task<bool> AddProductsToWishList(Guid customerId, ProductDTO product)
        {
            if (product == null || customerId == Guid.Empty)
                return false;

            var message = PrepareHttpRequestMessage(product, $"{_baseUrl}/{customerId}/wishListProducts", HttpMethod.Post);
            var result = await _httpHelper.SendAsync<object>(message);

            return result.IsSuccess;
        }


        public async Task<bool> RemoveProductsFromWishList(Guid customerId, Guid productId)
        {
            if (productId == Guid.Empty || customerId == Guid.Empty)
                return false;

            var message = PrepareHttpRequestMessage(null, $"{_baseUrl}/{customerId}/wishListProducts/{productId}", HttpMethod.Delete);
            var result = await _httpHelper.SendAsync<object>(message);

            return result.IsSuccess;
        }
        private HttpRequestMessage PrepareHttpRequestMessage(object body, string url, HttpMethod method)
        {
            HttpContent content = null;

            if (method == HttpMethod.Put || method == HttpMethod.Post)
                content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

            return new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = method,
                Content = content
            };
        }
    }
}
