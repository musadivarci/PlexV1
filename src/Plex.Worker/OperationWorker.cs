namespace Plex.Worker;

public sealed class OperationWorker(ILogger<OperationWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Plex worker started at {StartedAt}", DateTimeOffset.UtcNow);

        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogDebug("Plex worker heartbeat at {HeartbeatAt}", DateTimeOffset.UtcNow);
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}
