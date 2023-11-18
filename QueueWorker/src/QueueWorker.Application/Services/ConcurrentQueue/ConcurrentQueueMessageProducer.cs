using QueueWorker.Application.Interfaces;
using QueueWorker.Domain.Constants;
using QueueWorker.Domain.Entities;

namespace QueueWorker.Application.Services.Queue;

public class ConcurrentQueueMessageProducer(IInMemoryConcurrentQueueService queueService) : IConcurrentQueueMessageProducer
{
    private readonly IInMemoryConcurrentQueueService _queueService = queueService;
    private readonly SemaphoreSlim _semaphoreSlim = new(QueueConstants.MaxThreadCount);

    public async Task GenerateQueueMessagesAsync(CancellationToken cancellationToken)
    {
        var j = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.WhenAll(Enumerable.Range(1, QueueConstants.UserMessageCount).Select(async i =>
            {
                await _semaphoreSlim.WaitAsync();
                try
                {
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        i = j * QueueConstants.UserMessageCount + i;
                        _queueService.AddToQueue(new UserMessage($"User{i}", $"Message content for user {i}"));
                    }
                }
                finally
                {
                    _semaphoreSlim.Release();
                }
            }).ToArray());
            j++;
        }
    }
}
