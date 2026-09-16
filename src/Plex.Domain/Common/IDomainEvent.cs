namespace Plex.Domain.Common;

/// <summary>
/// Defines the contract for domain events emitted by domain aggregates.
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// The unique identifier of the event instance.
    /// </summary>
    Guid EventId { get; }

    /// <summary>
    /// The UTC timestamp when the domain event occurred.
    /// </summary>
    DateTimeOffset OccurredAt { get; }
}
