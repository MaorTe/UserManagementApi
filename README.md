# UserManagementApi
A .NET 9-based Minimal API RESTful project for managing users and their associated cars, providing endpoints to create, read, update, and delete both user and car records backed by Entity Framework Core and a relational database.
It follows Clean Architecture principles with a modular, clean‐style structure-separating data access, business logic, endpoint definitions, and middleware-and uses dependency injection to ensure a maintainable codebase.
---

## Table of Contents

- [Description](#description)  
- [Folder Structure](#folder-structure)  
- [Prerequisites](#prerequisites)  
- [Installation](#installation)  
- [Running the Application](#running-the-application)  
- [API Endpoints (CRUD)](#api-endpoints-crud)  
- [Architecture Overview](#architecture-overview)  
- [Technologies Used](#technologies-used)  

---

## Description

This project exposes a lightweight HTTP API for managing user records (create, read, update, delete). It follows Clean Architecture conventions by separating concerns into distinct layers:

1. **Data**: EF Core DbContext, repository implementations, and migrations.  
2. **Models**: Domain entities and Data Transfer Objects (DTOs).  
3. **Services**: Business‐logic interfaces and their implementations.  
4. **Endpoints**: Minimal API route definitions (no MVC controllers).  
5. **Middleware**: Any global filters or cross‐cutting concerns (e.g., exception handling, logging).  

Dependency Injection is configured in `Program.cs`, providing a straightforward way to swap implementations or plug in new services. The code emphasizes clean, readable patterns and avoids unnecessary complexity.

---

## Folder Structure
UserManagementApi/ <br>
├── Data/ AppDbContext.cs            # EF Core DbContext and configurations <br>
│   └── …                           # (e.g., repository classes, migrations folder) <br>
│<br>
├── Endpoints/<br>
│   ├── UserEndpoints.cs           # Minimal API route definitions for User CRUD<br>
│   └── …                           <br>
│<br>
├── Middleware/<br>
│   ├── ExceptionMiddleware.cs     # Global exception handler (example)<br>
│   └── …                           <br>
│<br>
├── Models/<br>
│   ├── User.cs                    # Domain entity<br>
│   ├── UserDto.cs                 # DTO for incoming/outgoing payloads<br>
│   └── …                           <br>
│<br>
├── Services/<br>
│   ├── IUserService.cs            # Business-logic interface<br>
│   ├── UserService.cs             # Concrete implementation<br>
│   └── …                           <br>
│<br>
├── Properties/<br>
│   └── launchSettings.json        # Default launch settings (profiles, environment)<br>
│<br>
├── appsettings.Development.json   # Development-only settings (e.g., connection string)<br>
├── Program.cs                     # Application entry point: configure services & middleware<br>
├── UserManagementApi.csproj       # .NET 9 project file<br>
└── UserManagementApi.sln          # Visual Studio solution file<br>


## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (installed and on your `PATH`)  
- A running SQL Server instance (or another EF Core–compatible database)  
- (Optional) [EF Core CLI tools](https://learn.microsoft.com/ef/core/cli/dotnet) for applying migrations  

---

## Installation

1. **Clone the repository**  
   ```bash
   git clone https://github.com/MaorTe/UserManagementApi.git
   cd UserManagementApi
2.Configure your database connection
  Open appsettings.Development.json.
Update the ConnectionStrings:DefaultConnection entry to point to your SQL Server (or other supported database).

2. Configure your database connection
3. Clone the repository
   ```bash
   dotnet restore
4. Apply EF Core migrations
   If you have the EF Core CLI tools installed, run:
   ```bash
   dotnet ef database update
5. If you don’t have the CLI tools, the app will auto‐create the database on first run (depending on how AppDbContext is configured).

---

## Running the Application
You can run the API from the command line or via an IDE (Visual Studio, Rider, VS Code).
Via CLI
   ```bash
   dotnet run --project UserManagementApi.csproj
   ```
By default, the Minimal API will start listening on the ports specified in launchSettings.json.
Via Visual Studio
Open UserManagementApi.sln.
Set UserManagementApi as the startup project.
Press F5 or click the “Run” (▶) button.
Once running, you should see console output indicating that the web host is listening on a localhost port.

## API Endpoints (CRUD) for User

| Operation      | HTTP Method | URL               | Request Body (JSON) | Response                                                                                                                                                      |
|----------------|-------------|-------------------|---------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Create User    | POST        | `/api/users`      | JSON body           | `201 Created` with the created user payload (including its generated ID).                                                                                    |
| Get All Users  | GET         | `/api/users`      | _None_              | `200 OK` with a JSON array of all users.                                                                                                                      |
| Get User by ID | GET         | `/api/users/{id}` | _None_              | `200 OK` with user payload if found.<br>`404 Not Found` if no user exists for the specified ID.                                                             |
| Update User    | PUT         | `/api/users/{id}` | JSON body           | `204 No Content` on success (or `200 OK` with updated resource, depending on convention).<br>`404 Not Found` if the user does not exist.                    |
| Delete User    | DELETE      | `/api/users/{id}` | _None_              | `204 No Content` on successful deletion.<br>`404 Not Found` if no user exists for the specified ID.                                                         |


## API Endpoints (CRUD) for Car

| Operation    | HTTP Method | URL               | Request Body (JSON) | Response                                                                                                                                                      |
|--------------|-------------|-------------------|---------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Create Car   | POST        | `/api/cars`       | JSON body           | `201 Created` with the created car payload (including its generated ID).                                                                                     |
| Get All Cars | GET         | `/api/cars`       | _None_              | `200 OK` with a JSON array of all cars.                                                                                                                       |
| Get Car by ID| GET         | `/api/cars/{id}`  | _None_              | `200 OK` with car payload if found.<br>`404 Not Found` if no car exists for the specified ID.                                                                |
| Update Car   | PUT         | `/api/cars/{id}`  | JSON body           | `204 No Content` on success (or `200 OK` with updated resource, depending on convention).<br>`404 Not Found` if the car does not exist.                     |
| Delete Car   | DELETE      | `/api/cars/{id}`  | _None_              | `204 No Content` on successful deletion.<br>`404 Not Found` if no car exists for the specified ID.                                                          |

## Architecture Overview
1. **Data Layer (`/Data`)**
   - **AppDbContext**: Inherits from `DbContext`, defines `DbSet<User> Users`, configures model relationships, migrations, and any seeding logic.
   - **Repositories (optional)**: If implemented, these classes directly interact with EF Core to perform CRUD; otherwise, the service layer may directly use `AppDbContext`.

2. **Models (`/Models`)**
   - **User**: The domain entity representing a user record (properties like `Id`, `FirstName`, `LastName`, `Email`, etc.).
   - **UserDto**: A DTO to validate incoming data and shape outgoing responses (prevents over-posting or exposing internal fields).

3. **Services (`/Services`)**
   - **IUserService**: Defines business-logic operations (e.g., `CreateAsync`, `GetAllAsync`, `GetByIdAsync`, `UpdateAsync`, `DeleteAsync`).
   - **UserService**: Implements `IUserService`, uses `AppDbContext` (or repository) to perform EF Core operations. Contains any validation or transactional logic.

4. **Endpoints (`/Endpoints`)**
   - Defines one or more static classes (e.g., `UserEndpoints`) that group related routes. Each route handler calls the corresponding `IUserService` method.
   - Uses minimal API syntax (`app.MapGet`, `app.MapPost`, etc.) to wire HTTP verbs to handler lambdas.

5. **Middleware (`/Middleware`)**
   - Custom middleware for cross-cutting concerns, such as:
     - **ExceptionMiddleware**: Catches unhandled exceptions, logs them, and returns standardized error payloads.
     - **LoggingMiddleware** (if present): Logs request/response details for diagnostics.

6. **Dependency Injection**
   - All services and the `DbContext` are registered in `Program.cs`.
   - This promotes loose coupling and makes unit testing easier (services can be mocked).

Overall, the dependencies flow inward:

- **Endpoints** → **Services** → **Data (AppDbContext)** → **Database**
- **Models** (shared between layers to shape data)

This separation ensures that each layer has a single responsibility, making the codebase more maintainable and testable.

## Technologies Used
.NET 9 (Minimal API model)

C# 12 (as supported by .NET 9)

Entity Framework Core (EF Core 8 or higher)

Dependency Injection (built into ASP.NET Core)

SQL Server (or any EF Core–supported relational database)

Clean Architecture Principles (layers for domain, data, services, and presentation)

Middleware (custom exception handling, logging)
