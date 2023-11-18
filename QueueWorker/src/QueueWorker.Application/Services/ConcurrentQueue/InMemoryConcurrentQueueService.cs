using Microsoft.Extensions.Logging;
using QueueWorker.Application.Interfaces;
using QueueWorker.Domain.Entities;
using System.Collections.Concurrent;

namespace QueueWorker.Application.Services.ConcurrentQueue;

public class InMemoryConcurrentQueueService(ILogger<InMemoryConcurrentQueueService> logger) : IInMemoryConcurrentQueueService
{
    private readonly ILogger<InMemoryConcurrentQueueService> _logger = logger;
    // Thread safe data structures designed to use in multi-threaded applications, ensures safe enqueueing and dequeuing operations, prevents data corruption and race conditions
    private readonly ConcurrentQueue<UserMessage> MessageQueue = new();

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
                _logger.LogInformation("Concurrent queue is empty. All user messages processed.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to read message from the concurrent queue: {exception}", ex.Message);
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
            _logger.LogError("Failed to add message to the concurrent queue: {exception}", ex.Message);
        }
    }
}
