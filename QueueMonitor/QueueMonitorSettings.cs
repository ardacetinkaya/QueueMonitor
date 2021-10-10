public class QueueMonitorSetting
{
    public string Title { get; set; }
    public string ChartDescription { get; set; }
    public string Color { get; set; }
    public string QueueName { get; set; }
    public string Type { get; set; }
    public int Threshold { get; set; }
    public string ConnectionString { get; set; }
}

public class QueueMonitorSettings
{
    public const string Queue = "Charts";

    public IEnumerable<QueueMonitorSetting> Settings { get; set; }
}

public class QueueMonitorSettingDTO
{
    public string Title { get; internal set; }
    public string ChartDescription { get; internal set; }
    public string Color { get; internal set; }
    public string QueueName { get; internal set; }
    public int Threshold { get; internal set; }
}