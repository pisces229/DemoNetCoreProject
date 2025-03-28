using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DemoNetCoreProject.Batch.Runners
{
    internal class DefaultRunner(ILogger<DefaultRunner> _logger,
        IConfiguration _configuration,
        [FromKeyedServices(EnumRunner.First)] IEnumRunner _firstEnumRunner,
        [FromKeyedServices(EnumRunner.Second)] IEnumRunner _secondEnumRunner,
        IDefaultSqlLogic _defaultLogic) : IRunner
    {
        public async Task Run()
        {
            try
            {
                _logger.LogInformation("DefaultRunner Start");

                await _firstEnumRunner.Run();
                await _secondEnumRunner.Run();

                // Define the cancellation token.
                using (var cancellationTokenSource = new CancellationTokenSource())
                {
                    var cancellationToken = cancellationTokenSource.Token;
                    cancellationTokenSource.Cancel();
                }
                Console.WriteLine(_configuration.GetValue<string>("Path:Temp"));

                await _defaultLogic.RunSqlCondition();
                await Task.Run(() => _logger.LogInformation("..."));
                await Task.Delay(5000);
                await Task.Run(() => _logger.LogInformation("..."));

                _logger.LogInformation("DefaultRunner End");
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, "Exception");
            }
        }
    }
}
