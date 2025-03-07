using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DemoNetCoreProject.Batch
{
    internal class HostedService : IHostedService
    {
        private int? _exitCode;
        private readonly ILogger<HostedService> _logger;
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly IRunner _runner;
        public HostedService(ILogger<HostedService> logger,
            IHostApplicationLifetime applicationLifetime,
            IRunner runner)
        {
            _logger = logger;
            _applicationLifetime = applicationLifetime;
            _runner = runner;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("HostedService Start:[{time}]", DateTimeOffset.Now);
                await _runner.Run();
                _exitCode = 1;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "HostedService Exception!");
                _exitCode = 1;
            }
            finally
            {
                _applicationLifetime.StopApplication();
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("HostedService Stop:[{time}]", DateTimeOffset.Now);
            Environment.ExitCode = _exitCode.GetValueOrDefault(-1);
            return Task.CompletedTask;
        }
    }
}
