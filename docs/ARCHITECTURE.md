# Plex Architecture

## Context

Plex is intentionally structured as a backend system with explicit dependency boundaries.

The current reference slice uses an in-memory adapter so the repository can compile and run without external infrastructure. The production persistence direction is EF Core + PostgreSQL.

## Dependency rule

Dependencies point inward:

```text
Domain <- Application <- Infrastructure
                       <- API
                       <- Worker
```

`Plex.Domain` has no dependency on ASP.NET Core, EF Core, PostgreSQL or hosting concerns.

## Operation aggregate

`Operation` owns its lifecycle and audit history. State changes are methods on the aggregate rather than public property mutation.

Valid lifecycle:

```text
Queued -> Running -> Succeeded
                  -> Failed
```

Invalid transitions fail fast in the domain layer.

## Application ports

`IOperationStore` is defined by the application layer. Infrastructure implements it.

This makes persistence replaceable without changing domain behavior or application use cases.

## Current adapter

`InMemoryOperationStore` is deliberately simple and dependency-free. It acts as a reference adapter and keeps the first vertical slice executable.

## Persistence evolution

The next persistence adapter is planned around:

- Entity Framework Core
- PostgreSQL
- optimistic concurrency
- durable audit entries
- transactional operation state transitions
- migrations owned by Infrastructure

## Runtime boundaries

### API

The ASP.NET Core API owns transport and composition only. Business transitions are delegated to `OperationService`.

### Worker

The Worker Service is a separate process boundary for long-running jobs. This keeps background execution independent from HTTP request lifetime.

## Architectural principle

> Frameworks belong at the edges. Business invariants belong in the center.

This repository is maintained by **Musa Divarcı** as a public .NET/C# architecture project.
