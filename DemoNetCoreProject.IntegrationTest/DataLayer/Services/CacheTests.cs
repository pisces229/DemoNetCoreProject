using DemoNetCoreProject.DataLayer.IServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DemoNetCoreProject.IntegrationTest.Domain.Services
{
    [TestClass]
    public class CacheTests : InitializeTest
    {
        private readonly ILogger<CacheTests> _logger;
        private readonly ICache _cache;
        public CacheTests() : base()
        {
            _logger = _serviceProvider.GetRequiredService<ILogger<CacheTests>>();
            _cache = _serviceProvider.GetRequiredService<ICache>();
        }
        [TestInitialize]
        public void Initialize()
        {
            _logger.LogInformation("Test_Cache.Initialize");
        }
        [TestMethod]
        public async Task TestMethod()
        {
            await _cache.Add("key", "value", TimeSpan.FromSeconds(1));
            _logger.LogInformation((await _cache.Get<string>("key")));
            // Assert.Fail();
        }
    }
}