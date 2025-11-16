using Eshop.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Api.Data;

public class EshopDbContext : DbContext
{
    public EshopDbContext(DbContextOptions<EshopDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Product entity
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ImgUri).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.Description).IsRequired(false).HasMaxLength(10000);
        });

        // Seed initial data
        DatabaseSeeder.SeedData(modelBuilder);
    }
}
