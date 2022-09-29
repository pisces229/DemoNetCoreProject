using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using DemoNetCoreProject.DataLayer.IRepositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemoNetCoreProject.IntegrationTest.Domain.Repositories
{
    [TestClass]
    public class UnitTest_DefaultRepository : UnitTestInitialize
    {
        private readonly ILogger<UnitTest_DefaultRepository> _logger;
        private readonly IDefaultRepository _repository;
        public UnitTest_DefaultRepository() : base()
        {
            _logger = this._host.Services.GetRequiredService<ILogger<UnitTest_DefaultRepository>>();
            _repository = this._host.Services.GetRequiredService<IDefaultRepository>();
        }
        [TestInitialize]
        public void Initialize()
        {
            _logger.LogInformation("UnitTest_DefaultRepository.Initialize");
        }
        [TestMethod]
        public async Task TestMethod()
        {
            var value = await _repository.MaxRow();
            _logger.LogInformation(value!.ToString());
            // Assert.Fail();
        }
    }
}