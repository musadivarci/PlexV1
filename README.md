# Plex

> **Distributed Operations & Audit Platform**

**Created and engineered by Musa Divarcı** — .NET / C# software engineer and technology professional.

Plex is an architecture-focused backend platform for coordinating long-running operational jobs while preserving a complete, queryable audit trail of what happened, when, and why.

The project is designed as a technical reference for production-oriented **ASP.NET Core, C# and distributed backend engineering** rather than as a CRUD demo.

## Engineering goals

- clean separation between domain, application and infrastructure concerns
- explicit operational state transitions
- durable audit history
- background processing with retry-aware workers
- API-first integration
- observable and testable business flows
- infrastructure that can evolve without leaking into the domain

## Stack

- **.NET 10 LTS**
- **C# 14**
- **ASP.NET Core**
- **Entity Framework Core**
- **PostgreSQL**
- **BackgroundService / Worker Services**
- **OpenAPI**
- **Health Checks**
- **xUnit**
- **Docker**
- **GitHub Actions**

.NET 10 is the active LTS release line used by this project.

## Architecture

```text
Plex.Domain
    ↓
Plex.Application
    ↓
Plex.Infrastructure
    ↓
Plex.Api          Plex.Worker
       \            /
        --- PostgreSQL ---
```

### `Plex.Domain`
Pure business rules, aggregates, value objects and domain events. No database or web framework dependencies.

### `Plex.Application`
Use cases and ports. Coordinates domain behavior without depending on concrete infrastructure.

### `Plex.Infrastructure`
EF Core persistence, external adapters and repository implementations.

### `Plex.Api`
ASP.NET Core HTTP boundary, OpenAPI, health checks and dependency composition.

### `Plex.Worker`
Long-running background processing for queued operational work.

### `Plex.Tests`
Behavior-focused automated tests around the domain and application layers.

## Core model

The first vertical slice centers on an `Operation` aggregate.

An operation has a lifecycle:

```text
Queued -> Running -> Succeeded
                  \-> Failed
```

Every meaningful transition emits an immutable audit event. This keeps operational truth separate from transient logs.

## Why this repository exists

My public repositories include modern TypeScript and AI-oriented work. Plex documents another core part of my engineering background: **C#/.NET backend development, layered architecture and service-oriented system design**.

## Author

**Musa Divarcı**  
Creator · Project Lead · Software Engineer

Primary technical focus represented in this repository:

`C#` · `.NET` · `ASP.NET Core` · `Backend Architecture` · `Clean Architecture` · `Distributed Systems` · `PostgreSQL`

---

**Good backend architecture is less about frameworks and more about preserving boundaries as systems grow.**