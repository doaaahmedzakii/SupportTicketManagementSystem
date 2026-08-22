# Support Ticket Management API

## How to Run the Application

### Prerequisites
* .NET SDK
* SQL Server

### Run the Application
1. Clone the repository.
2. Configure the database connection as described in the **Database Configuration** section.
3. Apply the database migrations.
4. Run the API:
    dotnet run --project SupportTicket.Api

The API can then be accessed through the configured application URL, and Swagger can be used to explore the available endpoints.

## Database Configuration
The application uses **SQL Server** with **Entity Framework Core**.

Update the connection string in:
    SupportTicket.Api/appsettings.json

with the SQL Server instance and database name appropriate for your environment.

Example:
  {
    "ConnectionStrings": {
      "DefaultConnection": "Server=YOUR_SERVER;Database=SupportTicketDb;Trusted_Connection=True;TrustServerCertificate=True;"
    }
  }

## How to Run Migrations
From the **Visual Studio Package Manager Console**, run:  
    Update-Database

This applies the existing Entity Framework Core migrations and creates or updates the database schema.

## How to Run the Tests
From Visual Studio:
    **Test → Run All Tests**

This executes the automated tests covering the required ticket operations and business rules.

## Architecture
The application follows a simple layered architecture with clear separation of responsibilities:
* **API Layer**
  Handles HTTP requests, controllers, dependency injection, and global exception handling.
* **Application Layer**
  Contains application services, business logic, DTOs, interfaces, and application exceptions.
* **Domain Layer**
  Contains the core ticket entities, enums, and domain-related rules.
* **Infrastructure Layer**
  Handles Entity Framework Core, database access, repositories, and migrations.

This separation keeps business logic independent from API and database concerns while avoiding unnecessary architectural complexity.

## Architecture Diagram
                    ┌─────────────────────┐
                    │       Client        │
                    │   Swagger / HTTP    │
                    └──────────┬──────────┘
                               │
                               ▼
                    ┌─────────────────────┐
                    │       API Layer     │
                    │   Controllers       │
                    │   DTOs / Endpoints  │
                    └──────────┬──────────┘
                               │
                               ▼
                    ┌─────────────────────┐
                    │     Application     │
                    │   Services          │
                    │   Business Logic    │
                    │   Interfaces        │
                    └──────────┬──────────┘
                               │
                               ▼
                    ┌─────────────────────┐
                    │       Domain        │
                    │     Entities        │
                    │     Enums           │
                    │     Rules           │
                    └──────────┬──────────┘
                               ▲
                               │
                    ┌──────────┴──────────┐
                    │   Infrastructure    │
                    │  EF Core / DbContext│
                    │   Repositories      │
                    └──────────┬──────────┘
                               │
                               ▼
                    ┌─────────────────────┐
                    │     SQL Server      │
                    └─────────────────────┘

## Sequence Diagram
<img width="914" height="533" alt="image" src="https://github.com/user-attachments/assets/3376c3d3-341e-4df3-9ffe-9eae14c60777" />

                    
## Assumptions
* SQL Server is available in the local development environment.
* The configured connection string points to a valid SQL Server instance.
* Entity Framework Core migrations are used to create and update the database schema.
* The API is intended to run in a development/local environment as specified by the assignment.

## Improvements With More Time
If more time were available, I would consider:
* Adding more comprehensive test coverage for edge cases and integration scenarios.
* Adding API integration tests to verify the complete request/response flow.
* Improving API documentation with more detailed endpoint examples.
* Adding production-oriented configuration and deployment setup.

These improvements are intentionally outside the current implementation scope to keep the solution focused on the assignment requirements.
