using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using Microsoft.Extensions.Logging;

namespace DemoNetCoreProject.IntegrationTest.BusinessLayer.Logics.Default
{
    [TestClass]
    public class UnitTest_DefaultLogic : UnitTestInitialize
    {
        private readonly ILogger<UnitTest_DefaultLogic> _logger;
        private readonly IDefaultLogic _logic;
        public UnitTest_DefaultLogic() : base()
        {
            _logger = this._host.Services.GetRequiredService<ILogger<UnitTest_DefaultLogic>>();
            _logic = this._host.Services.GetRequiredService<IDefaultLogic>();
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
            // Assert.Fail();
        }
        [TestMethod]
        public async Task RunDbRepositoryCreate()
        {
            await _logic.RunDbRepositoryCreate();
            // Assert.Fail();
        }
        [TestMethod]
        public async Task RunDbRepositoryModify()
        {
            await _logic.RunDbRepositoryModify();
            // Assert.Fail();
        }
        [TestMethod]
        public async Task RunDbRepositoryRemove()
        {
            await _logic.RunDbRepositoryRemove();
            // Assert.Fail();
        }
        [TestMethod]
        public async Task RunDbRepositoryPagedQuery()
        {
            await _logic.RunDbRepositoryPagedQuery();
            // Assert.Fail();
        }
        [TestMethod]
        public async Task RunDapperQuery()
        {
            await _logic.RunDapperQuery();
            // Assert.Fail();
        }
        [TestMethod]
        public async Task RunDapperExecuteScalar()
        {
            await _logic.RunDapperExecuteScalar();
            // Assert.Fail();
        }
        [TestMethod]
        public async Task RunDapperQueryMultiple()
        {
            await _logic.RunDapperQueryMultiple();
            // Assert.Fail();
        }
        [TestMethod]
        public async Task RunDapperExecuteReader()
        {
            await _logic.RunDapperExecuteReader();
            // Assert.Fail();
        }
        [TestMethod]
        public async Task RunDapperPagedQuery()
        {
            await _logic.RunDapperPagedQuery();
            // Assert.Fail();
        }
        [TestMethod]
        public async Task RunSqlCondition()
        {
            await _logic.RunSqlCondition();
            // Assert.Fail();
        }
    }
}