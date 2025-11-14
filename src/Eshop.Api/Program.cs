using Eshop.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

// Add DbContext
builder.Services.AddDbContext<EshopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // migrate db on app start in development environment
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<EshopDbContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
