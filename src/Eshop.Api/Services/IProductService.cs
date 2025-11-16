using Eshop.Api.DTOs;

namespace Eshop.Api.Services;

/// <summary>
/// Defines the business logic operations for products.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Gets a list of all products.
    /// </summary>
    Task<IEnumerable<GetProductDto>> GetProductsAsync();

    /// <summary>
    /// Gets a specific product by its ID.
    /// </summary>
    Task<GetProductDto?> GetProductByIdAsync(int id);

    /// <summary>
    /// Updates the description of an existing product.
    /// </summary>
    /// <returns>True if the product was found and updated; otherwise, false.</returns>
    Task<bool> UpdateProductDescriptionAsync(int id, UpdateProductDescriptionDto dto);

    /// <summary>
    /// Gets a paginated list of products.
    /// </summary>
    Task<PagedResponse<GetProductDto>> GetProductsPaginatedAsync(int pageNumber, int pageSize);
}