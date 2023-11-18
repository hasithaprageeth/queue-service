using Microsoft.Extensions.Logging;
using QueueWorker.Application.Interfaces;
using QueueWorker.Domain.Entities;

namespace QueueWorker.Application.Services.LockQueue;

public class InMemoryLockQueueService(ILogger<InMemoryLockQueueService> logger) : IInMemoryLockQueueService
{
    private readonly ILogger<InMemoryLockQueueService> _logger = logger;
    private readonly Queue<UserMessage> MessageQueue = new();
    private readonly object lockObject = new();

    public UserMessage? ReadFromQueue()
    {
        var userMessage = default(UserMessage?);
        try
        {
            var isSuccess = false;
            lock (lockObject)
            {
                isSuccess = MessageQueue.TryDequeue(out userMessage);
            }

            if (isSuccess)
            {
                return userMessage;
            }
            else
            {
                _logger.LogInformation("Lock queue is empty. All user messages processed.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to read message from the lock queue: {exception}", ex.Message);
        }
        return userMessage;
    }

    public void AddToQueue(UserMessage userMessage)
    {
        try
        {
            lock (lockObject)
            {
                MessageQueue.Enqueue(userMessage);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to add message to the lock queue: {exception}", ex.Message);
        }
    }
}
