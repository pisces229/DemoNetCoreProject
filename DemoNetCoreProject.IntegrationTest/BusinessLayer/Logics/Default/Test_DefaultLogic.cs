using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using Microsoft.Extensions.Logging;

namespace DemoNetCoreProject.IntegrationTest.BusinessLayer.Logics.Default
{
    [TestClass]
    public class Test_DefaultLogic : Test_Initialize
    {
        private readonly ILogger<Test_DefaultLogic> _logger;
        private readonly IDefaultLogic _logic;
        public Test_DefaultLogic() : base()
        {
            _logger = _serviceProvider.GetRequiredService<ILogger<Test_DefaultLogic>>();
            _logic = _serviceProvider.GetRequiredService<IDefaultLogic>();
        }
        [TestInitialize]
        public void Initialize()
        {
            _logger.LogInformation("UnitTest_DefaultLogic.Initialize");
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