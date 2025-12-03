# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Architecture Overview

This is a multi-layered inventory management system with a WPF desktop application and ASP.NET Core backend API.

### Project Structure

- **InventoryManagementApp** - WPF desktop application (.NET 8.0-windows)
  - Uses MVVM pattern with dependency injection
  - Can operate with in-memory cache or backend API based on feature flag

- **InventorySrv** - ASP.NET Core Web API (.NET 8.0)
  - Entity Framework Core with SQL Server or In-Memory database
  - AutoMapper for DTOs
  - JWT authentication (configured but noted as "not polished")
  - API versioning with Swagger

- **Shared.Models** - Shared library containing common models (e.g., `InventoryItem`)

- **InventoryManagementAppTest** - NUnit test project using Moq

### Desktop App Architecture

The WPF app follows a structured MVVM architecture:

**Dependency Injection:**
- All services, ViewModels, and Views are registered in `App.xaml.cs` → `ConfigureServices()`
- Uses Microsoft.Extensions.DependencyInjection
- ViewModels are transient/singleton as appropriate

**Service Layer:**
- `IInventoryService` interface with two implementations:
  - `InMemoryInventoryService` - Local cache-based
  - `ApiSqlInventoryService` - HTTP client to backend API
- `ServiceFactory` pattern selects implementation based on feature flag
- `FeatureManagerService` reads from `App.config` to determine:
  - `UseSqlDb` - Switch between cache (false) and API (true)
  - `ServiceUrl` - Backend API URL (default: http://localhost:5016)

**ViewModel Communication:**
- ViewModels inherit from `BaseViewModel` which extends `ObservableObject`
- Uses EventAggregator pattern (via `INavigationService`) to decouple ViewModel communication
- All ViewModels have an `Initialize(object parameter)` lifecycle method

**Infrastructure:**
- `RelayCommand` - ICommand implementation for MVVM
- `LazyResolver<>` - Lazy dependency resolution registered as singleton
- `ObservableObject` - Base class with INotifyPropertyChanged

### Backend API Architecture

**Layered Structure:**
- Controllers → Services → Repositories → DbContext
- AutoMapper profiles for DTO mapping
- Repository pattern (`IInventoryRepo`, `InventoryRepo`)

**Configuration:**
- Database selection via `UseInMemoryDB` flag in appsettings (dev only)
- Connection strings in appsettings for SQL Server
- JWT configuration: `Issuer`, `Audience`, `SuperSecretKey`

**Middleware:**
- `JsonFormatMiddleware` - Custom JSON formatting

**Data Seeding:**
- `SeedDb.InitializeDb()` called in Program.cs after app build

## Build and Run Commands

### Build the Solution
```bash
dotnet build InventoryManagementSystem.sln
```

### Run the Desktop App
```bash
dotnet run --project InventoryManagementApp/InventoryManagementApp.csproj
```

### Run the Backend API
```bash
dotnet run --project InventorySrv/InventorySrv.csproj
```

The API will start on http://localhost:5016 (or port specified in launchSettings)

### Run All Tests
```bash
dotnet test
```

### Run Tests for Specific Project
```bash
dotnet test InventoryManagementAppTest/InventoryManagementAppTest.csproj
```

### Run Single Test
```bash
dotnet test --filter "FullyQualifiedName~TestMethodName"
```

## Configuration

### Desktop App Configuration (App.config)
- **UseSqlDb**: `false` = use in-memory cache, `true` = use backend API
- **ServiceUrl**: Backend API endpoint (default: http://localhost:5016)

### Backend API Configuration (appsettings.json / appsettings.Development.json)
- **UseInMemoryDB**: Development only, switches between in-memory and SQL Server
- **ConnectionStrings:SqlServerConnection**: SQL Server connection string
- JWT settings: **Issuer**, **Audience**, **SuperSecretKey**

## Key Patterns and Conventions

### Service Factory Pattern
The app uses a Service Factory to dynamically select between `InMemoryInventoryService` and `ApiSqlInventoryService`. When adding new inventory service implementations:
1. Implement `IInventoryService`
2. Register in DI container in `App.xaml.cs`
3. Add mapping in `ServiceFactory._serviceMap`

### ViewModel Lifecycle
ViewModels follow this pattern:
1. Constructor injection of dependencies
2. `Initialize(object parameter)` method called by navigation service
3. Subscribe to events as needed
4. Commands expose actions to the View

### Shared Models
`Shared.Models` project contains entities used by both frontend and backend. Changes here affect both projects.

### API DTOs
Backend uses separate DTOs (CreateDto, ReadDto, UpdateDto) mapped via AutoMapper. Don't confuse DTOs with the shared models.

## Known Limitations (per README)

The following are intentionally NOT implemented:
- Logging and monitoring
- Authentication/Authorization (JWT configured but not enforced)
- Performance cache layer
