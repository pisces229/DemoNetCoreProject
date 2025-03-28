using Microsoft.Extensions.Logging;

namespace DemoNetCoreProject.Batch.EnumRunners
{
    internal class FirstEnumRunner(ILogger<FirstEnumRunner> _logger) : IEnumRunner
    {
        public Task Run() => Task.Run(() =>_logger.LogInformation("FirstEnumRunner.Run"));
    }
}
