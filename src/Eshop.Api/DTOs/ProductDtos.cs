﻿using System.ComponentModel.DataAnnotations;

namespace Eshop.Api.DTOs;

/// <summary>
/// DTO for displaying a product. This is a public API contract.
/// </summary>
public record GetProductDto(
    [property: Required]
    int Id,
    [property: Required]
    [property: StringLength(100)]
    string Name,
    [property: Required]
    [property: StringLength(500)]
    string ImgUri,
    [property: Required]
    decimal Price,
    [property: StringLength(4000)]
    string? Description
);

/// <summary>
/// DTO for updating a product's description.
/// </summary>
public record UpdateProductDescriptionDto(
    [property: StringLength(4000)]
    string? Description
);