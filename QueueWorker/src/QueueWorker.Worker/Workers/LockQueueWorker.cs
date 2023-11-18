using QueueWorker.Application.Interfaces;

namespace QueueWorker.Worker.Workers;

public class LockQueueWorker(ILogger<LockQueueWorker> logger, ILockQueueMessageProducer queueMessageProducer, 
    ILockQueueMessageConsumer queueMessageConsumer) : BackgroundService
{
    private readonly ILogger<LockQueueWorker> _logger = logger;
    private readonly ILockQueueMessageProducer _queueMessageProducer = queueMessageProducer;
    private readonly ILockQueueMessageConsumer _queueMessageConsumer = queueMessageConsumer;


    protected override  async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Started Lock Queue Worker at: {time}", DateTimeOffset.Now);

        // Generate messages to the queue
        Task genDataTask = Task.Run(async () => 
            await _queueMessageProducer.GenerateQueueMessagesAsync(stoppingToken), stoppingToken);

        // Process messages from the queue
        Task processDataTask = Task.Run(async () => 
            await _queueMessageConsumer.ProcessQueueMessagesAsync(stoppingToken), stoppingToken);

        await Task.WhenAll(new List<Task> { genDataTask, processDataTask });

        _logger.LogInformation("Completed Lock Queue Worker at: {time}", DateTimeOffset.Now);
    }
}
