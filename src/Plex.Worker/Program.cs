using Plex.Application.Operations;
using Plex.Infrastructure.Operations;
using Plex.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IOperationStore, InMemoryOperationStore>();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScoped<OperationService>();
builder.Services.AddHostedService<OperationWorker>();

await builder.Build().RunAsync();
