# Restaurants API (.NET 10 Clean Architecture)

Hands-on sample REST API for restaurants and dishes built with ASP.NET Core 10.

Based on the Udemy course:
https://www.udemy.com/course/aspnet-core-web-api-clean-architecture-azure/

## Highlights
- Clean Architecture (Domain / Application / Infrastructure / API)
- CQRS with MediatR
- FluentValidation and AutoMapper
- EF Core + SQL Server
- ASP.NET Identity API endpoints
- Serilog logging and Swagger/OpenAPI

## Solution structure
```
Restaurants.API             # API entry point
Restaurants.Application     # Use cases / services
Restaurants.Domain          # Entities + domain contracts
Restaurants.Infrastructure  # EF Core, repositories, persistence
```

## Prerequisites
- .NET SDK 10.x
- SQL Server instance (Docker example below)
- Docker Desktop (for Azure SQL Edge on Apple Silicon)

## Database (Azure SQL Edge on Apple Silicon)
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

Swagger UI is available at `/swagger` in all environments.

## Endpoints
### Restaurants
- `GET /api/restaurants`
- `GET /api/restaurants/{id}`
- `POST /api/restaurants`
- `PATCH /api/restaurants/{id}`
- `DELETE /api/restaurants/{id}`

### Dishes
- `GET /api/restaurants/{restaurantId}/dishes`
- `GET /api/restaurants/{restaurantId}/dishes/{dishId}`
- `POST /api/restaurants/{restaurantId}/dishes`
- `DELETE /api/restaurants/{restaurantId}/dishes`
- `DELETE /api/restaurants/{restaurantId}/dishes/{dishId}`

### Identity
- Endpoints are mapped under `/api/identity` (check Swagger for the full list).

## Example requests
### Create restaurant
Request body:
```json
{
  "name": "Siam Noodles",
  "description": "Thai comfort food and soups.",
  "category": "Thai",
  "hasDelivery": true,
  "contactEmail": "hello@siamnoodles.example",
  "contactNumber": "+66 2 123 4567",
  "city": "Bangkok",
  "street": "Sukhumvit 21",
  "postalCode": "10110"
}
```

Response:
```json
123
```

Notes:
- `category` must be one of: `Italian`, `Mexican`, `Japanese`, `Thai`, `American`, `Indian`.

### Create dish
Request body:
```json
{
  "name": "Pad Thai",
  "description": "Stir-fried rice noodles with tamarind sauce.",
  "price": 12.5,
  "kiloCalories": 620
}
```

Response:
```json
456
```

## Seeding
Sample data is seeded only in Development when the database is empty.

## Commit message format
This repo uses a Conventional Commits style template (see `.gitmessage`).
