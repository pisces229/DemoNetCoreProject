using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DemoNetCoreProject.Batch.Runners
{
    internal class DefaultRunner : IRunner
    {
        private readonly ILogger<DefaultRunner> _logger;
        private readonly IConfiguration _configuration;
        private readonly IDefaultSqlLogic _defaultLogic;
        public DefaultRunner(ILogger<DefaultRunner> logger,
            IConfiguration configuration,
            IDefaultSqlLogic defaultLogic)
        {
            _logger = logger;
            _configuration = configuration;
            _defaultLogic = defaultLogic;
        }
        public async Task Run()
        {
            try
            {
                _logger.LogInformation("DefaultRunner Start");
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
