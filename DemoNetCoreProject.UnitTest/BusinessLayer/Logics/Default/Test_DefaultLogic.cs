using AutoMapper;
using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.BusinessLayer.Logics.Default;
using DemoNetCoreProject.BusinessLayer.Profiles;
using DemoNetCoreProject.Common.Dtos;
using DemoNetCoreProject.Common.Options;
using DemoNetCoreProject.DataLayer.Dtos.Default;
using DemoNetCoreProject.DataLayer.IRepositories.Default;
using DemoNetCoreProject.DataLayer.IServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace DemoNetCoreProject.UnitTest.BusinessLayer.Logics.Default
{
    [TestClass]
    public class Test_DefaultLogic : Test_Initialize
    {
        public Test_DefaultLogic() : base()
        {
        }
        [TestInitialize]
        public void Initialize()
        {
            Console.WriteLine("Test_DefaultLogic.Initialize");
        }
        [TestMethod]
        public async Task FromForm()
        {
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultLogic>();
            var mockDefaultRequestRepository = new Mock<IDefaultRepository>();
            mockDefaultRequestRepository
                .Setup(s => s.Upload(It.IsAny<DefaultRepositoryUploadInputDto>()))
                .ReturnsAsync(true);
            var mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<DefaultProfile>()));
            var mockOptions = new Mock<IOptions<JwtOption>>();
            var mockUserService = new Mock<IUserService>();
            var mockCache = new Mock<ICache>();
            var defaultFirstLogic = new DefaultLogic(
                logger,
                mockDefaultRequestRepository.Object,
                mapper,
                mockOptions.Object,
                mockUserService.Object,
                mockCache.Object);
            var result = await defaultFirstLogic.FromForm(new DefaultLogicFromFormInputDto()
            {
                Value = "1",
                Values = ["1", "2"],
                File = new MemoryStream()
            });
            mockDefaultRequestRepository.Verify(s => s.Upload(It.IsAny<DefaultRepositoryUploadInputDto>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }
        [TestMethod]
        public async Task Download()
        {
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultLogic>();
            var mockDefaultRequestRepository = new Mock<IDefaultRepository>();
            mockDefaultRequestRepository
                .Setup(s => s.Download())
                .Returns(new CommonOutputDto<CommonDownloadOutputDto>()
                {
                    Success = true,
                    Data = new CommonDownloadOutputDto()
                    {
                        FileName = "FileName",
                        FilePath = "FilePath",
                    }
                });
            var mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<DefaultProfile>()));
            var mockOptions = new Mock<IOptions<JwtOption>>();
            var mockUserService = new Mock<IUserService>();
            var mockCache = new Mock<ICache>();
            var defaultFirstLogic = new DefaultLogic(
                logger,
                mockDefaultRequestRepository.Object,
                mapper,
                mockOptions.Object,
                mockUserService.Object,
                mockCache.Object);
            var result = await defaultFirstLogic.Download();
            mockDefaultRequestRepository.Verify(s => s.Download(), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }
    }
}