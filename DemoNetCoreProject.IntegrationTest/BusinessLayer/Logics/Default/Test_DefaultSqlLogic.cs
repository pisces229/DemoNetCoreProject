using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DemoNetCoreProject.IntegrationTest.BusinessLayer.Logics.Default
{
    [TestClass]
    public class Test_DefaultSqlLogic : Test_Initialize
    {
        private readonly ILogger<Test_DefaultSqlLogic> _logger;
        private readonly IDefaultSqlLogic _logic;
        public Test_DefaultSqlLogic() : base()
        {
            _logger = _serviceProvider.GetRequiredService<ILogger<Test_DefaultSqlLogic>>();
            _logic = _serviceProvider.GetRequiredService<IDefaultSqlLogic>();
        }
        [TestInitialize]
        public void Initialize()
        {
            _logger.LogInformation("Test_DefaultSqlLogic.Initialize");
        }
        [TestMethod]
        public async Task RunDbRepositoryQuery()
        {
            await _logic.RunDbRepositoryQuery();
        }
        [TestMethod]
        public async Task RunDbRepositoryCreate()
        {
            await _logic.RunDbRepositoryCreate();
        }
        [TestMethod]
        public async Task RunDbRepositoryModify()
        {
            await _logic.RunDbRepositoryModify();
        }
        [TestMethod]
        public async Task RunDbRepositoryRemove()
        {
            await _logic.RunDbRepositoryRemove();
        }
        [TestMethod]
        public async Task RunDbRepositoryPagedQuery()
        {
            await _logic.RunDbRepositoryPagedQuery();
        }
        [TestMethod]
        public async Task RunDapperQuery()
        {
            await _logic.RunDapperQuery();
        }
        [TestMethod]
        public async Task RunDapperExecuteScalar()
        {
            await _logic.RunDapperExecuteScalar();
        }
        [TestMethod]
        public async Task RunDapperQueryMultiple()
        {
            await _logic.RunDapperQueryMultiple();
        }
        [TestMethod]
        public async Task RunDapperExecuteReader()
        {
            await _logic.RunDapperExecuteReader();
        }
        [TestMethod]
        public async Task RunDapperPagedQuery()
        {
            await _logic.RunDapperPagedQuery();
        }
        [TestMethod]
        public async Task RunSqlCondition()
        {
            await _logic.RunSqlCondition();
        }
    }
}