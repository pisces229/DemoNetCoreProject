using AutoMapper;
using DemoNetCoreProject.BusinessLayer.DtoMappers;
using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using DemoNetCoreProject.BusinessLayer.Logics.Default;
using DemoNetCoreProject.DataLayer.Dtos.Default;
using DemoNetCoreProject.DataLayer.IRepositories.Default;
using DemoNetCoreProject.DataLayer.Repositories.Default;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DemoNetCoreProject.UnitTest.BusinessLayer.Logics.Default
{
    [TestClass]
    public class UnitTest_DefaultFirstLogic : UnitTestInitialize
    {
        public UnitTest_DefaultFirstLogic() : base()
        { 
        }
        [TestInitialize]
        public void Initialize()
        {
            Console.WriteLine("UnitTest_DefaultFirstLogic.Initialize");
        }
        [TestMethod]
        public async Task Run()
        {
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultFirstLogic>();
            var mockDefaultFirstRepository = new Mock<IDefaultFirstRepository>();
            mockDefaultFirstRepository
                .Setup(s => s.Run(It.IsAny<DefaultFirstRepositoryInputDto>()))
                .ReturnsAsync(new DefaultFirstRepositoryOutputDto() { Value = "MockDefaultFirstRepository" });
            var mockDefaultSecondLogic = new Mock<IDefaultSecondLogic>();
            mockDefaultSecondLogic
                .Setup(s => s.Run(It.IsAny<DefaultSecondLogicInputDto>()))
                .ReturnsAsync(new DefaultSecondLogicOutputDto() { Value = "MockDefaultSecondLogic" });
            var mapper = new Mapper(new MapperConfiguration(c => DefaultAutoMapper.Load(c)));
            var defaultFirstLogic = new DefaultFirstLogic(
                logger,
                mockDefaultSecondLogic.Object,
                mockDefaultFirstRepository.Object,
                mapper);
            var result = await defaultFirstLogic.Run(new DefaultFirstLogicInputDto() { Value = "UnitTest" });
            mockDefaultFirstRepository.Verify(s => s.Run(It.IsAny<DefaultFirstRepositoryInputDto>()), Times.Once);
            mockDefaultSecondLogic.Verify(s => s.Run(It.IsAny<DefaultSecondLogicInputDto>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual("MockDefaultSecondLogic", result.Data.Value);
        }
    }
}