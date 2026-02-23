using Microsoft.Extensions.DependencyInjection;

namespace DevEstate.Api.Services;

public class XmlFeedScheduler : BackgroundService
{
    private readonly IServiceProvider _sp;
    private readonly ILogger<XmlFeedScheduler> _logger;

    public XmlFeedScheduler(IServiceProvider sp, ILogger<XmlFeedScheduler> logger)
    {
        _sp = sp;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new PeriodicTimer(TimeSpan.FromMinutes(5));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                using var scope = _sp.CreateScope();
                var generator = scope.ServiceProvider.GetRequiredService<XmlFeedGenerationService>();

                await generator.GenerateAsync("dane");

                _logger.LogInformation("✅ XmlFeedScheduler: generated feed at {Time}", DateTimeOffset.Now);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ XmlFeedScheduler error");
            }
        }
    }
}