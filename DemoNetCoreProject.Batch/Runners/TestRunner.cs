using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DemoNetCoreProject.Batch.Runners
{
    internal class TestRunner : IRunner
    {
        private readonly ILogger<TestRunner> _logger;
        private readonly IConfiguration _configuration;
        public TestRunner(ILogger<TestRunner> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public async Task Run()
        {
            try
            {
                _logger.LogInformation(_configuration.GetValue<string>("Path:Temp"));
                await Task.Run(() => _logger.LogInformation("Run"));
                Thread.Sleep(3000);
            }
            catch (Exception e)
            { 
                _logger.LogError(e.ToString());
            }
        }
    }
}
