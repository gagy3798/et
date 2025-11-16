using Eshop.Api.DTOs;

namespace Eshop.Api.Services;

/// <summary>
/// Defines command (write) operations for products.
/// Follows CQRS pattern - separates write operations from read operations.
/// </summary>
public interface IProductCommandService
{
    /// <summary>
    /// Updates the description of an existing product.
    /// </summary>
    /// <param name="id">The product ID</param>
    /// <param name="dto">The DTO containing the new description</param>
    /// <returns>True if the product was found and updated; otherwise, false</returns>
    Task<bool> UpdateDescriptionAsync(int id, UpdateProductDescriptionDto dto);
}
