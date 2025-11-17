using Eshop.Api.Data;
using Eshop.Api.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Eshop.Tests.Infrastructure;

/// <summary>
/// Custom factory for creating test server instances with real SQL Server database.
/// Uses a separate test database to ensure complete integration testing.
/// </summary>
/// <typeparam name="TProgram">The program type (usually Program from the API project)</typeparam>
public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    private readonly string _dbName = $"EshopTestDb_{Guid.NewGuid()}";
    private string ConnectionString =>
        $"Server=(localdb)\\MSSQLLocalDB;Database={_dbName};Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

    /// <summary>
    /// Creates a new instance of the test factory.
    /// Uses real SQL Server database for comprehensive integration tests.
    /// Each factory instance gets a unique database name to allow parallel test execution.
    /// </summary>
    public CustomWebApplicationFactory()
    {
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Set environment to Testing BEFORE services are configured
        // This prevents SQL Server DbContext from being registered in Program.cs
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // Add SQL Server database for testing
            // This is a REAL database, not in-memory, for complete integration testing
            services.AddDbContext<EshopDbContext>(options =>
            {
                options.UseSqlServer(ConnectionString);
            });
        });
    }

    protected override void Dispose(bool disposing)
    {
        // The base Dispose(bool) method will dispose the host and the service provider.
        // We need to call it first before we try to clean up the database.
        base.Dispose(disposing);

        if (disposing)
        {
            // After the host is disposed, we can safely clean up our test database.
            // We create a new DbContext instance with the same connection string
            // to connect to the database and delete it.
            var options = new DbContextOptionsBuilder<EshopDbContext>()
                .UseSqlServer(ConnectionString)
                .Options;
            using var context = new EshopDbContext(options);
            context.Database.EnsureDeleted();
        }
    }

    /// <summary>
    /// Seeds the database with test data.
    /// Call this method after creating the factory to populate the database.
    /// </summary>
    public void SeedDatabase()
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<EshopDbContext>();

        // Ensure the database is created with full schema (including migrations)
        db.Database.EnsureDeleted(); // Clean start
        db.Database.EnsureCreated();  // Create schema

        // Seed test data using our mock data
        TestDataSeeder.SeedData(db);
    }
}
