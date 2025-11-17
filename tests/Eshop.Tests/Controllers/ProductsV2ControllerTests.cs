using Eshop.Api.Controllers;
using Eshop.Api.DTOs;
using Eshop.Api.Services;
using Eshop.Tests.MockData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Eshop.Tests.Controllers
{
    public class ProductsV2ControllerTests
    {
        private readonly Mock<IProductQueryService> _queryServiceMock;
        private readonly ProductsV2Controller _controller;

        public ProductsV2ControllerTests()
        {
            _queryServiceMock = new Mock<IProductQueryService>();
            _controller = new ProductsV2Controller(_queryServiceMock.Object);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnOk_WithAllProducts()
        {
            // Arrange
            var pagedResponse = ProductMockData.CreatePagedResponse(1, 2, 5);
            _queryServiceMock
                .Setup(s => s.GetProductsAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(pagedResponse);

            // Act
            var result = await _controller.GetProductsV2();

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(pagedResponse);
            _queryServiceMock.Verify(s => s.GetProductsAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }
    }
}