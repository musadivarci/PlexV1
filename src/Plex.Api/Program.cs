using Plex.Application.Operations;
using Plex.Infrastructure.Operations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IOperationStore, InMemoryOperationStore>();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScoped<OperationService>();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    service = "Plex API",
    runtime = ".NET 10",
    status = "online"
}));

app.MapHealthChecks("/health");

var operations = app.MapGroup("/api/operations");

operations.MapGet("/", async (OperationService service, int? limit, CancellationToken ct) =>
{
    var list = await service.ListAsync(limit ?? 50, ct);
    return Results.Ok(list.Select(ToResponse));
});

operations.MapPost("/", async (CreateOperationRequest request, OperationService service, CancellationToken ct) =>
{
    var operation = await service.QueueAsync(request.Name, ct);
    return Results.Created($"/api/operations/{operation.Id}", ToResponse(operation));
});

operations.MapGet("/{id:guid}", async (Guid id, OperationService service, CancellationToken ct) =>
{
    var operation = await service.GetAsync(id, ct);
    return operation is null ? Results.NotFound() : Results.Ok(ToResponse(operation));
});

operations.MapPost("/{id:guid}/start", async (Guid id, OperationService service, CancellationToken ct) =>
    Results.Ok(ToResponse(await service.StartAsync(id, ct))));

operations.MapPost("/{id:guid}/succeed", async (Guid id, OperationService service, CancellationToken ct) =>
    Results.Ok(ToResponse(await service.SucceedAsync(id, ct))));

operations.MapPost("/{id:guid}/fail", async (Guid id, FailOperationRequest request, OperationService service, CancellationToken ct) =>
    Results.Ok(ToResponse(await service.FailAsync(id, request.Reason, ct))));

app.Run();

static object ToResponse(Plex.Domain.Operations.Operation operation) => new
{
    operation.Id,
    operation.Name,
    status = operation.Status.ToString(),
    operation.CreatedAt,
    audit = operation.Audit
};

public sealed record CreateOperationRequest(string Name);
public sealed record FailOperationRequest(string Reason);
