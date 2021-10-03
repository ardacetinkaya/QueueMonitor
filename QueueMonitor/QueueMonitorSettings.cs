public class QueueMonitorSetting
{
    public string Title { get; set; }
    public string ChartDescription { get; set; }
    public string Color { get; set; }
    public string QueueName { get; set; }
    public string QueueType { get; set; }
    public int Threshold { get; set; }
    public string ConnectionString { get; set; }
}

public class QueueMonitorSettings
{
    public const string Queue = "Charts";

    public IEnumerable<QueueMonitorSetting> Settings { get; set; }
}