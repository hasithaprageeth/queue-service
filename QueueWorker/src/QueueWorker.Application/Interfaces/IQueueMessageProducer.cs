namespace QueueWorker.Application.Interfaces;

public interface IQueueMessageProducer
{
    Task GenerateQueueMessagesAsync(CancellationToken cancellationToken);
}

public interface IRegularQueueMessageProducer : IQueueMessageProducer
{
}

public interface ILockQueueMessageProducer : IQueueMessageProducer
{
}

public interface IConcurrentQueueMessageProducer : IQueueMessageProducer
{
}
