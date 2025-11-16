using Eshop.Api.Data;
using Eshop.Api.Models;
using Eshop.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp.Versioning;

namespace Eshop.Api.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/products")]
public class ProductsController : ControllerBase
{
    private readonly EshopDbContext _db;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(EshopDbContext db, ILogger<ProductsController> logger)
    {
        _db = db;
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
        try
        {
            var products = await _db.Products
                .Select(p => new GetProductDto(p.Id, p.Name, p.ImgUri, p.Price, p.Description))
                .ToListAsync();
            
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving products.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An internal server error occurred. Please try again later.");
        }
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
        try
        {
            var product = await _db.Products
                .Where(p => p.Id == id)
                .Select(p => new GetProductDto(p.Id, p.Name, p.ImgUri, p.Price, p.Description))
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving product with ID {ProductId}.", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An internal server error occurred. Please try again later.");
        }
    }

    /// <summary>
    /// Updates the description of an existing product.
    /// </summary>
    /// <param name="id">The ID of the product to update.</param>
    /// <param name="dto">An object containing the new product description.</param>
    /// <response code="204">If the description was updated successfully.</response>
    /// <response code="404">If the product with the given ID does not exist.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpPatch("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateProductDescription(int id, [FromBody] UpdateProductDescriptionDto dto)
    {
        try
        {
            // Using ExecuteUpdateAsync for an efficient partial update without loading the entire entity.
            var rowsAffected = await _db.Products
                .Where(p => p.Id == id)
                .ExecuteUpdateAsync(updates =>
                    updates.SetProperty(p => p.Description, dto.Description));

            return rowsAffected == 0 ? NotFound() : NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating product description for ID {ProductId}.", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An internal server error occurred. Please try again later.");
        }
    }
}
