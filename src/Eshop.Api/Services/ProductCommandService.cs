using Eshop.Api.DTOs;
using Eshop.Api.Repositories;

namespace Eshop.Api.Services;

/// <summary>
/// Implements command (write) operations for products.
/// </summary>
public class ProductCommandService : IProductCommandService
{
    private readonly IProductRepository _productRepository;

    public ProductCommandService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<bool> UpdateDescriptionAsync(int id, UpdateProductDescriptionDto dto)
    {
        return await _productRepository.UpdateDescriptionAsync(id, dto.Description);
    }
}
