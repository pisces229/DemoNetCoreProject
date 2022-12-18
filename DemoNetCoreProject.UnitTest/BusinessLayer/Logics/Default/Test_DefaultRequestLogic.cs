using AutoMapper;
using DemoNetCoreProject.BusinessLayer.DtoMappers;
using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using DemoNetCoreProject.BusinessLayer.Logics.Default;
using DemoNetCoreProject.Common.Dtos;
using DemoNetCoreProject.Common.Options;
using DemoNetCoreProject.DataLayer.Dtos.Default;
using DemoNetCoreProject.DataLayer.IRepositories.Default;
using DemoNetCoreProject.DataLayer.IServices;
using DemoNetCoreProject.DataLayer.Repositories.Default;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DemoNetCoreProject.UnitTest.BusinessLayer.Logics.Default
{
    [TestClass]
    public class Test_DefaultRequestLogic : Test_Initialize
    {
        public Test_DefaultRequestLogic() : base()
        { 
        }
        [TestInitialize]
        public void Initialize()
        {
            Console.WriteLine("UnitTest_DefaultRequestLogic.Initialize");
        }
        [TestMethod]
        public async Task Upload()
        {
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultRequestLogic>();
            var mockDefaultRequestRepository = new Mock<IDefaultRequestRepository>();
            mockDefaultRequestRepository
                .Setup(s => s.Upload(It.IsAny<DefaultRequestRepositoryUploadInputDto>()))
                .ReturnsAsync(true);
            var mapper = new Mapper(new MapperConfiguration(c => DefaultAutoMapper.Load(c)));
            var mockOptions = new Mock<IOptions<JwtOption>>();
            var mockUserService = new Mock<IUserService>();
            var mockCache = new Mock<ICache>();
            var defaultFirstLogic = new DefaultRequestLogic(
                logger,
                mockDefaultRequestRepository.Object,
                mapper,
                mockOptions.Object,
                mockUserService.Object,
                mockCache.Object);
            var result = await defaultFirstLogic.Upload(new DefaultRequestLogicUploadInputDto() 
            { 
                Name = "UploadName",
                FileName = "UploadName.txt",
                File = new MemoryStream()
            });
            mockDefaultRequestRepository.Verify(s => s.Upload(It.IsAny<DefaultRequestRepositoryUploadInputDto>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }
        [TestMethod]
        public async Task Download()
        {
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultRequestLogic>();
            var mockDefaultRequestRepository = new Mock<IDefaultRequestRepository>();
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
            var mapper = new Mapper(new MapperConfiguration(c => DefaultAutoMapper.Load(c)));
            var mockOptions = new Mock<IOptions<JwtOption>>();
            var mockUserService = new Mock<IUserService>();
            var mockCache = new Mock<ICache>();
            var defaultFirstLogic = new DefaultRequestLogic(
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