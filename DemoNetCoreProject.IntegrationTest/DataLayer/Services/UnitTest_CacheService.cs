using DemoNetCoreProject.DataLayer.IServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemoNetCoreProject.IntegrationTest.Domain.Services
{
    [TestClass]
    public class UnitTest_CacheService : UnitTestInitialize
    {
        [TestMethod]
        public async Task TestMethod()
        {
            var service = this.Host.Services.GetRequiredService<ICacheService>();
            await service.Add("key", "value", TimeSpan.FromSeconds(1));
            Console.WriteLine(await service.Get<string>("key"));
        }
    }
}