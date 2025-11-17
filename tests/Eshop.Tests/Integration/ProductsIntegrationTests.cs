using System.Net;
using System.Net.Http.Json;
using Eshop.Api.Data;
using Eshop.Api.DTOs;
using Eshop.Tests.Infrastructure;
using Eshop.Tests.MockData;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Eshop.Tests.Integration;

/// <summary>
/// Integration tests for Products API endpoints.
///
/// KEY FEATURE: Easy switching between mock and DB data
/// - The same ProductMockData is used to seed the database
/// - This ensures consistency between unit tests (using mocks) and integration tests (using DB)
/// </summary>
public class ProductsIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public ProductsIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;

        // Seed the database with test data
        _factory.SeedDatabase();

        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetProducts_V1_ShouldReturnAllProducts_FromDatabase()
    {
        // Act - call the real API endpoint
        var response = await _client.GetAsync("/api/v1/products");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var pagedResponse = await response.Content.ReadFromJsonAsync<PagedResponse<GetProductDto>>();
        pagedResponse.Should().NotBeNull();
        pagedResponse!.Items.Should().HaveCount(5); // ProductMockData.GetProductList() has 5 items
        pagedResponse.TotalCount.Should().Be(5);
    }

    [Fact]
    public async Task GetProduct_WhenProductExists_ShouldReturnProduct_FromDatabase()
    {
        // Arrange - use known ID from ProductMockData
        var productId = 1; // First product from ProductMockData.GetProductList()

        // Act
        var response = await _client.GetAsync($"/api/v1/products/{productId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var product = await response.Content.ReadFromJsonAsync<GetProductDto>();
        product.Should().NotBeNull();
        product!.Id.Should().Be(productId);
        product.Name.Should().Be("Apple HomePod mini white"); // From ProductMockData
    }

    [Fact]
    public async Task GetProduct_WhenProductDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistentId = 999;

        // Act
        var response = await _client.GetAsync($"/api/v1/products/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetProducts_V2_WithPagination_ShouldReturnPagedResults_FromDatabase()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 2;

        // Act
        var response = await _client.GetAsync($"/api/v2/products?pageNumber={pageNumber}&pageSize={pageSize}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var pagedResponse = await response.Content.ReadFromJsonAsync<PagedResponse<GetProductDto>>();
        pagedResponse.Should().NotBeNull();
        pagedResponse!.Items.Should().HaveCount(2); // Requested page size
        pagedResponse.PageNumber.Should().Be(pageNumber);
        pagedResponse.PageSize.Should().Be(pageSize);
        pagedResponse.TotalCount.Should().Be(5); // Total from ProductMockData
    }

    [Fact]
    public async Task UpdateProductDescription_ShouldUpdateInDatabase()
    {
        // Arrange
        var productId = 1;
        var updateDto = new UpdateProductDescriptionDto("Updated description from integration test");

        // Act
        var response = await _client.PutAsJsonAsync($"/api/v1/products/{productId}/description", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify the update by fetching the product
        var getResponse = await _client.GetAsync($"/api/v1/products/{productId}");
        var product = await getResponse.Content.ReadFromJsonAsync<GetProductDto>();
        product!.Description.Should().Be("Updated description from integration test");
    }

    [Fact]    
    public async Task UpdateProductDescription_WhenProductNotFound_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistentId = 999;
        var updateDto = new UpdateProductDescriptionDto("This should fail");

        // Act
        var response = await _client.PutAsJsonAsync($"/api/v1/products/{nonExistentId}/description", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateProductDescription_WhenDescriptionTooLong_ShouldReturnBadRequest()
    {
        // Arrange
        var productId = 1;
        var updateDto = new UpdateProductDescriptionDto(new string('x', 4001));

        // Act
        var response = await _client.PutAsJsonAsync($"/api/v1/products/{productId}/description", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    /// <summary>
    /// Example: Accessing the database directly in a test
    /// This shows how you can manipulate the DB for specific test scenarios
    /// </summary>
    [Fact]
    public async Task DirectDatabaseAccess_Example()
    {
        // Arrange - get the database context
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<EshopDbContext>();

        // Verify seeded data
        var productsInDb = db.Products.ToList();
        productsInDb.Should().HaveCount(5);

        // Act - call API
        var response = await _client.GetAsync("/api/v1/products");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
