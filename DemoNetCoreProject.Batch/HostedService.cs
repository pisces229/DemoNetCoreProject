using DemoNetCoreProject.Batch.Runners;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DemoNetCoreProject.Batch
{
    internal class HostedService(ILogger<HostedService> _logger,
        IHostApplicationLifetime _applicationLifetime,
        IRunner _runner,
        ChannelRunner _channelRunner) : IHostedService
    {
        private int? _exitCode;
        private readonly ILogger<HostedService> _logger = _logger;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("HostedService Start:[{time}]", DateTimeOffset.Now);
                await _runner.Run();

                while (true)
                {
                    await Task.WhenAll([
                        _channelRunner.Producer(), 
                        _channelRunner.Consumer(1), 
                        _channelRunner.Consumer(2),
                        _channelRunner.Consumer(3),
                        _channelRunner.Consumer(4),
                        _channelRunner.Consumer(5),
                    ]);
                }
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