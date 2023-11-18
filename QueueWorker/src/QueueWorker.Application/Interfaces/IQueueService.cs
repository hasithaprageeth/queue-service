using QueueWorker.Domain.Entities;

namespace QueueWorker.Application.Interfaces;

public interface IQueueService
{
    UserMessage? ReadFromQueue();

    void AddToQueue(UserMessage userMessage);
}

public interface IInMemoryRegularQueueService : IQueueService
{
}

public interface IInMemoryLockQueueService : IQueueService
{
}

public interface IInMemoryConcurrentQueueService : IQueueService
{
}
