using DemoNetCoreProject.DataLayer.IServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemoNetCoreProject.IntegrationTest.Domain.Services
{
    [TestClass]
    public class Test_Cache : TestInitialize
    {
        private readonly ILogger<Test_Cache> _logger;
        private readonly ICache _cache;
        public Test_Cache() : base()
        {
            _logger = _serviceProvider.GetRequiredService<ILogger<Test_Cache>>();
            _cache = _serviceProvider.GetRequiredService<ICache>();
        }
        [TestInitialize]
        public void Initialize()
        {
            _logger.LogInformation("UnitTest_Cache.Initialize");
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