using AutoMapper;
using Eshop.Api.DTOs;
using Eshop.Api.Repositories;

namespace Eshop.Api.Services;

/// <summary>
/// Implements query (read) operations for products.
/// </summary>
public class ProductQueryService : IProductQueryService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductQueryService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<GetProductDto?> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return _mapper.Map<GetProductDto?>(product);
    }

    public async Task<PagedResponse<GetProductDto>> GetProductsAsync(int pageNumber = 1, int pageSize = int.MaxValue)
    {
        var (products, totalCount) = await _productRepository.GetPagedAsync(pageNumber, pageSize);
        var productDtos = _mapper.Map<List<GetProductDto>>(products);

        return new PagedResponse<GetProductDto>(productDtos, pageNumber, pageSize, totalCount);
    }
}
