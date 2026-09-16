using Plex.Api.Contracts;
using Plex.Api.Hubs;
using Plex.Application.Operations;
using Plex.Infrastructure.Operations;

var builder = WebApplication.CreateBuilder(args);

// Core Services
builder.Services.AddSingleton<IOperationStore, InMemoryOperationStore>();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScoped<IOperationEventPublisher, SignalROperationEventPublisher>();
builder.Services.AddScoped<OperationService>();

// SignalR & Health
builder.Services.AddSignalR();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapHealthChecks("/health");

// SignalR Real-Time Hub
app.MapHub<OperationsHub>("/hubs/operations");

// Operational REST API
var operations = app.MapGroup("/api/operations");

operations.MapGet("/", async (OperationService service, int? limit, CancellationToken ct) =>
{
    var list = await service.ListAsync(limit ?? 50, ct);
    return Results.Ok(list.Select(op => op.ToDetailDto()));
});

operations.MapPost("/", async (CreateOperationRequest request, OperationService service, CancellationToken ct) =>
{
    if (string.IsNullOrWhiteSpace(request.Name))
    {
        return Results.BadRequest(new { error = "Operation name is required." });
    }

    var operation = await service.QueueAsync(request.Name, ct);
    return Results.Created($"/api/operations/{operation.Id}", operation.ToDetailDto());
});

operations.MapGet("/{id:guid}", async (Guid id, OperationService service, CancellationToken ct) =>
{
    var operation = await service.GetAsync(id, ct);
    return operation is null ? Results.NotFound() : Results.Ok(operation.ToDetailDto());
});

operations.MapPost("/{id:guid}/start", async (Guid id, OperationService service, CancellationToken ct) =>
{
    try
    {
        var operation = await service.StartAsync(id, ct);
        return Results.Ok(operation.ToDetailDto());
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound();
    }
    catch (InvalidOperationException ex)
    {
        return Results.Conflict(new { error = ex.Message });
    }
});

operations.MapPost("/{id:guid}/succeed", async (Guid id, OperationService service, CancellationToken ct) =>
{
    try
    {
        var operation = await service.SucceedAsync(id, ct);
        return Results.Ok(operation.ToDetailDto());
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound();
    }
    catch (InvalidOperationException ex)
    {
        return Results.Conflict(new { error = ex.Message });
    }
});

operations.MapPost("/{id:guid}/fail", async (Guid id, FailOperationRequest request, OperationService service, CancellationToken ct) =>
{
    try
    {
        var operation = await service.FailAsync(id, request.Reason, ct);
        return Results.Ok(operation.ToDetailDto());
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound();
    }
    catch (InvalidOperationException ex)
    {
        return Results.Conflict(new { error = ex.Message });
    }
});

app.Run();

