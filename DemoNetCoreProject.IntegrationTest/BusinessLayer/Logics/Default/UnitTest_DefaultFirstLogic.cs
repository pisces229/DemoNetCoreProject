using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemoNetCoreProject.IntegrationTest.BusinessLayer.Logics.Default
{
    [TestClass]
    public class UnitTest_DefaultFirstLogic : UnitTestInitialize
    {
        private readonly ILogger<UnitTest_DefaultLogic> _logger;
        private readonly IDefaultFirstLogic _logic;
        public UnitTest_DefaultFirstLogic() : base()
        {
            _logger = _serviceProvider.GetRequiredService<ILogger<UnitTest_DefaultLogic>>();
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
