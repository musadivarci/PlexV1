using Microsoft.AspNetCore.SignalR;

namespace Plex.Api.Hubs;

/// <summary>
/// Real-time SignalR hub delivering operations and telemetry events to connected clients.
/// </summary>
public sealed class OperationsHub : Hub<IOperationHubClient>
{
    private readonly ILogger<OperationsHub> _logger;

    public OperationsHub(ILogger<OperationsHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("SignalR client connected: {ConnectionId}", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("SignalR client disconnected: {ConnectionId}", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}
