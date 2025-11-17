using Eshop.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Eshop.Api.Services;

namespace Eshop.Api.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductQueryService _queryService;
    private readonly IProductCommandService _commandService;

    public ProductsController(IProductQueryService queryService, IProductCommandService commandService)
    {
        _queryService = queryService;
        _commandService = commandService;
    }

    /// <summary>
    /// Gets a list of all products.
    /// </summary>
    /// <returns>A paginated response containing all products.</returns>
    /// <response code="200">Returns the paginated list of products.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<GetProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedResponse<GetProductDto>>> GetProducts()
    {
        // V1 API returns all products by using default pageSize (int.MaxValue)
        var response = await _queryService.GetProductsAsync();
        return Ok(response);
    }

    /// <summary>
    /// Gets a specific product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product.</param>
    /// <returns>The product details.</returns>
    /// <response code="200">Returns the requested product.</response>
    /// <response code="404">If the product with the given ID does not exist.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GetProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetProductDto>> GetProduct(int id)
    {
        var product = await _queryService.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    /// <summary>
    /// Updates the description of an existing product.
    /// </summary>
    /// <param name="id">The ID of the product to update.</param>
    /// <param name="dto">An object containing the new product description.</param>
    /// <response code="204">If the description was updated successfully.</response>
    /// <response code="400">If the request body is invalid.</response>
    /// <response code="404">If the product with the given ID does not exist.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpPatch("{id:int}/description")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateProductDescription(int id, [FromBody] UpdateProductDescriptionDto dto)
    {
        var wasUpdated = await _commandService.UpdateDescriptionAsync(id, dto);
        return wasUpdated ? NoContent() : NotFound();
    }
}
