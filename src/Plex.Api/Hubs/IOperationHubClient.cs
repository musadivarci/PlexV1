using Plex.Api.Contracts;

namespace Plex.Api.Hubs;

/// <summary>
/// Strong-typed SignalR client methods for real-time operation and audit streams.
/// </summary>
public interface IOperationHubClient
{
    Task OperationQueued(OperationSummaryDto operation);
    Task OperationStarted(Guid operationId, DateTimeOffset occurredAt);
    Task OperationSucceeded(Guid operationId, DateTimeOffset occurredAt);
    Task OperationFailed(Guid operationId, string reason, DateTimeOffset occurredAt);
}
