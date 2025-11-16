using Eshop.Api.DTOs;

namespace Eshop.Api.Services;

/// <summary>
/// Defines query (read) operations for products.
/// Follows CQRS pattern - separates read operations from write operations.
/// </summary>
public interface IProductQueryService
{
    /// <summary>
    /// Gets a specific product by its ID.
    /// </summary>
    /// <param name="id">The product ID</param>
    /// <returns>The product if found; otherwise, null</returns>
    Task<GetProductDto?> GetByIdAsync(int id);

    /// <summary>
    /// Gets a paginated list of products.
    /// Use pageSize = int.MaxValue to get all products (for backwards compatibility).
    /// </summary>
    /// <param name="pageNumber">The page number (1-based). Default is 1.</param>
    /// <param name="pageSize">The number of items per page. Default is int.MaxValue (all items).</param>
    /// <returns>A paginated response containing products</returns>
    Task<PagedResponse<GetProductDto>> GetProductsAsync(int pageNumber = 1, int pageSize = int.MaxValue);
}
