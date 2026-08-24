using System.Collections.Concurrent;
using Plex.Application.Operations;
using Plex.Domain.Operations;

namespace Plex.Infrastructure.Operations;

public sealed class InMemoryOperationStore : IOperationStore
{
    private readonly ConcurrentDictionary<Guid, Operation> _operations = new();

    public Task AddAsync(Operation operation, CancellationToken cancellationToken = default)
    {
        if (!_operations.TryAdd(operation.Id, operation))
            throw new InvalidOperationException($"Operation {operation.Id} already exists.");

        return Task.CompletedTask;
    }

    public Task<Operation?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _operations.TryGetValue(id, out var operation);
        return Task.FromResult(operation);
    }

    public Task SaveAsync(Operation operation, CancellationToken cancellationToken = default)
    {
        _operations[operation.Id] = operation;
        return Task.CompletedTask;
    }
}
