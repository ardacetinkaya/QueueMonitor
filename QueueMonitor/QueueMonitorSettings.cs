public class QueueMonitorSetting
{
    public string Title { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string ConnectionString { get; set; }
}

public class QueueMonitorSettings
{
    public const string Queue = "Queue";

    public IEnumerable<QueueMonitorSetting> Settings { get; set; }
}