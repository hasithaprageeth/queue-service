using Microsoft.Extensions.Logging;
using QueueWorker.Application.Interfaces;
using QueueWorker.Domain.Constants;
using QueueWorker.Domain.Entities;

namespace QueueWorker.Application.Services.Queue;

public class RegularQueueMessageProducer(ILogger<RegularQueueMessageProducer> logger, IInMemoryRegularQueueService queueService) : IRegularQueueMessageProducer
{
    private readonly ILogger<RegularQueueMessageProducer> _logger = logger;
    private readonly IInMemoryRegularQueueService _queueService = queueService;

    public async Task GenerateQueueMessagesAsync(CancellationToken cancellationToken)
    {     
        foreach (var i in Enumerable.Range(1, QueueConstants.UserMessageCount))
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                _queueService.AddToQueue(new UserMessage($"User{i}", $"Message content for user {i}"));
            }            
        }
        _logger.LogInformation("Generated {MessageCount} user messages to Regular Queue.", QueueConstants.UserMessageCount);
        await Task.CompletedTask;
    }
}
