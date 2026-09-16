using Plex.Domain.Common;

namespace Plex.Domain.Operations;

/// <summary>
/// Domain event published when an operation is created and queued.
/// </summary>
public sealed record OperationQueuedDomainEvent(
    Guid OperationId,
    string Name,
    DateTimeOffset OccurredAt,
    Guid EventId = default) : IDomainEvent
{
    public Guid EventId { get; init; } = EventId == default ? Guid.NewGuid() : EventId;
}

/// <summary>
/// Domain event published when an operation transitions to the running state.
/// </summary>
public sealed record OperationStartedDomainEvent(
    Guid OperationId,
    DateTimeOffset OccurredAt,
    Guid EventId = default) : IDomainEvent
{
    public Guid EventId { get; init; } = EventId == default ? Guid.NewGuid() : EventId;
}

/// <summary>
/// Domain event published when an operation succeeds.
/// </summary>
public sealed record OperationSucceededDomainEvent(
    Guid OperationId,
    DateTimeOffset OccurredAt,
    Guid EventId = default) : IDomainEvent
{
    public Guid EventId { get; init; } = EventId == default ? Guid.NewGuid() : EventId;
}

/// <summary>
/// Domain event published when an operation fails.
/// </summary>
public sealed record OperationFailedDomainEvent(
    Guid OperationId,
    string Reason,
    DateTimeOffset OccurredAt,
    Guid EventId = default) : IDomainEvent
{
    public Guid EventId { get; init; } = EventId == default ? Guid.NewGuid() : EventId;
}
