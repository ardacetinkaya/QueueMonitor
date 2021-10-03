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
            QueueName = s.Name.ToLowerInvariant()            
        }).ToList();
    }

    public void OnGet()
    {

    }
}

public class Chart
{
    
    public string Title { get; set; }
    public string QueueName { get; set; }


    public string GenerateScript()
    {
        var variableName = Title;
        return @$"
var data_{Title} = new google.visualization.DataTable();
data_{Title}.addColumn('string', 'Level');
data_{Title}.addColumn('number', 'Count');
data_{Title}.addRows([['{Title}', message]]);

chart_{Title}.draw(data_{Title}, options);
";
    }
}
