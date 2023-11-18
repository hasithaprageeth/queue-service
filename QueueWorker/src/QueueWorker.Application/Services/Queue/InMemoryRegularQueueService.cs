using Microsoft.Extensions.Logging;
using QueueWorker.Application.Interfaces;
using QueueWorker.Domain.Entities;

namespace QueueWorker.Application.Services.Queue;

public class InMemoryRegularQueueService(ILogger<InMemoryRegularQueueService> logger) : IInMemoryRegularQueueService
{
    private readonly ILogger<InMemoryRegularQueueService> _logger = logger;
    private readonly Queue<UserMessage> MessageQueue = new();

    public UserMessage? ReadFromQueue()
    {
        try
        {
            if (MessageQueue.TryDequeue(out var userMessage))
            {
                return userMessage;
            }
            else
            {
                _logger.LogInformation("Queue is empty. All user messages processed.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to read message from the queue: {exception}", ex.Message);
        }
        return default;
    }

    public void AddToQueue(UserMessage userMessage)
    {
        try
        {
            MessageQueue.Enqueue(userMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to add message to the queue: {exception}", ex.Message);
        }
    }
}
