namespace Eshop.Api.DTOs;

/// <summary>
/// DTO for displaying a product. This is a public API contract.
/// </summary>
public record GetProductDto(
    int Id,
    string Name,
    string ImgUri,
    decimal Price,
    string? Description);

/// <summary>
/// DTO for updating a product's description.
/// </summary>
public record UpdateProductDescriptionDto(string? Description);