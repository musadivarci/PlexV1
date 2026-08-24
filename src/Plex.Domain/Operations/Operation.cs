namespace Plex.Domain.Operations;

public enum OperationStatus
{
    Queued,
    Running,
    Succeeded,
    Failed
}

public sealed class Operation
{
    private readonly List<OperationAuditEntry> _audit = [];

    private Operation(Guid id, string name, DateTimeOffset createdAt)
    {
        Id = id;
        Name = name;
        CreatedAt = createdAt;
        Status = OperationStatus.Queued;
        Record("operation.queued", "Operation created and queued.", createdAt);
    }

    public Guid Id { get; }
    public string Name { get; }
    public OperationStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; }
    public IReadOnlyCollection<OperationAuditEntry> Audit => _audit.AsReadOnly();

    public static Operation Queue(string name, DateTimeOffset now)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Operation name is required.", nameof(name));

        return new Operation(Guid.NewGuid(), name.Trim(), now);
    }

    public void Start(DateTimeOffset now)
    {
        EnsureStatus(OperationStatus.Queued);
        Status = OperationStatus.Running;
        Record("operation.started", "Operation execution started.", now);
    }

    public void Succeed(DateTimeOffset now)
    {
        EnsureStatus(OperationStatus.Running);
        Status = OperationStatus.Succeeded;
        Record("operation.succeeded", "Operation completed successfully.", now);
    }

    public void Fail(string reason, DateTimeOffset now)
    {
        EnsureStatus(OperationStatus.Running);
        Status = OperationStatus.Failed;
        Record("operation.failed", string.IsNullOrWhiteSpace(reason) ? "Operation failed." : reason.Trim(), now);
    }

    private void EnsureStatus(OperationStatus expected)
    {
        if (Status != expected)
            throw new InvalidOperationException($"Expected {expected} but operation is {Status}.");
    }

    private void Record(string eventType, string message, DateTimeOffset occurredAt) =>
        _audit.Add(new OperationAuditEntry(Guid.NewGuid(), eventType, message, occurredAt));
}

public sealed record OperationAuditEntry(
    Guid Id,
    string EventType,
    string Message,
    DateTimeOffset OccurredAt);
