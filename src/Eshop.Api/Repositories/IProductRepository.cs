using Eshop.Api.Models;

namespace Eshop.Api.Repositories;

/// <summary>
/// Repository interface for Product-specific data access operations.
/// </summary>
public interface IProductRepository : IRepository<Product>
{
    /// <summary>
    /// Gets a paginated list of products.
    /// </summary>
    /// <param name="pageNumber">The page number (1-based)</param>
    /// <param name="pageSize">The number of items per page</param>
    /// <returns>A tuple containing the products and total count</returns>
    Task<(IEnumerable<Product> Products, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);

    /// <summary>
    /// Updates the description of a product.
    /// </summary>
    /// <param name="id">The product ID</param>
    /// <param name="description">The new description</param>
    /// <returns>True if the product was found and updated; otherwise, false</returns>
    Task<bool> UpdateDescriptionAsync(int id, string? description);
}
