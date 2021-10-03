var builder = WebApplication.CreateBuilder(args);

QueueMonitorSettings settings = new QueueMonitorSettings();
builder.Configuration.GetSection(QueueMonitorSettings.Queue).Bind(settings);

// Add services to the container.
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddSignalR();

foreach (var item in settings.Settings)
{
    builder.Services.AddSingleton<IHostedService>(x =>
        ActivatorUtilities.CreateInstance<QueueMonitorService<AzQueueCheck>>(x, item.QueueName, item.ConnectionString)
    );
}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapHub<QueueMonitor.Hubs.QueueHub>("/queueHub");
});

app.Run();
