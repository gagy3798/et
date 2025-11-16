using Microsoft.EntityFrameworkCore;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Eshop.Api;
using Eshop.Api.Data;
using Eshop.Api.Middleware;
using Eshop.Api.Repositories;
using Eshop.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add API Versioning services and configure them
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Add Swagger services and configure them to work with API versioning
builder.Services.AddTransient<Microsoft.Extensions.Options.IConfigureOptions<Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(options =>
{
    // Enable XML comments if available for richer Swagger documentation
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// Add DbContext
builder.Services.AddDbContext<EshopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Register repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Register CQRS services (Query and Command separation)
builder.Services.AddScoped<IProductQueryService, ProductQueryService>();
builder.Services.AddScoped<IProductCommandService, ProductCommandService>();

var app = builder.Build();

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Add the global exception handler middleware at the beginning of the pipeline.
app.UseMiddleware<GlobalExceptionHandler>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // Build a swagger endpoint for each discovered API version
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });

    // migrate db on app start in development environment
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<EshopDbContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
