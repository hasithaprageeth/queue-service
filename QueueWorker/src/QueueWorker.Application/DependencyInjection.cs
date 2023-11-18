using QueueWorker.Application.Interfaces;
using QueueWorker.Application.Services.ConcurrentQueue;
using QueueWorker.Application.Services.LockQueue;
using QueueWorker.Application.Services.Queue;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        #region Regular Queue

        services.AddSingleton<IInMemoryRegularQueueService, InMemoryRegularQueueService>();
        services.AddSingleton<IRegularQueueMessageProducer, RegularQueueMessageProducer>();
        services.AddSingleton<IRegularQueueMessageConsumer, RegularQueueMessageConsumer>();

        #endregion

        #region Lock Queue

        services.AddSingleton<IInMemoryLockQueueService, InMemoryLockQueueService>();
        services.AddSingleton<ILockQueueMessageProducer, LockQueueMessageProducer>();
        services.AddSingleton<ILockQueueMessageConsumer, LockQueueMessageConsumer>();

        #endregion

        #region Concurrent Queue

        services.AddSingleton<IInMemoryConcurrentQueueService, InMemoryConcurrentQueueService>();
        services.AddSingleton<IConcurrentQueueMessageProducer, ConcurrentQueueMessageProducer>();
        services.AddSingleton<IConcurrentQueueMessageConsumer, ConcurrentQueueMessageConsumer>();

        #endregion


        return services;
    }
}