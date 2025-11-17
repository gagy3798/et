using Eshop.Api.Controllers;
using Eshop.Api.DTOs;
using Eshop.Api.Services;
using Eshop.Tests.MockData;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using FluentAssertions;
using Moq;

namespace Eshop.Tests.Controllers;

/// <summary>
/// Unit tests for the ProductsController.
/// </summary>
public class ProductsControllerTests
{
    private readonly Mock<IProductQueryService> _queryServiceMock;
    private readonly Mock<IProductCommandService> _commandServiceMock;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _queryServiceMock = new Mock<IProductQueryService>();
        _commandServiceMock = new Mock<IProductCommandService>();
        _controller = new ProductsController(_queryServiceMock.Object, _commandServiceMock.Object);
    }

    [Fact]
    public async Task GetProducts_ShouldReturnOk_WithAllProducts()
    {
        // Arrange
        var pagedResponse = ProductMockData.CreatePagedResponse(1, int.MaxValue, 5);
        _queryServiceMock.Setup(s => s.GetProductsAsync(1, int.MaxValue)).ReturnsAsync(pagedResponse);

        // Act
        var result = await _controller.GetProducts();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedData = okResult.Value.Should().BeOfType<PagedResponse<GetProductDto>>().Subject;
        returnedData.Should().BeEquivalentTo(pagedResponse);
        _queryServiceMock.Verify(s => s.GetProductsAsync(1, int.MaxValue), Times.Once);
    }

    [Fact]
    public async Task GetProduct_WhenProductExists_ShouldReturnOk_WithProduct()
    {
        // Arrange
        var productDto = ProductMockData.GetValidProductDto();
        _queryServiceMock.Setup(s => s.GetByIdAsync(productDto.Id)).ReturnsAsync(productDto);

        // Act
        var result = await _controller.GetProduct(productDto.Id);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(productDto);
        _queryServiceMock.Verify(s => s.GetByIdAsync(productDto.Id), Times.Once);
    }

    [Fact]
    public async Task GetProduct_WhenProductDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        _queryServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((GetProductDto?)null);

        // Act
        var result = await _controller.GetProduct(999);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task UpdateProductDescription_WhenUpdateSucceeds_ShouldReturnNoContent()
    {
        // Arrange
        var productId = 1;
        var updateDto = ProductMockData.GetValidUpdateDescriptionDto();
        _commandServiceMock.Setup(s => s.UpdateDescriptionAsync(productId, updateDto)).ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateProductDescription(productId, updateDto);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _commandServiceMock.Verify(s => s.UpdateDescriptionAsync(productId, updateDto), Times.Once);
    }

    [Fact]
    public async Task UpdateProductDescription_WhenProductNotFound_ShouldReturnNotFound()
    {
        // Arrange
        var productId = 999;
        var updateDto = ProductMockData.GetValidUpdateDescriptionDto();
        _commandServiceMock.Setup(s => s.UpdateDescriptionAsync(productId, updateDto)).ReturnsAsync(false);

        // Act
        var result = await _controller.UpdateProductDescription(productId, updateDto);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}