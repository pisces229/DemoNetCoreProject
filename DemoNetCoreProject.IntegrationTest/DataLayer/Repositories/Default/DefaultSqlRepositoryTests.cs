using DemoNetCoreProject.DataLayer.IRepositories.Default;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DemoNetCoreProject.IntegrationTest.DataLayer.Repositories.Default
{
    [TestClass]
    public class DefaultSqlRepositoryTests : InitializeTest
    {
        private readonly ILogger<DefaultSqlRepositoryTests> _logger;
        private readonly IDefaultSqlRepository _repository;
        public DefaultSqlRepositoryTests() : base()
        {
            _logger = _serviceProvider.GetRequiredService<ILogger<DefaultSqlRepositoryTests>>();
            _repository = _serviceProvider.GetRequiredService<IDefaultSqlRepository>();
        }
        [TestInitialize]
        public void Initialize()
        {
            _logger.LogInformation("Test_DefaultSqlRepository.Initialize");
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