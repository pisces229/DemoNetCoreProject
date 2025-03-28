using Microsoft.Extensions.Logging;

namespace DemoNetCoreProject.Batch.EnumRunners
{
    internal class SecondEnumRunner(ILogger<SecondEnumRunner> _logger) : IEnumRunner
    {
        public Task Run() => Task.Run(() =>_logger.LogInformation("SecondEnumRunner.Run"));
    }
}
