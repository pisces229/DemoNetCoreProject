using DemoNetCoreProject.DataLayer.IRepositories.Default;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemoNetCoreProject.IntegrationTest.DataLayer.Repositories.Default
{
    [TestClass]
    public class UnitTest_DefaultRepository : UnitTestInitialize
    {
        private readonly ILogger<UnitTest_DefaultRepository> _logger;
        private readonly IDefaultRepository _repository;
        public UnitTest_DefaultRepository() : base()
        {
            _logger = _host.Services.GetRequiredService<ILogger<UnitTest_DefaultRepository>>();
            _repository = _host.Services.GetRequiredService<IDefaultRepository>();
        }
        [TestInitialize]
        public void Initialize()
        {
            _logger.LogInformation("UnitTest_DefaultRepository.Initialize");
        }
        [TestMethod]
        public async Task MaxRow()
        {
            var value = await _repository.MaxRow();
            _logger.LogInformation(value!.ToString());
        }
        [TestMethod]
        public async Task RunDapperQuery()
        {
            await _repository.RunDapperQuery();
        }
        [TestMethod]
        public async Task RunDapperExecuteScalar()
        {
            await _repository.RunDapperExecuteScalar();
        }
        [TestMethod]
        public async Task RunDapperQueryMultiple()
        {
            await _repository.RunDapperQueryMultiple();
        }
        [TestMethod]
        public async Task RunDapperExecuteReader()
        {
            await _repository.RunDapperExecuteReader();
        }
        [TestMethod]
        public async Task RunDapperPagedQuery()
        {
            await _repository.RunDapperPagedQuery();
        }
        [TestMethod]
        public async Task RunSqlCondition()
        {
            await _repository.RunSqlCondition();
        }
    }
}