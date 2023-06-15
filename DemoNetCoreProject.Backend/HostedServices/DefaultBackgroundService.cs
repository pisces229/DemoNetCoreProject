namespace DemoNetCoreProject.Backend.HostedServices
{
    public class DefaultBackgroundService : BackgroundService
    {
        private readonly ILogger<DefaultBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;
        public DefaultBackgroundService(ILogger<DefaultBackgroundService> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("DefaultBackgroundService.StartAsync");
            base.StartAsync(cancellationToken);
            return Task.CompletedTask;
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("DefaultBackgroundService.ExecuteAsync");
            do
            {
                using (var serviceScope = _serviceProvider.CreateScope())
                {
                    //var service = serviceScope.ServiceProvider.GetRequiredService<IScopedProcessingService>();
                    //await scopedProcessingService.DoWork(stoppingToken);
                    await Task.Run(() => _logger.LogInformation("DefaultBackgroundService.ExecuteAsync.Run"), cancellationToken);
                }
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
            while (!cancellationToken.IsCancellationRequested);
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("DefaultBackgroundService.StopAsync");
            base.StopAsync(cancellationToken);
            return Task.CompletedTask;
        }
        public override void Dispose()
        {
            _logger.LogInformation("DefaultBackgroundService.Dispose");
            base.Dispose();
        }
    }
}
