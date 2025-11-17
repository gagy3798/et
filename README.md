# E-shop Product Management API

REST API service for managing e-shop products, built with .NET 10 and VS Code 1.106.0

## Table of Contents

- [About](#about)
- [Technologies](#technologies)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Running the Application](#running-the-application)
- [Running Tests](#running-tests)
- [API Documentation](#api-documentation)
- [Project Structure](#project-structure)
- [Database](#database)

## About

This project provides a RESTful API for managing products in an e-shop. It includes requested API endpoints,  API versioning and tests.

The solution demonstrates:
- Layered architecture (Controllers → Services → Repositories → Data)
- Swagger documentation of all endpoints
- API versioning using URL segment versioning
- Unit and integration testing

## Technologies

- **.NET 10.0** - Latest .NET framework
- **ASP.NET Core Web API** - RESTful API framework
- **Entity Framework Core 10.0** - ORM for database access
- **SQL Server (LocalDB)** - Database
- **Swagger/OpenAPI** - API documentation
- **Asp.Versioning** - API versioning support
- **xUnit** - Testing framework
- **Moq** - Mocking framework

## Prerequisites

Before running the application, ensure you have the following installed:

1. **[.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)** or later
2. **SQL Server** 
3. **[Visual Studio Code 1.106.0](https://code.visualstudio.com/download)** or later
4. **C# Dev Kit for Visual Studio Code** extension
5. **Git** (for cloning the repository)

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/gagy3798/et
cd et
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Update Database Connection Strings

The default connection string uses SQL Server LocalDB.

**`src/Eshop.Api/appsettings.Development.json`**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=EshopDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

**`tests/Eshop.Tests/Infrastructure/CustomWebApplicationFactory.cs`**

Connection string for integration tests in the `CustomWebApplicationFactory.cs`

```csharp
private string ConnectionString =>
    $"Server=(localdb)\\MSSQLLocalDB;Database={_dbName};Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
```

**Note:** Integration tests automatically create a unique test database for each test run (e.g., `EshopTestDb_<guid>`) and clean it up after tests complete.

### 4. Apply Database Migrations

In development environment will be database created automatically.

In other environments run this command:

```bash
cd src/Eshop.Api
dotnet ef database update
```

This will create the database and seed it with initial product data.

## Running the Application

### Option 1: Using .NET CLI

```bash
# From the repository root
cd src/Eshop.Api
dotnet run
```

The API will start at:
- HTTPS: `https://localhost:7257`
- HTTP: `http://localhost:5038`

**Note:** By default, the application runs with HTTPS enabled. If you see a warning about HTTPS redirection, make sure the HTTPS profile is being used (it is now the default profile).

### Option 2: Using Visual Studio Code

1. Open the repository folder in VS Code
2. Make sure you have the **C# Dev Kit** extension installed
3. Press `F5` to start debugging
4. Select `.NET Core Launch (web)` configuration
5. The browser will automatically open Swagger UI at `https://localhost:7257/swagger`

**Note:** The `.vscode` folder contains pre-configured tasks and launch configurations for easy development.

### Access Swagger Documentation

Once the application is running, open your browser and navigate to:

```
https://localhost:7257/swagger
```

You'll see the interactive Swagger UI with all available endpoints for both API versions (v1 and v2).

## Running Tests

The solution includes comprehensive unit and integration tests.

### Run All Tests

```bash
# From the repository root
dotnet test
```

### Run Tests with Code Coverage

**Prerequisites**

Install the ReportGenerator global tool (only needed once):

```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
```

**Generate code coverage report:**

```bash
# Run tests with coverage collection
dotnet test --collect:"XPlat Code Coverage" --results-directory ./tests/Eshop.Tests/TestResults

# Generate HTML report
reportgenerator -reports:"./tests/Eshop.Tests/TestResults/**/coverage.cobertura.xml" -targetdir:"./tests/Eshop.Tests/TestResults/CoverageReport" -reporttypes:Html

# Open the report (Windows)
start ./tests/Eshop.Tests/TestResults/CoverageReport/index.html
```

### Test Structure

- **Unit Tests** (`tests/Eshop.Tests/Controllers/`)
  - `ProductsControllerTests.cs` - Tests for v1 API
  - `ProductsV2ControllerTests.cs` - Tests for v2 API with pagination

- **Integration Tests** (`tests/Eshop.Tests/Integration/`)
  - `ProductsIntegrationTests.cs` - End-to-end tests with real database
  - `ProductQueryServiceTests.cs` - Service layer integration tests

- **Mock Data** (`tests/Eshop.Tests/MockData/`)
  - `ProductMockData.cs` - Centralized mock data used by both unit and integration tests

## API Documentation

### API Endpoints

#### Version 1 (v1)

| Method | Endpoint | Description | Response |
|--------|----------|-------------|----------|
| GET | `/api/v1/products` | Get all products | `PagedResponse<GetProductDto>` |
| GET | `/api/v1/products/{id}` | Get product by ID | `GetProductDto` |
| PATCH | `/api/v1/products/{id}/description` | Update product description | `204 No Content` |

#### Version 2 (v2)

| Method | Endpoint | Description | Query Parameters | Response |
|--------|----------|-------------|------------------|----------|
| GET | `/api/v2/products` | Get paginated products | `pageNumber` (default: 1)<br>`pageSize` (default: 10) | `PagedResponse<GetProductDto>` |


## Project Structure

```
et/
├── src/
│   └── Eshop.Api/                    # Main Web API project
│       ├── Controllers/              # API Controllers (v1, v2)
│       ├── Services/                 # Business logic layer (CQRS)
│       ├── Repositories/             # Data access layer
│       ├── Data/                     # DbContext and database seeding
│       ├── Models/                   # Domain entities
│       ├── DTOs/                     # Data Transfer Objects
│       ├── Mapping/                  # AutoMapper profiles
│       ├── Middleware/               # Custom middleware
│       └── Migrations/               # EF Core migrations
│
├── tests/
│   └── Eshop.Tests/                  # Test project
│       ├── Controllers/              # Controller unit tests
│       ├── Integration/              # Integration tests
│       ├── Infrastructure/           # Test infrastructure
│       └── MockData/                 # Shared test data
│
└── EshopProductSolution.sln          # Solution file
```

### Architecture Layers

1. **Controllers** - HTTP request handling, validation
2. **Services** - Business logic (CQRS: Query/Command separation)
3. **Repositories** - Data access abstraction
4. **Data** - Entity Framework DbContext and database operations

## Database

### Database Schema

**Products Table**

| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | PRIMARY KEY, IDENTITY |
| Name | nvarchar(100) | NOT NULL |
| ImgUri | nvarchar(500) | NOT NULL |
| Price | decimal(18,2) | NOT NULL |
| Description | nvarchar(10000) | NULL |

### Seeded Data

The database is automatically seeded with 3 sample products on first run:
- Apple HomePod mini biely
- Amazon Echo Spot Glacier White
- Google Nest Audio Chalk

### Database Migrations

```bash
# Create a new migration
dotnet ef migrations add MigrationName --project src/Eshop.Api

# Update database to latest migration
dotnet ef database update --project src/Eshop.Api

# Rollback to specific migration
dotnet ef database update PreviousMigrationName --project src/Eshop.Api
```

## Development

### Adding a New Endpoint

1. Add method to appropriate controller (`ProductsController` or `ProductsV2Controller`)
2. Implement business logic in service layer (`IProductQueryService` or `IProductCommandService`)
3. Add repository method if needed (`IProductRepository`)
4. Create/update DTOs if needed
5. Add AutoMapper mapping if needed
6. Write unit tests
7. Write integration tests
8. Update Swagger documentation (XML comments)

