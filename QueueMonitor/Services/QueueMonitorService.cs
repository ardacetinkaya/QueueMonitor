using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using QueueMonitor.Hubs;

public interface IQueuePlatform
{
    void Initialize(string queueName,string connectionString);
    int GetMessageCount();
}

public class QueueMonitorService<T> :IHostedService, IDisposable where T: IQueuePlatform, new()
{
    private string _queueName;
    private Timer? _timer;
    private readonly ILogger<QueueMonitorService<T>> _logger;
    private readonly IHubContext<QueueHub> _hubContext;
    private readonly T _queueCheck;
    private readonly QueueMonitorSettingDTO? _setting;

    public QueueMonitorService(string queueName, string connectionString
        , ILogger<QueueMonitorService<T>> logger
        , IHubContext<QueueHub> hubContext,IOptions<QueueMonitorSettings> options)
    {
        _logger = logger;
        _hubContext = hubContext;
        _queueCheck = new T();
        _queueCheck.Initialize(queueName, connectionString);
        _queueName = queueName;

        _setting = options.Value.Settings.Where(s => s.QueueName == _queueName)
            .Select(s=>new QueueMonitorSettingDTO {
                ChartDescription = s.ChartDescription,
                Color = s.Color,
                QueueName = s.QueueName,
                Title = s.Title,
                Threshold = s.Threshold
            })
            .FirstOrDefault();
    }

    public void Dispose()
    {
         _timer?.Dispose();
    }

    private void CheckMessageCount(object state)
    {  
        int count = _queueCheck.GetMessageCount();

        _hubContext.Clients.All.SendAsync("ReceiveMessage", _queueName, count,_setting);

        _logger.LogInformation($"Monitor Service for '{_queueName}' is working. Count: {count}");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Monitor Service for '{_queueName}' running...");

        _timer = new Timer(CheckMessageCount, null, TimeSpan.Zero, 
            TimeSpan.FromSeconds(5));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Monitor Service for '{_queueName}' is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }
}
