using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemoNetCoreProject.IntegrationTest.BusinessLayer.Logics.Default
{
    [TestClass]
    public class Test_DefaultFirstLogic : TestInitialize
    {
        private readonly ILogger<Test_DefaultLogic> _logger;
        private readonly IDefaultFirstLogic _logic;
        public Test_DefaultFirstLogic() : base()
        {
            _logger = _serviceProvider.GetRequiredService<ILogger<Test_DefaultLogic>>();
            _logic = _serviceProvider.GetRequiredService<IDefaultFirstLogic>();
        }
        [TestInitialize]
        public void Initialize()
        {
            _logger.LogInformation("UnitTest_DefaultLogic.Initialize");
        }
        [TestMethod]
        public async Task Run()
        {
            await _logic.Run(new DefaultFirstLogicInputDto() { Value = "UnitTest" });
        }
    }
}
