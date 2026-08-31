using Plex.Domain.Operations;
using Xunit;

namespace Plex.Domain.Tests;

public class OperationTests
{
    [Fact]
    public void Queue_WithValidName_ShouldCreateQueuedOperationWithAudit()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        const string name = "nightly-ledger-rebuild";

        // Act
        var operation = Operation.Queue(name, now);

        // Assert
        Assert.NotEqual(Guid.Empty, operation.Id);
        Assert.Equal(name, operation.Name);
        Assert.Equal(OperationStatus.Queued, operation.Status);
        Assert.Equal(now, operation.CreatedAt);
        Assert.Single(operation.Audit);

        var audit = operation.Audit.First();
        Assert.Equal("operation.queued", audit.EventType);
        Assert.Equal(now, audit.OccurredAt);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Queue_WithInvalidName_ShouldThrowArgumentException(string invalidName)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Operation.Queue(invalidName, DateTimeOffset.UtcNow));
    }

    [Fact]
    public void Start_WhenQueued_ShouldTransitionToRunningAndRecordAudit()
    {
        // Arrange
        var created = DateTimeOffset.UtcNow;
        var started = created.AddSeconds(5);
        var operation = Operation.Queue("sync-job", created);

        // Act
        operation.Start(started);

        // Assert
        Assert.Equal(OperationStatus.Running, operation.Status);
        Assert.Equal(2, operation.Audit.Count);
        Assert.Contains(operation.Audit, a => a.EventType == "operation.started" && a.OccurredAt == started);
    }

    [Fact]
    public void Succeed_WhenRunning_ShouldTransitionToSucceededAndRecordAudit()
    {
        // Arrange
        var created = DateTimeOffset.UtcNow;
        var started = created.AddSeconds(2);
        var completed = created.AddSeconds(10);
        var operation = Operation.Queue("sync-job", created);
        operation.Start(started);

        // Act
        operation.Succeed(completed);

        // Assert
        Assert.Equal(OperationStatus.Succeeded, operation.Status);
        Assert.Equal(3, operation.Audit.Count);
        Assert.Contains(operation.Audit, a => a.EventType == "operation.succeeded" && a.OccurredAt == completed);
    }

    [Fact]
    public void Fail_WhenRunning_ShouldTransitionToFailedAndRecordReason()
    {
        // Arrange
        var created = DateTimeOffset.UtcNow;
        var started = created.AddSeconds(2);
        var failed = created.AddSeconds(10);
        const string reason = "Connection timeout to external bank gateway";
        var operation = Operation.Queue("sync-job", created);
        operation.Start(started);

        // Act
        operation.Fail(reason, failed);

        // Assert
        Assert.Equal(OperationStatus.Failed, operation.Status);
        Assert.Equal(3, operation.Audit.Count);
        var audit = operation.Audit.Last();
        Assert.Equal("operation.failed", audit.EventType);
        Assert.Equal(reason, audit.Message);
    }

    [Fact]
    public void Start_WhenAlreadyRunning_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var operation = Operation.Queue("sync-job", DateTimeOffset.UtcNow);
        operation.Start(DateTimeOffset.UtcNow);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => operation.Start(DateTimeOffset.UtcNow));
    }
}
