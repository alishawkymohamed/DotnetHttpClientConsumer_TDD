using HttpConsumer.Repository.Helpers.HttpHelper;
using HttpConsumer.Repository.Implementation;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HttpConsumer.Tests
{
    public class CustomerRepository_RemoveProductsFromWishList_Tests
    {
        [Fact]
        public async void AddProductsToWishList_EmptyCutomerIdValidProductId_ReturnsFalse()
        {
            // Arrange
            var fakeFactory = new Mock<IHttpClientFactory>();
            var fakeHttpHelper = new Mock<HttpHelper>(fakeFactory.Object);
            var fakeRepository = new Mock<CustomerRepository>(fakeHttpHelper.Object);

            // Act
            var result = await fakeRepository.Object.RemoveProductsFromWishList(Guid.Empty, Guid.NewGuid());

            // Assert
            Assert.False(result);
        }


        [Fact]
        public async void AddProductsToWishList_ValidCustomerIdEmptyProductId_ReturnsFalse()
        {
            // Arrange
            var fakeFactory = new Mock<IHttpClientFactory>();
            var fakeHttpHelper = new Mock<HttpHelper>(fakeFactory.Object);
            var fakeRepository = new Mock<CustomerRepository>(fakeHttpHelper.Object);

            // Act
            var result = await fakeRepository.Object.RemoveProductsFromWishList(Guid.NewGuid(), Guid.Empty);

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

            // Act
            var result = await fakeRepository.Object.RemoveProductsFromWishList(Guid.NewGuid(), Guid.NewGuid());

            // Assert
            Assert.True(result);
        }
    }
}
