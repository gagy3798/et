using AutoMapper;
using Eshop.Api.DTOs;
using Eshop.Api.Repositories;

namespace Eshop.Api.Services;

/// <summary>
/// Implements the business logic for managing products.
/// </summary>
public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetProductDto>> GetProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<GetProductDto>>(products);
    }

    public async Task<GetProductDto?> GetProductByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return _mapper.Map<GetProductDto?>(product);
    }

    public async Task<bool> UpdateProductDescriptionAsync(int id, UpdateProductDescriptionDto dto)
    {
        return await _productRepository.UpdateDescriptionAsync(id, dto.Description);
    }

    public async Task<PagedResponse<GetProductDto>> GetProductsPaginatedAsync(int pageNumber, int pageSize)
    {
        var (products, totalCount) = await _productRepository.GetPagedAsync(pageNumber, pageSize);
        var productDtos = _mapper.Map<List<GetProductDto>>(products);

        return new PagedResponse<GetProductDto>(productDtos, pageNumber, pageSize, totalCount);
    }
}