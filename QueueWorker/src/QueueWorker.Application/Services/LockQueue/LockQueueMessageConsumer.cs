using Microsoft.Extensions.Logging;
using QueueWorker.Application.Interfaces;
using QueueWorker.Domain.Constants;

namespace QueueWorker.Application.Services.Queue;

public class LockQueueMessageConsumer(ILogger<LockQueueMessageConsumer> logger, IInMemoryLockQueueService queueService) : ILockQueueMessageConsumer
{
    private readonly ILogger<LockQueueMessageConsumer> _logger = logger;
    private readonly IInMemoryLockQueueService _queueService = queueService;

    public async Task ProcessQueueMessagesAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(QueueConstants.QueueReadDelay, cancellationToken);

        while (!cancellationToken.IsCancellationRequested)
        {
            var message = _queueService.ReadFromQueue();
            if (message is null)
            {
                await Task.Delay(QueueConstants.QueueReadDelay, cancellationToken);
            }
            else
            {
                _logger.LogInformation("{message}", message.GetUserMessage());
            }
        }
        await Task.CompletedTask;
    }
}
