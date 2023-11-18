using Microsoft.Extensions.Logging;
using QueueWorker.Application.Interfaces;
using QueueWorker.Domain.Constants;

namespace QueueWorker.Application.Services.Queue;

public class ConcurrentQueueMessageConsumer(ILogger<ConcurrentQueueMessageConsumer> logger, IInMemoryConcurrentQueueService queueService) : IConcurrentQueueMessageConsumer
{
    private readonly ILogger<ConcurrentQueueMessageConsumer> _logger = logger;
    private readonly IInMemoryConcurrentQueueService _queueService = queueService;
    private readonly SemaphoreSlim _semaphoreSlim = new(QueueConstants.MaxThreadCount);

    public async Task ProcessQueueMessagesAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(QueueConstants.QueueReadDelay, cancellationToken);

        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.WhenAll(Enumerable.Range(1, QueueConstants.UserMessageCount).Select(async i =>
            {
                await _semaphoreSlim.WaitAsync();
                try
                {
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        var message = _queueService.ReadFromQueue();
                        if (message is not null)
                        {
                            _logger.LogInformation("{message}", message.GetUserMessage());
                        }
                    }
                }
                finally
                {
                    _semaphoreSlim.Release();
                }
            }).ToArray());

            await Task.Delay(QueueConstants.QueueReadDelay, cancellationToken);
        }
    }
}
