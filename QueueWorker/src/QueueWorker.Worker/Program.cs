using QueueWorker.Worker.Workers;

var builder = Host.CreateApplicationBuilder(args);
builder.Services
//.AddHostedService<RegularQueueWorker>();
//.AddHostedService<LockQueueWorker>();
.AddHostedService<ConcurrentQueueWorker>();


builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices();

var host = builder.Build();
host.Run();
