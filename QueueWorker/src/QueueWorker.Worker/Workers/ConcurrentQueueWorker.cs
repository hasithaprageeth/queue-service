using QueueWorker.Application.Interfaces;

namespace QueueWorker.Worker.Workers;

public class ConcurrentQueueWorker(ILogger<ConcurrentQueueWorker> logger, IConcurrentQueueMessageProducer queueMessageProducer,
    IConcurrentQueueMessageConsumer queueMessageConsumer) : BackgroundService
{
    private readonly ILogger<ConcurrentQueueWorker> _logger = logger;
    private readonly IConcurrentQueueMessageProducer _queueMessageProducer = queueMessageProducer;
    private readonly IConcurrentQueueMessageConsumer _queueMessageConsumer = queueMessageConsumer;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Started Concurrent Queue Worker at: {time}", DateTimeOffset.Now);

        // Generate messages to the queue
        Task genDataTask = Task.Run(async () =>
            await _queueMessageProducer.GenerateQueueMessagesAsync(stoppingToken), stoppingToken);

        // Process messages from the queue
        Task processDataTask = Task.Run(async () =>
            await _queueMessageConsumer.ProcessQueueMessagesAsync(stoppingToken), stoppingToken);

        await Task.WhenAll(new List<Task> { genDataTask, processDataTask });

        _logger.LogInformation("Completed Concurrent Queue Worker at: {time}", DateTimeOffset.Now);
    }
}
