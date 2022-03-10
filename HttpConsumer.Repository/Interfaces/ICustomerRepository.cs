using HttpConsumer.Domain.DTOs;
using System;
using System.Threading.Tasks;

namespace HttpConsumer.Repository.Interfaces
{
    public interface ICustomerRepository
    {
        Task<ResourceCreatedDTO> AddCustomer(CreateCustomerDTO customer);
        Task<bool> AddProductsToWishList(Guid customerId, ProductDTO product);
        Task<bool> RemoveProductsFromWishList(Guid customerId, Guid productId);
    }
}
