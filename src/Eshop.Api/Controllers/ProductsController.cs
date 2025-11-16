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
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    /// <summary>
    /// Gets a list of all products.
    /// </summary>
    /// <returns>A list of products.</returns>
    /// <response code="200">Returns the list of products.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<GetProductDto>>> GetProducts()
    {
        // The try-catch block is no longer needed, the middleware will handle exceptions.
        var products = await _productService.GetProductsAsync();
        return Ok(products);
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
        var product = await _productService.GetProductByIdAsync(id);
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
    [HttpPut("{id:int}/description")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateProductDescription(int id, [FromBody] UpdateProductDescriptionDto dto)
    {
        var wasUpdated = await _productService.UpdateProductDescriptionAsync(id, dto);
        return wasUpdated ? NoContent() : NotFound();
    }
}
