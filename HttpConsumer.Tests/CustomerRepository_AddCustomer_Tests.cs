using HttpConsumer.Domain.DTOs;
using HttpConsumer.Repository.Helpers.HttpHelper;
using HttpConsumer.Repository.Implementation;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HttpConsumer.Tests
{
    public class CustomerRepository_AddCustomer_Tests
    {
        [Fact]
        public async void AddCustomer_ValidCustomer_ReturnsResourceCreatedDTO()
        {
            // Arrange
            var fakeFactory = new Mock<IHttpClientFactory>();
            var fakeResult = new ResourceCreatedDTO { Id = Guid.NewGuid() };

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Created,
                    Content = new StringContent(JsonConvert.SerializeObject(fakeResult)),
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            fakeFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var fakeHttpHelper = new Mock<HttpHelper>(fakeFactory.Object);
            var fakeRepository = new Mock<CustomerRepository>(fakeHttpHelper.Object);

            var customer = new CreateCustomerDTO
            {
                Description = "New Customer",
                Enabled = true,
                Name = "Aly",
                TenantId = Guid.NewGuid()
            };

            // Act
            var result = await fakeRepository.Object.AddCustomer(customer);

            // Assert
            Assert.IsType<ResourceCreatedDTO>(result);
        }

        [Fact]
        public async void AddCustomer_CustomerWithoutName_ReturnsNull()
        {
            // Arrange
            var fakeFactory = new Mock<IHttpClientFactory>();

            var errorDictionary = new Dictionary<string, List<string>>();
            errorDictionary.Add("Name", new List<string> { "The Name field is required." });

            var fakeResult = new ErrorResponseDTO
            {
                Type = "",
                Title = "One or more validation errors occurred.",
                Status = 400,
                TraceId = "",
                Errors = errorDictionary
            };

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(JsonConvert.SerializeObject(fakeResult)),
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            fakeFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var fakeHttpHelper = new Mock<HttpHelper>(fakeFactory.Object);
            var fakeRepository = new Mock<CustomerRepository>(fakeHttpHelper.Object);

            var customer = new CreateCustomerDTO
            {
                Description = "New Customer",
                Enabled = true,
                TenantId = Guid.NewGuid()
            };

            // Act
            var result = await fakeRepository.Object.AddCustomer(customer);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void AddCustomer_NullCustomer_ReturnsNull()
        {
            // Arrange
            var fakeFactory = new Mock<IHttpClientFactory>();
            var fakeHttpHelper = new Mock<HttpHelper>(fakeFactory.Object);
            var fakeRepository = new Mock<CustomerRepository>(fakeHttpHelper.Object);

            // Act
            var result = await fakeRepository.Object.AddCustomer(null);

            // Assert
            Assert.Null(result);
        }
    }
}
