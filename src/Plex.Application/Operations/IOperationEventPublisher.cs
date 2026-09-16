using Plex.Domain.Common;

namespace Plex.Application.Operations;

/// <summary>
/// Defines the port for publishing domain events originating from operations.
/// </summary>
public interface IOperationEventPublisher
{
    Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
}

/// <summary>
/// Default null-object publisher when no external event dispatcher or message bus is configured.
/// </summary>
public sealed class NullOperationEventPublisher : IOperationEventPublisher
{
    public static readonly NullOperationEventPublisher Instance = new();

    public Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default) =>
        Task.CompletedTask;
}
