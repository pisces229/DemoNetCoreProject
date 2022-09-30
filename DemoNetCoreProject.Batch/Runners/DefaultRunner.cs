using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DemoNetCoreProject.Batch.Runners
{
    internal class DefaultRunner : IRunner
    {
        private readonly ILogger<DefaultRunner> _logger;
        private readonly IConfiguration _configuration;
        private readonly IDefaultLogic _defaultLogic;
        public DefaultRunner(ILogger<DefaultRunner> logger,
            IConfiguration configuration,
            IDefaultLogic defaultLogic)
        {
            _logger = logger;
            _configuration = configuration;
            _defaultLogic = defaultLogic;
        }
        public async Task Run()
        {
            try
            {
                _logger.LogInformation(_configuration.GetValue<string>("Path:Temp"));
                await _defaultLogic.RunSqlCondition();
            }
            catch (Exception e)
            { 
                _logger.LogError(e.ToString());
            }
        }
    }
}
