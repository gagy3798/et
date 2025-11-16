using Eshop.Api.Data;
using Eshop.Api.Models;
using Eshop.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp.Versioning;

namespace Eshop.Api.Controllers;

[ApiVersion("2.0")]
[ApiController]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/products")]
public class ProductsV2Controller : ControllerBase
{
    private readonly EshopDbContext _db;
    private readonly ILogger<ProductsV2Controller> _logger;

    public ProductsV2Controller(EshopDbContext db, ILogger<ProductsV2Controller> logger)
    {
        _db = db;
        _logger = logger;
    }

    /// <summary>
    /// Gets a paginated list of products.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A paginated list of products.</returns>
    /// <response code="200">Returns the paginated list of products.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpGet(Name = "GetProductsV2")]
    [ProducesResponseType(typeof(PagedResponse<GetProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedResponse<GetProductDto>>> GetProductsV2([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var totalCount = await _db.Products.CountAsync();

            var products = await _db.Products
                .OrderBy(p => p.Id) // Consistent ordering is important for pagination
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new GetProductDto(p.Id, p.Name, p.ImgUri, p.Price, p.Description))
                .ToListAsync();

            var pagedResponse = new PagedResponse<GetProductDto>(products, pageNumber, pageSize, totalCount);

            return Ok(pagedResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving products for page {PageNumber} with page size {PageSize}.", pageNumber, pageSize);
            return StatusCode(StatusCodes.Status500InternalServerError, "An internal server error occurred. Please try again later.");
        }
    }
}
