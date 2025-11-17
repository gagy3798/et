using Eshop.Api.Data;
using Eshop.Tests.MockData;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Tests.Infrastructure;

/// <summary>
/// Seeds test data into the database using ProductMockData.
/// This allows EASY SWITCHING between mock data and DB data:
/// - Unit tests use ProductMockData directly with mocks
/// - Integration tests use ProductMockData to seed the in-memory database
/// </summary>
public static class TestDataSeeder
{
    /// <summary>
    /// Seeds the database with test data from ProductMockData.
    /// This ensures the same data is used for both unit and integration tests.
    /// </summary>
    /// <param name="context">The database context to seed</param>
    public static void SeedData(EshopDbContext context)
    {
        // Clear existing data to ensure clean state
        context.Products.RemoveRange(context.Products);
        context.SaveChanges();

        // Get products from mock data
        var products = ProductMockData.GetProductList();

        foreach (var product in products)
        {
            context.Database.ExecuteSqlRaw(
                @"SET IDENTITY_INSERT Products ON;
                    INSERT INTO Products (Id, Name, Description, ImgUri, Price)
                    VALUES ({0}, {1}, {2}, {3}, {4});
                    SET IDENTITY_INSERT Products OFF;",
                product.Id,
                product.Name,
                product.Description,
                product.ImgUri,
                product.Price);
        }
    }

    /// <summary>
    /// Clears all test data from the database.
    /// </summary>
    /// <param name="context">The database context to clear</param>
    public static void ClearData(EshopDbContext context)
    {
        context.Products.RemoveRange(context.Products);
        context.SaveChanges();
    }
}
