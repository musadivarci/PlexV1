using Microsoft.AspNetCore.SignalR;
using Plex.Api.Contracts;
using Plex.Application.Operations;
using Plex.Domain.Common;
using Plex.Domain.Operations;

namespace Plex.Api.Hubs;

/// <summary>
/// Bridges domain events to SignalR clients for live operational visibility.
/// </summary>
public sealed class SignalROperationEventPublisher(
    IHubContext<OperationsHub, IOperationHubClient> hubContext,
    ILogger<SignalROperationEventPublisher> logger) : IOperationEventPublisher
{
    public async Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        try
        {
            switch (domainEvent)
            {
                case OperationQueuedDomainEvent queued:
                    await hubContext.Clients.All.OperationQueued(
                        new OperationSummaryDto(queued.OperationId, queued.Name, OperationStatus.Queued.ToString(), queued.OccurredAt));
                    break;

                case OperationStartedDomainEvent started:
                    await hubContext.Clients.All.OperationStarted(started.OperationId, started.OccurredAt);
                    break;

                case OperationSucceededDomainEvent succeeded:
                    await hubContext.Clients.All.OperationSucceeded(succeeded.OperationId, succeeded.OccurredAt);
                    break;

                case OperationFailedDomainEvent failed:
                    await hubContext.Clients.All.OperationFailed(failed.OperationId, failed.Reason, failed.OccurredAt);
                    break;

                default:
                    logger.LogDebug("Unhandled domain event type: {EventType}", domainEvent.GetType().Name);
                    break;
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to broadcast domain event {EventType} over SignalR", domainEvent.GetType().Name);
        }
    }
}
