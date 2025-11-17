using Eshop.Api.DTOs;
using Eshop.Api.Models;

namespace Eshop.Tests.MockData;

/// <summary>
/// Provides mock data for Product-related unit tests.
/// </summary>
public static class ProductMockData
{
    #region Product Entity Mock Data

    /// <summary>
    /// Gets a single valid product for testing.
    /// </summary>
    public static Product GetValidProduct() => new()
    {
        Id = 1,
        Name = "Test Product",
        ImgUri = "https://example.com/product.jpg",
        Price = 99.99m,
        Description = "Test product description"
    };

    /// <summary>
    /// Gets a product without a description.
    /// </summary>
    public static Product GetProductWithoutDescription() => new()
    {
        Id = 2,
        Name = "Product Without Description",
        ImgUri = "https://example.com/product2.jpg",
        Price = 49.99m,
        Description = null
    };

    /// <summary>
    /// Gets a product with minimum price.
    /// </summary>
    public static Product GetProductWithMinPrice() => new()
    {
        Id = 3,
        Name = "Cheap Product",
        ImgUri = "https://example.com/cheap.jpg",
        Price = 0.01m,
        Description = "Minimum price product"
    };

    /// <summary>
    /// Gets a product with maximum price.
    /// </summary>
    public static Product GetProductWithMaxPrice() => new()
    {
        Id = 4,
        Name = "Expensive Product",
        ImgUri = "https://example.com/expensive.jpg",
        Price = 99999.99m,
        Description = "Maximum price product"
    };

    /// <summary>
    /// Gets a product with long name (at max length of 100 chars).
    /// </summary>
    public static Product GetProductWithLongName() => new()
    {
        Id = 5,
        Name = new string('A', 100), // 100 characters
        ImgUri = "https://example.com/long-name.jpg",
        Price = 29.99m,
        Description = "Product with maximum length name"
    };

    /// <summary>
    /// Gets a product with long description (at max length of 4000 chars).
    /// </summary>
    public static Product GetProductWithLongDescription() => new()
    {
        Id = 6,
        Name = "Product with Long Description",
        ImgUri = "https://example.com/long-desc.jpg",
        Price = 59.99m,
        Description = new string('B', 4000) // 4000 characters
    };

    /// <summary>
    /// Gets a list of multiple products for testing collections.
    /// </summary>
    public static List<Product> GetProductList() =>
    [
        new Product
        {
            Id = 1,
            Name = "Apple HomePod mini white",
            ImgUri = "https://example.com/homepod.jpg",
            Price = 125.9m,
            Description = "Voice assistant with Siri support"
        },
        new Product
        {
            Id = 2,
            Name = "Amazon Echo Spot",
            ImgUri = "https://example.com/echo.jpg",
            Price = 83.9m,
            Description = "Smart speaker with Alexa"
        },
        new Product
        {
            Id = 3,
            Name = "Google Nest Audio",
            ImgUri = "https://example.com/nest.jpg",
            Price = 92.99m,
            Description = "Smart speaker with Google Assistant"
        },
        new Product
        {
            Id = 4,
            Name = "Budget Speaker",
            ImgUri = "https://example.com/budget.jpg",
            Price = 19.99m,
            Description = null
        },
        new Product
        {
            Id = 5,
            Name = "Premium Audio System",
            ImgUri = "https://example.com/premium.jpg",
            Price = 499.99m,
            Description = "High-end audio system with premium sound quality"
        }
    ];

    /// <summary>
    /// Gets an empty list of products.
    /// </summary>
    public static List<Product> GetEmptyProductList() => [];

    #endregion

    #region Product DTO Mock Data

    /// <summary>
    /// Gets a valid GetProductDto for testing.
    /// </summary>
    public static GetProductDto GetValidProductDto() => new(
        Id: 1,
        Name: "Test Product",
        ImgUri: "https://example.com/product.jpg",
        Price: 99.99m,
        Description: "Test product description"
    );

    /// <summary>
    /// Gets a GetProductDto without description.
    /// </summary>
    public static GetProductDto GetProductDtoWithoutDescription() => new(
        Id: 2,
        Name: "Product Without Description",
        ImgUri: "https://example.com/product2.jpg",
        Price: 49.99m,
        Description: null
    );

