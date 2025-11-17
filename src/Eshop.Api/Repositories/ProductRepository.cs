using Eshop.Api.Data;
using Eshop.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Api.Repositories;

/// <summary>
/// Implementation of the Product repository for data access operations.
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly EshopDbContext _context;

    public ProductRepository(EshopDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product> AddAsync(Product entity)
    {
        var entry = await _context.Products.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task UpdateAsync(Product entity)
    {
        _context.Products.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Product entity)
    {
        _context.Products.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<(IEnumerable<Product> Products, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
    {
        var totalCount = await _context.Products.CountAsync();

        var products = await _context.Products
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (products, totalCount);
    }

    public async Task<bool> UpdateDescriptionAsync(int id, string? description)
    {
        // Find the product first using AsNoTracking to avoid tracking conflicts
        var product = await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return false;
        }

        // Update the description
        // Since Product is a record, we need to create a new instance with updated values
        var updatedProduct = product with { Description = description };

        _context.Products.Update(updatedProduct);
        await _context.SaveChangesAsync();

        return true;
    }
}
