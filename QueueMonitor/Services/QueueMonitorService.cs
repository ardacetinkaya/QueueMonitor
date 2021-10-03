using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.AspNetCore.SignalR;
using QueueMonitor.Hubs;

public interface IQueuePlatform
{
    void Initialize(string queueName,string connectionString);
    int GetMessageCount();
}

public class QueueMonitorService<T> :IHostedService, IDisposable where T: IQueuePlatform, new()
{
    private string _queueName;
    private Timer _timer;
    private readonly ILogger<QueueMonitorService<T>> _logger;
    private readonly IHubContext<QueueHub> _hubContext;
    private readonly T _queueCheck;

    public QueueMonitorService(string queueName, string connectionString
        , ILogger<QueueMonitorService<T>> logger
        , IHubContext<QueueHub> hubContext)
    {
        _logger = logger;
        _hubContext = hubContext;
        _queueCheck = new T();
        _queueCheck.Initialize(queueName, connectionString);
        _queueName = queueName;
    }

    public void Dispose()
    {
         _timer?.Dispose();
    }

    private void DoWork(object state)
    {  
        int count = _queueCheck.GetMessageCount();
        _hubContext.Clients.All.SendAsync("ReceiveMessage", _queueName, count);
        _logger.LogInformation($"Monitor Service for {_queueName} is working. Count: {count}");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Monitor Service for {_queueName} running.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero, 
            TimeSpan.FromSeconds(5));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Monitor Service for {_queueName} is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }
}

internal class AzQueueCheck: IQueuePlatform
{
    private QueueClient _queueClient;
    private bool _isInitialized = false;
    
    public int GetMessageCount()
    {

        int messageCount = 0;
        if (_isInitialized && _queueClient.Exists())
        {
            QueueProperties properties = _queueClient.GetProperties();
            messageCount = properties.ApproximateMessagesCount;
        }

        return messageCount;
    }

    public void Initialize(string queueName,string connectionString)
    {
        _queueClient = new QueueClient(connectionString, queueName);
        _isInitialized = true;
    }
}