    /// <summary>
    /// Gets a list of GetProductDto objects.
    /// </summary>
    public static List<GetProductDto> GetProductDtoList() =>
    [
        new GetProductDto(1, "Apple HomePod mini white", "https://example.com/homepod.jpg", 125.9m, "Voice assistant with Siri support"),
        new GetProductDto(2, "Amazon Echo Spot", "https://example.com/echo.jpg", 83.9m, "Smart speaker with Alexa"),
        new GetProductDto(3, "Google Nest Audio", "https://example.com/nest.jpg", 92.99m, "Smart speaker with Google Assistant")
    ];

    /// <summary>
    /// Gets a valid UpdateProductDescriptionDto.
    /// </summary>
    public static UpdateProductDescriptionDto GetValidUpdateDescriptionDto() => new(
        Description: "Updated product description"
    );

    /// <summary>
    /// Gets an UpdateProductDescriptionDto with null description (for clearing description).
    /// </summary>
    public static UpdateProductDescriptionDto GetNullUpdateDescriptionDto() => new(
        Description: null
    );

    /// <summary>
    /// Gets an UpdateProductDescriptionDto with empty description.
    /// </summary>
    public static UpdateProductDescriptionDto GetEmptyUpdateDescriptionDto() => new(
        Description: string.Empty
    );

    /// <summary>
    /// Gets an UpdateProductDescriptionDto with long description (at max length).
    /// </summary>
    public static UpdateProductDescriptionDto GetLongUpdateDescriptionDto() => new(
        Description: new string('C', 4000) // 4000 characters
    );

    #endregion

    #region Helper Methods

    /// <summary>
    /// Creates a custom product with specified properties.
    /// </summary>
    public static Product CreateProduct(
        int id,
        string name = "Test Product",
        string imgUri = "https://example.com/default.jpg",
        decimal price = 99.99m,
        string? description = "Default description")
    {
        return new Product
        {
            Id = id,
            Name = name,
            ImgUri = imgUri,
            Price = price,
            Description = description
        };
    }

    /// <summary>
    /// Creates a custom GetProductDto with specified properties.
    /// </summary>
    public static GetProductDto CreateProductDto(
        int id,
        string name = "Test Product",
        string imgUri = "https://example.com/default.jpg",
        decimal price = 99.99m,
        string? description = "Default description")
    {
        return new GetProductDto(id, name, imgUri, price, description);
    }

    /// <summary>
    /// Creates a list of products with specified count.
    /// </summary>
    public static List<Product> CreateProductList(int count)
    {
        var products = new List<Product>();
        for (int i = 1; i <= count; i++)
        {
            products.Add(CreateProduct(
                id: i,
                name: $"Product {i}",
                imgUri: $"https://example.com/product{i}.jpg",
                price: 10m * i,
                description: $"Description for product {i}"
            ));
        }
        return products;
    }

    /// <summary>
    /// Creates a PagedResponse for testing pagination.
    /// </summary>
    public static PagedResponse<GetProductDto> CreatePagedResponse(
        int pageNumber = 1,
        int pageSize = 10,
        int totalCount = 50)
    {
        var items = GetProductDtoList()
            .Take(pageSize)
            .ToList();

        return new PagedResponse<GetProductDto>(
            items,
            pageNumber,
            pageSize,
            totalCount
        );
    }

    #endregion

    #region Edge Cases

    /// <summary>
    /// Gets a product with special characters in the name.
    /// </summary>
    public static Product GetProductWithSpecialCharactersInName() => new()
    {
        Id = 100,
        Name = "Product & Co. - \"Special\" Edition (50% Off!)",
        ImgUri = "https://example.com/special.jpg",
        Price = 39.99m,
        Description = "Product with special characters: & < > \" ' % #"
    };

    /// <summary>
    /// Gets a product with Unicode characters.
    /// </summary>
    public static Product GetProductWithUnicodeCharacters() => new()
    {
        Id = 101,
        Name = "Produktový názov v slovenčine",
        ImgUri = "https://example.com/unicode.jpg",
        Price = 45.50m,
        Description = "Popis s diakritikou: áäčďéíľĺňóôŕšťúýž ÁÄČĎÉÍĽĹŇÓÔŔŠŤÚÝŽ"
    };

    /// <summary>
    /// Gets a product with zero price (free item).
    /// </summary>
    public static Product GetFreeProduct() => new()
    {
        Id = 102,
        Name = "Free Sample",
        ImgUri = "https://example.com/free.jpg",
        Price = 0m,
        Description = "Free promotional item"
    };

    #endregion
}
