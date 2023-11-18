using Microsoft.Extensions.Logging;
using QueueWorker.Application.Interfaces;

namespace QueueWorker.Application.Services.Queue;

public class RegularQueueMessageConsumer(ILogger<RegularQueueMessageConsumer> logger, IInMemoryRegularQueueService queueService) : IRegularQueueMessageConsumer
{
    private readonly ILogger<RegularQueueMessageConsumer> _logger = logger;
    private readonly IInMemoryRegularQueueService _queueService = queueService;

    public async Task ProcessQueueMessagesAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var message = _queueService.ReadFromQueue();
            if (message is null)
            {
                break;
            }

            _logger.LogInformation("{message}", message.GetUserMessage());
        }
        await Task.CompletedTask;
    }
}
