using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using DemoNetCoreProject.BusinessLayer.ILogics.Default;

namespace DemoNetCoreProject.IntegrationTest.BusinessLayer.Logics.Default
{
    [TestClass]
    public class UnitTest_DefaultLogic : UnitTestInitialize
    {
        [TestMethod]
        public async Task RunRepository()
        {
            var logic = this.Host.Services.GetRequiredService<IDefaultLogic>();
            await logic.RunRepository();
        }
        [TestMethod]
        public async Task RunSqlStatement()
        {
            var logic = this.Host.Services.GetRequiredService<IDefaultLogic>();
            await logic.RunSqlStatement();
        }
        [TestMethod]
        public async Task RunSqlCondition()
        {
            var logic = this.Host.Services.GetRequiredService<IDefaultLogic>();
            await logic.RunSqlCondition();
        }
    }
}