using HttpConsumer.Domain.DTOs;
using HttpConsumer.Repository.Helpers.HttpHelper;
using HttpConsumer.Repository.Implementation;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HttpConsumer.Tests
{
    public class CustomerRepository_AddProductsToWishList_Tests
    {
        [Fact]
        public async void AddProductsToWishList_EmptyIdValidProduct_ReturnsFalse()
        {
            // Arrange
            var fakeFactory = new Mock<IHttpClientFactory>();
            var fakeHttpHelper = new Mock<HttpHelper>(fakeFactory.Object);
            var fakeRepository = new Mock<CustomerRepository>(fakeHttpHelper.Object);
            var fakeProduct = new Mock<ProductDTO>();

            // Act
            var result = await fakeRepository.Object.AddProductsToWishList(Guid.Empty, fakeProduct.Object);

            // Assert
            Assert.False(result);
        }


        [Fact]
        public async void AddProductsToWishList_ValidIdNullProduct_ReturnsFalse()
        {
            // Arrange
            var fakeFactory = new Mock<IHttpClientFactory>();
            var fakeHttpHelper = new Mock<HttpHelper>(fakeFactory.Object);
            var fakeRepository = new Mock<CustomerRepository>(fakeHttpHelper.Object);

            // Act
            var result = await fakeRepository.Object.AddProductsToWishList(Guid.NewGuid(), null);

            // Assert
            Assert.False(result);
        }


        [Fact]
        public async void AddProductsToWishList_ValidIdValidProduct_ReturnsTrue()
        {
            // Arrange
            var fakeFactory = new Mock<IHttpClientFactory>();

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NoContent,
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            fakeFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var fakeHttpHelper = new Mock<HttpHelper>(fakeFactory.Object);
            var fakeRepository = new Mock<CustomerRepository>(fakeHttpHelper.Object);

            var product = new ProductDTO
            {
                Id = Guid.NewGuid(),
                Description = $"New Product {Guid.NewGuid()}",
                Name = $"New Product Name {Guid.NewGuid()}",
            };

            // Act
            var result = await fakeRepository.Object.AddProductsToWishList(Guid.NewGuid(), product);

            // Assert
            Assert.True(result);
        }
    }
}
