# Dotnet_Server

Modernized ASP.NET Core API server for the BuyingLibrary customer flow.

## What changed in this rework

- Upgraded platform to **.NET 10**.
- Migrated to latest **BuyingLibrary v2 API surface** and vendored library source in-repo.
- Refactored startup for modern hosting:
  - options binding and startup validation
  - centralized exception handling with ProblemDetails
  - explicit CORS policy
  - HTTPS/HSTS
  - OpenAPI in development
  - health checks (`/health`) with Mongo and mail checks
- Refactored controllers:
  - fixed route templates
  - improved HTTP semantics and response codes
  - added ObjectId validation guards
  - removed GET side effects
  - added cancellation token support
- Hardened mail sender:
  - async SMTP operations
  - safer config checks
  - structured logging
- Added tests:
  - unit tests for core controller behavior
  - integration tests for root and health endpoints
- Added CI and dependency automation.

## Repository layout

- `Aspnet_server.csproj` — API host project
- `BuyingLibrary/` — shared models/services dependency (v2 source)
- `controllers/` — API controllers
- `mail_sender/` — email sender abstraction/implementation
- `Infrastructure/` — health checks
- `Contracts/` — request DTOs with annotations
- `tests/Aspnet_server.Tests/` — unit/integration tests
- `docs/` — migration, baseline, compatibility, and architecture docs

## Requirements

- .NET SDK 10.x
- MongoDB 6+

## Configuration

Set values in `appsettings.json` / environment variables:

- `ConnectionStrings:ClientUrl`
- `DataBaseSettings:DataBaseConnection`
- `DataBaseSettings:DataBase`
- `EmailSettings:*` (optional for app startup, required for actual mail sending)

## Run

```bash
dotnet restore Aspnet_server.sln
dotnet build Aspnet_server.sln
dotnet run --project /home/runner/work/Dotnet_Server/Dotnet_Server/Aspnet_server.csproj
```

## Test

```bash
dotnet test /home/runner/work/Dotnet_Server/Dotnet_Server/tests/Aspnet_server.Tests/Aspnet_server.Tests.csproj
```

## API conventions

- Controllers use `[ApiController]` behavior.
- Invalid identifiers return validation-style `400` responses.
- Missing entities return `404`.
- Created resources return `201 Created` where applicable.
- Unhandled exceptions are translated to RFC7807 responses.

## Detailed documentation

- Baseline/discovery: `docs/discovery-baseline.md`
- BuyingLibrary compatibility matrix: `docs/buyinglibrary-compatibility-matrix.md`
- Architecture + Mermaid diagrams: `docs/architecture.md`
