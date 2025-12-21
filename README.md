# Clean dotnet10

Hands-on .NET 10 Clean Architecture sample using a Restaurants API.

## Description
This repo follows a course-style path for building scalable REST APIs with ASP.NET Core 10,
Clean Architecture, EF Core, and Azure. It covers REST fundamentals, routing and model binding,
database setup and seeding, CRUD workflows, DTO mapping and validation, CQRS/MediatR,
logging and OpenAPI docs, global exception handling, auth, pagination/sorting,
testing, and Azure deployment with CI/CD.

I learned from this course:
https://www.udemy.com/course/aspnet-core-web-api-clean-architecture-azure/

## Tech stack
- .NET 10
- ASP.NET Core (Controllers)
- EF Core + SQL Server (Azure SQL Edge for Apple Silicon)
- Clean Architecture: Domain / Application / Infrastructure / API

## Solution structure
```
Restaurants.API             # API entry point
Restaurants.Application     # Use cases / services
Restaurants.Domain          # Entities + domain contracts
Restaurants.Infrastructure  # EF Core, repositories, persistence
```

## Prerequisites
- .NET SDK 10.x
- Docker Desktop (for Azure SQL Edge)

## Database (Apple Silicon friendly)
Run SQL Server compatible engine:
```bash
docker run --name sql-edge -e "ACCEPT_EULA=Y" -e 'MSSQL_SA_PASSWORD=Strong\!Passw0rd1' \
  -p 1433:1433 -d mcr.microsoft.com/azure-sql-edge
```

## Configure connection string
Use User Secrets (recommended for local dev):
```bash
cd Restaurants.API
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" \
  "Server=localhost,1433;Database=RestaurantsDb;User Id=sa;Password=Strong!Passw0rd1;TrustServerCertificate=True;Encrypt=False"
```

Or use env var:
```bash
export ConnectionStrings__DefaultConnection="Server=localhost,1433;Database=RestaurantsDb;User Id=sa;Password=Strong!Passw0rd1;TrustServerCertificate=True;Encrypt=False"
```

## Migrations
```bash
dotnet ef migrations add InitialCreate -p Restaurants.Infrastructure -s Restaurants.API
dotnet ef database update -p Restaurants.Infrastructure -s Restaurants.API
```

## Run the API
```bash
dotnet run --project Restaurants.API
```

## Endpoints
- `GET /api/restaurants`
- `GET /api/restaurants/{id}`

## Seeding
Sample data is seeded only in Development environment when the database is empty.

## Commit message format
This repo uses a Conventional Commits style template (see `.gitmessage`).
