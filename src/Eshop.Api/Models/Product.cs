namespace Eshop.Api.Models;

/// <summary>
/// Represents a product in the e-shop.
/// </summary>
public record Product
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public required string ImgUri { get; init; }
    public required decimal Price { get; init; }
    public string? Description { get; init; }
}
