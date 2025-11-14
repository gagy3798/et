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

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.ImgUri).IsRequired();
            entity.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.Description).IsRequired(false);

            entity.HasData(SeedProducts());
        });
    }

    private static Product[] SeedProducts()
    {
        return
        [
            new Product
            {
                Id = 1,
                Name = "Apple HomePod mini biely",
                ImgUri = "https://image.alza.cz/products/JA041a1/JA041a1.jpg?width=500&height=500",
                Price = 125.9m,
                Description = "Hlasový asistent Siri – v angličtine, kompatibilná aplikácia Apple Home, podpora iOS, pripojenie cez WiFi 2,4 GHz a bluetooth, otvorený systém, fungovanie samostatne, ovládanie domácnosti, kamera, 2 mikrofóny na snímanie okolitého zvuku, podporuje Apple Music, basový reproduktor"
            },
            new Product
            {
                Id = 2,
                Name = "Amazon Echo Spot Glacier White",
                ImgUri = "https://image.alza.cz/products/AME1047/AME1047.jpg?width=500&height=500",
                Price = 83.9m,
                Description = "Hlasový asistent Amazon Alexa – kompatibilný s aplikáciami výrobcu, podpora Android a iOS, pripojenie cez WiFi 2,4 GHz, otvorený systém, fungovanie samostatne, ovládanie domácnosti, tvorba scenárov, displej, hodiny a tlačidlo na odpojenie mikrofónu, dotykové ovládanie"
            },
            new Product
            {
                Id = 3,
                Name = "Google Nest Audio Chalk",
                ImgUri = "https://image.alza.cz/products/GOOGnestA1/GOOGnestA1.jpg?width=500&height=500",
                Price = 92.99m,
                Description = "Hlasový asistent Google Assistant – v angličtine, kompatibilná aplikácia google Home, podpora Android a iOS, pripojenie cez WiFi 2,4 GHz a bluetooth, otvorený systém, fungovanie samostatne, ovládanie domácnosti, 3 mikrofóny na snímanie okolitého zvuku, podporuje Spotify, basový a výškový reproduktor"
            }
        ];
    }
}
