using QueueWorker.Application.Interfaces;

namespace QueueWorker.Worker.Workers;

public class RegularQueueWorker(ILogger<RegularQueueWorker> logger, IRegularQueueMessageProducer queueMessageProducer, 
    IRegularQueueMessageConsumer queueMessageConsumer) : BackgroundService
{
    private readonly ILogger<RegularQueueWorker> _logger = logger;
    private readonly IRegularQueueMessageProducer _queueMessageProducer = queueMessageProducer;
    private readonly IRegularQueueMessageConsumer _queueMessageConsumer = queueMessageConsumer;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Generate messages to the queue
        await _queueMessageProducer.GenerateQueueMessagesAsync(stoppingToken);

        _logger.LogInformation("Started Regular Queue Worker at: {time}", DateTimeOffset.Now);

        // Process messages from the queue
        await _queueMessageConsumer.ProcessQueueMessagesAsync(stoppingToken);

        _logger.LogInformation("Completed Regular Queue Worker at: {time}", DateTimeOffset.Now);
    }
}
