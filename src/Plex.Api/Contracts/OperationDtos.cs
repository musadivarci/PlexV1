using Plex.Domain.Operations;

namespace Plex.Api.Contracts;

public sealed record OperationSummaryDto(
    Guid Id,
    string Name,
    string Status,
    DateTimeOffset CreatedAt);

public sealed record OperationDetailDto(
    Guid Id,
    string Name,
    string Status,
    DateTimeOffset CreatedAt,
    IReadOnlyCollection<OperationAuditDto> Audit);

public sealed record OperationAuditDto(
    Guid Id,
    string EventType,
    string Message,
    DateTimeOffset OccurredAt);

public sealed record CreateOperationRequest(string Name);
public sealed record FailOperationRequest(string Reason);

public static class OperationMappingExtensions
{
    public static OperationDetailDto ToDetailDto(this Operation operation) =>
        new(
            operation.Id,
            operation.Name,
            operation.Status.ToString(),
            operation.CreatedAt,
            operation.Audit.Select(a => new OperationAuditDto(a.Id, a.EventType, a.Message, a.OccurredAt)).ToList());

    public static OperationSummaryDto ToSummaryDto(this Operation operation) =>
        new(
            operation.Id,
            operation.Name,
            operation.Status.ToString(),
            operation.CreatedAt);
}
