using DemoNetCoreProject.DataLayer.IRepositories.Http;
using Microsoft.Extensions.Logging;

namespace DemoNetCoreProject.DataLayer.Repositories.Http
{
    internal class DefaultHttpRepository : IDefaultHttpRepository
    {
        private readonly ILogger<DefaultHttpRepository> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public DefaultHttpRepository(
            ILogger<DefaultHttpRepository> logger,
            IHttpClientFactory httpClientFactory) 
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }
        public async Task Run()
        {
            try
            {
                var responseString = await _httpClientFactory
                    .CreateClient("Default")
                    .GetStringAsync("run");
                _logger.LogInformation(responseString);
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, "");
            }
        }
    }
}
