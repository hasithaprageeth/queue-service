using QueueWorker.Application.Interfaces;
using QueueWorker.Domain.Entities;

namespace QueueWorker.Application.Services.Queue;

public class LockQueueMessageProducer(IInMemoryLockQueueService queueService) : ILockQueueMessageProducer
{
    private readonly IInMemoryLockQueueService _queueService = queueService;

    public async Task GenerateQueueMessagesAsync(CancellationToken cancellationToken)
    {
        var i = 1;
        while (!cancellationToken.IsCancellationRequested)
        {
            _queueService.AddToQueue(new UserMessage($"User{i}", $"Message content for user {i}"));
            await Task.Delay(100, cancellationToken);
            i++;
        }
        await Task.CompletedTask;
    }
}
