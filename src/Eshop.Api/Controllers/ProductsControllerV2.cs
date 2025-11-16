using Eshop.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Eshop.Api.Services;
using System.ComponentModel.DataAnnotations;

namespace Eshop.Api.Controllers;

[ApiVersion("2.0")]
[ApiController]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/products")]
public class ProductsV2Controller : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsV2Controller> _logger;
    private const int MaxPageSize = 100;

    public ProductsV2Controller(IProductService productService, ILogger<ProductsV2Controller> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    /// <summary>
    /// Gets a paginated list of products.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A paginated list of products.</returns>
    /// <response code="200">Returns the paginated list of products.</response>
    /// <response code="400">If the paging parameters are invalid.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpGet(Name = "GetProductsV2")]
    [ProducesResponseType(typeof(PagedResponse<GetProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedResponse<GetProductDto>>> GetProductsV2(
        [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1,
        [FromQuery, Range(1, MaxPageSize)] int pageSize = 10)
    {
        return Ok(await _productService.GetProductsPaginatedAsync(pageNumber, pageSize));
    }
}
