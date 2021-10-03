using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace QueueMonitor.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IConfiguration _configuration;


    public List<Chart> Charts { get; set; }

    public IndexModel(ILogger<IndexModel> logger,IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;

        QueueMonitorSettings settings = new QueueMonitorSettings();
        _configuration.GetSection(QueueMonitorSettings.Queue).Bind(settings);

        Charts = settings.Settings.Select(s => new Chart
        {
            Title = s.Title.ToLowerInvariant(),
            QueueName = s.QueueName.ToLowerInvariant(),
            Color = s.Color?.ToLowerInvariant(),
            Description = s.ChartDescription
        }).ToList();
    }

    public void OnGet()
    {

    }
}

public class Chart
{
    
    public string Title { get; internal set; }
    public string QueueName { get; internal set; }
    public string? Color { get; internal set; }
    public string Description { get; internal set; }

    public string GenerateOptions()
    {
        var barColor = Color;
        return string.Format(@"
        var options_{0} = {{
            title: '{1}',
            height: 500,
            width: 250,
            vAxis: {{
                format:'decimal',
                viewWindow: {{
                    max: 100,
                    min: 0
                }}
            }},
            legend: {{ position: 'none' }},
            colors: ['{2}']
        }};
        ",Title,Description,barColor);
    }

    public string GenerateScript()
    {
        return @$"
var data_{Title} = new google.visualization.DataTable();
data_{Title}.addColumn('string', 'Level');
data_{Title}.addColumn('number', 'Count');
data_{Title}.addRows([['{Title}', message]]);

chart_{Title}.draw(data_{Title}, options_{Title});
";
    }
}
