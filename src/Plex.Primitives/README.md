# Plex.Primitives

Small, dependency-free primitives for modern .NET backend applications.

Created and maintained by **Musa Divarcı**.

## Install

```bash
dotnet add package Plex.Primitives
```

## Result<T>

```csharp
using Plex.Primitives;

static Result<int> ParsePort(string value)
{
    return int.TryParse(value, out var port) && port > 0
        ? Result.Success(port)
        : Result.Failure<int>(new Error("port.invalid", "Port must be a positive integer."));
}

var message = ParsePort("8080").Match(
    port => $"Listening on {port}",
    error => error.Message);
```

## Guard

```csharp
var name = Guard.NotNullOrWhiteSpace(input.Name, nameof(input.Name));
var retryCount = Guard.Positive(input.RetryCount, nameof(input.RetryCount));
```

## Design goals

- zero runtime dependencies
- explicit success/failure modeling
- small API surface
- nullable-reference-type friendly
- suitable for domain and application layers

## Author

**Musa Divarcı** — .NET / C# Software Engineer  
GitHub: https://github.com/musadivarci

Source: https://github.com/musadivarci/PlexV1
