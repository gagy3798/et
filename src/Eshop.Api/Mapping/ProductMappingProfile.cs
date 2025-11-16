using AutoMapper;
using Eshop.Api.DTOs;
using Eshop.Api.Models;

namespace Eshop.Api.Mapping;

/// <summary>
/// AutoMapper profile for Product entity mappings.
/// </summary>
public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        // Map from Product entity to GetProductDto
        CreateMap<Product, GetProductDto>();

        // If you need reverse mapping in the future, uncomment:
        // CreateMap<GetProductDto, Product>();
    }
}
