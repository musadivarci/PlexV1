# Plex

> **Distributed Operations & Audit Platform**

**Created and engineered by Musa Divarcı** — .NET / C# software engineer and technology professional.

Plex is an architecture-focused backend platform for coordinating long-running operational jobs while preserving a complete, queryable audit trail of what happened, when, and why.

The project is designed as a technical reference for production-oriented **ASP.NET Core, C# and distributed backend engineering** rather than as a CRUD demo.

## Current implementation

- **.NET 10 LTS**
- **C# 14**
- **ASP.NET Core Minimal API**
- **Clean dependency boundaries**
- **Domain aggregate with guarded state transitions**
- **Application ports and use-case service**
- **Infrastructure adapter**
- **Worker Service / BackgroundService**
- **Health Checks**
- **Docker multi-stage build**
- **GitHub Actions CI**

The first executable vertical slice models an operational lifecycle with immutable audit entries:

```text
Queued -> Running -> Succeeded
                  \-> Failed
```

## Architecture

```text
Plex.Domain
    ↑
Plex.Application
    ↑
Plex.Infrastructure
   ↗            ↖
Plex.Api      Plex.Worker
```

The dependency rule is intentional: frameworks stay at the edges; business invariants stay in the center.

### `Plex.Domain`
Pure C# business rules and the `Operation` aggregate. No ASP.NET Core or persistence dependency.

### `Plex.Application`
Use cases and ports such as `IOperationStore`.

### `Plex.Infrastructure`
Concrete adapters. The current executable reference adapter is in-memory so the first slice remains dependency-free and easy to run.

### `Plex.Api`
ASP.NET Core HTTP boundary exposing operation lifecycle endpoints and health checks.

### `Plex.Worker`
Independent Worker Service boundary for long-running background work.

## Persistence roadmap

The production persistence adapter is deliberately a separate next step:

`Entity Framework Core` · `PostgreSQL` · `migrations` · `optimistic concurrency` · `transactional audit history`

Testing and observability will then expand with:

`xUnit` · `integration tests` · `structured logging` · `OpenTelemetry`

See [`docs/ARCHITECTURE.md`](docs/ARCHITECTURE.md) for the architectural rationale.

## Example API flow

```http
POST /api/operations
{ "name": "nightly-ledger-rebuild" }

POST /api/operations/{id}/start
POST /api/operations/{id}/succeed
GET  /api/operations/{id}
```

The returned operation includes its audit trail, keeping state history explicit instead of hiding it in transient logs.

## Why this repository exists

My public repositories also include modern TypeScript and AI-oriented work. Plex documents another core part of my engineering background: **C#/.NET backend development, layered architecture and service-oriented system design**.

## Author

**Musa Divarcı**  
Creator · Project Lead · Software Developer  - MIS

Technical identity represented by this repository:

`C#` · `.NET` · `ASP.NET Core` · `Backend Architecture` · `Clean Architecture` · `Distributed Systems`

---

**Good backend architecture is less about frameworks and more about preserving boundaries as systems grow.**
