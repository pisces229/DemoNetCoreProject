namespace DemoNetCoreProject.Backend.HostedServices
{
    public class DefaultHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<DefaultHostedService> _logger;
        private Timer? _timer = null;
        private int executionCount = 0;
        public DefaultHostedService(ILogger<DefaultHostedService> logger)
        {
            _logger = logger;
        }
        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("DefaultHostedService.StartAsync");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }
        private void DoWork(object? state)
        {
            var count = Interlocked.Increment(ref executionCount);
            _logger.LogInformation($"DefaultHostedService.DoWork[{count}]");
        }
        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("DefaultHostedService.StopAsync");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _logger.LogInformation("DefaultHostedService.Dispose");
            _timer?.Dispose();
        }
    }
}
