namespace QueueWorker.Application.Interfaces;

public interface IQueueMessageConsumer
{
    Task ProcessQueueMessagesAsync(CancellationToken cancellationToken);
}

public interface IRegularQueueMessageConsumer : IQueueMessageConsumer
{
}

public interface ILockQueueMessageConsumer : IQueueMessageConsumer
{
}

public interface IConcurrentQueueMessageConsumer : IQueueMessageConsumer
{
}
