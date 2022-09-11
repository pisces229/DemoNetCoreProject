using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using DemoNetCoreProject.BusinessLayer.Logics.Default;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DemoNetCoreProject.UnitTest.BusinessLayer.Logics.Default
{
    [TestClass]
    public class UnitTest_DefaultFirstLogic : UnitTestInitialize
    {
        [TestMethod]
        public async Task Run()
        {
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultFirstLogic>();
            var mockDefaultCommonLogic = new Mock<IDefaultCommonLogic>();
            mockDefaultCommonLogic
                .Setup(s => s.Run(It.IsAny<DefaultCommonLogicInputDto>()))
                .ReturnsAsync(new DefaultCommonLogicOutputDto());
            var defaultFirstLogic = new DefaultFirstLogic(
                logger,
                mockDefaultCommonLogic.Object);
            await defaultFirstLogic.Run(new DefaultFirstLogicInputDto());
            mockDefaultCommonLogic.Verify(s => s.Run(It.IsAny<DefaultCommonLogicInputDto>()), Times.Once);
            //Assert.IsNotNull(output);
            //Assert.IsTrue(output.Success);
        }
    }
}