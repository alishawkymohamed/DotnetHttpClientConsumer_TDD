using HttpConsumer.Domain.DTOs;
using HttpConsumer.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HttpConsumer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var customer = new CreateCustomerDTO
            {
                Description = "New Customer",
                Enabled = true,
                Name = "Aly",
                TenantId = Guid.NewGuid()
            };

            var result = await _customerRepository.AddCustomer(customer);

            var productId = Guid.NewGuid();

            await _customerRepository.AddProductsToWishList(result.Id, new ProductDTO
            {
                Id = productId,
                Description = $"New Product {Guid.NewGuid()}",
                Name = $"New Product Name {Guid.NewGuid()}",
            });

            await _customerRepository.RemoveProductsFromWishList(result.Id, productId);

            return Ok(result);
        }
    }
}
