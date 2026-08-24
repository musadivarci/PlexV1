using Plex.Domain.Operations;

namespace Plex.Application.Operations;

public interface IOperationStore
{
    Task AddAsync(Operation operation, CancellationToken cancellationToken = default);
    Task<Operation?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task SaveAsync(Operation operation, CancellationToken cancellationToken = default);
}

public sealed class OperationService(IOperationStore store, TimeProvider timeProvider)
{
    public async Task<Operation> QueueAsync(string name, CancellationToken cancellationToken = default)
    {
        var operation = Operation.Queue(name, timeProvider.GetUtcNow());
        await store.AddAsync(operation, cancellationToken);
        return operation;
    }

    public async Task<Operation> StartAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var operation = await Required(id, cancellationToken);
        operation.Start(timeProvider.GetUtcNow());
        await store.SaveAsync(operation, cancellationToken);
        return operation;
    }

    public async Task<Operation> SucceedAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var operation = await Required(id, cancellationToken);
        operation.Succeed(timeProvider.GetUtcNow());
        await store.SaveAsync(operation, cancellationToken);
        return operation;
    }

    public async Task<Operation> FailAsync(Guid id, string reason, CancellationToken cancellationToken = default)
    {
        var operation = await Required(id, cancellationToken);
        operation.Fail(reason, timeProvider.GetUtcNow());
        await store.SaveAsync(operation, cancellationToken);
        return operation;
    }

    public Task<Operation?> GetAsync(Guid id, CancellationToken cancellationToken = default) =>
        store.GetAsync(id, cancellationToken);

    private async Task<Operation> Required(Guid id, CancellationToken cancellationToken) =>
        await store.GetAsync(id, cancellationToken)
        ?? throw new KeyNotFoundException($"Operation {id} was not found.");
}
