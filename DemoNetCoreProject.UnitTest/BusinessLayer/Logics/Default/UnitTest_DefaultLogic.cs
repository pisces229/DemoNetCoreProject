using AutoMapper;
using DemoNetCoreProject.BusinessLayer.DtoMappers;
using DemoNetCoreProject.BusinessLayer.Logics.Default;
using DemoNetCoreProject.Common.Dtos;
using DemoNetCoreProject.DataLayer.Entities;
using DemoNetCoreProject.DataLayer.IRepositories.Db;
using DemoNetCoreProject.DataLayer.IRepositories.Default;
using DemoNetCoreProject.DataLayer.IServices;
using DemoNetCoreProject.DataLayer.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DemoNetCoreProject.UnitTest.BusinessLayer.Logics.Default
{
    [TestClass]
    public class UnitTest_DefaultLogic : UnitTestInitialize
    {
        public UnitTest_DefaultLogic() : base()
        {
        }
        [TestInitialize]
        public void Initialize()
        {
            Console.WriteLine("UnitTest_DefaultLogic.Initialize");
        }
        [TestMethod]
        public void Run()
        {
            // Arrange
            // ...Mock
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultLogic>();
            var mapper = new Mapper(new MapperConfiguration(c => DefaultAutoMapper.Load(c)));
            // Act
            // ...Run
            // Assert
            // ...Verify
            Assert.IsTrue(true);
        }
        [TestMethod]
        public async Task RunDbRepositoryQuery()
        {
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultLogic>();
            var mockDefaultDbManager = new Mock<IDbManager<DefaultDbContext>>();
            var mockDefaultCustomerDbRepository = new Mock<IDefaultCustomerDbRepository>();
            mockDefaultCustomerDbRepository
                .Setup(s => s.Query(
                    It.IsAny<bool>(),
                    It.IsAny<Func<IQueryable<Customer>, IQueryable<Customer>>>(),
                    It.IsAny<Func<IQueryable<Customer>, IOrderedQueryable<Customer>>>()))
                .ReturnsAsync(new List<Customer>() { new Customer() });
            var mockDefaultRepository = new Mock<IDefaultRepository>();
            var defaultLogic = new DefaultLogic(
                logger,
                mockDefaultDbManager.Object,
                mockDefaultCustomerDbRepository.Object,
                mockDefaultRepository.Object);
            await defaultLogic.RunDbRepositoryQuery();
            mockDefaultCustomerDbRepository.Verify(v => v.Query(
                It.IsAny<bool>(),
                It.IsAny<Func<IQueryable<Customer>, IQueryable<Customer>>>(),
                It.IsAny<Func<IQueryable<Customer>, IOrderedQueryable<Customer>>>())
            , Times.Once);
        }
        [TestMethod]
        public async Task RunDbRepositoryCreate()
        {
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultLogic>();
            var mockDefaultDbManager = new Mock<IDbManager<DefaultDbContext>>();
            mockDefaultDbManager.Setup(s => s.SaveChangesAsync()).ReturnsAsync(1);
            var mockDefaultCustomerDbRepository = new Mock<IDefaultCustomerDbRepository>();
            mockDefaultCustomerDbRepository.Setup(s => s.Create(It.IsAny<Customer>()));
            var mockDefaultRepository = new Mock<IDefaultRepository>();
            var defaultLogic = new DefaultLogic(
                logger,
                mockDefaultDbManager.Object,
                mockDefaultCustomerDbRepository.Object,
                mockDefaultRepository.Object);
            await defaultLogic.RunDbRepositoryCreate();
            mockDefaultCustomerDbRepository.Verify(v => v.Create(It.IsAny<Customer>()), Times.Once);
            mockDefaultDbManager.Verify(s => s.SaveChangesAsync(), Times.Once);
        }
        [TestMethod]
        public async Task RunDbRepositoryModify()
        {
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultLogic>();
            var mockDefaultDbManager = new Mock<IDbManager<DefaultDbContext>>();
            mockDefaultDbManager.Setup(s => s.SaveChangesAsync()).ReturnsAsync(1);
            var mockDefaultCustomerDbRepository = new Mock<IDefaultCustomerDbRepository>();
            mockDefaultCustomerDbRepository
                .Setup(s => s.Query(
                    It.IsAny<bool>(),
                    It.IsAny<Func<IQueryable<Customer>, IQueryable<Customer>>>(),
                    It.IsAny<Func<IQueryable<Customer>, IOrderedQueryable<Customer>>>()))
                .ReturnsAsync(new List<Customer>() { new Customer() });
                //.ReturnsAsync(new List<Customer>());
            mockDefaultCustomerDbRepository.Setup(s => s.Modify(It.IsAny<Customer>()));
            var mockDefaultRepository = new Mock<IDefaultRepository>();
            var defaultLogic = new DefaultLogic(
                logger,
                mockDefaultDbManager.Object,
                mockDefaultCustomerDbRepository.Object,
                mockDefaultRepository.Object);
            await defaultLogic.RunDbRepositoryModify();
            mockDefaultCustomerDbRepository.Verify(v => v.Query(
                It.IsAny<bool>(),
                It.IsAny<Func<IQueryable<Customer>, IQueryable<Customer>>>(),
                It.IsAny<Func<IQueryable<Customer>, IOrderedQueryable<Customer>>>())
            , Times.Once);
            mockDefaultCustomerDbRepository.Verify(v => v.Modify(It.IsAny<Customer>()), Times.Once);
            //mockDefaultCustomerDbRepository.Verify(v => v.Modify(It.IsAny<Customer>()), Times.Never);
            mockDefaultDbManager.Verify(s => s.SaveChangesAsync(), Times.Once);
            //mockDefaultDbManager.Verify(s => s.SaveChangesAsync(), Times.Never);
        }
        [TestMethod]
        public async Task RunDbRepositoryRemove()
        {
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultLogic>();
            var mockDefaultDbManager = new Mock<IDbManager<DefaultDbContext>>();
            mockDefaultDbManager.Setup(s => s.SaveChangesAsync()).ReturnsAsync(1);
            var mockDefaultCustomerDbRepository = new Mock<IDefaultCustomerDbRepository>();
            mockDefaultCustomerDbRepository
                .Setup(s => s.Query(
                    It.IsAny<bool>(),
                    It.IsAny<Func<IQueryable<Customer>, IQueryable<Customer>>>(),
                    It.IsAny<Func<IQueryable<Customer>, IOrderedQueryable<Customer>>>()))
                //.ReturnsAsync(new List<Customer>() { new Customer() });
                .ReturnsAsync(new List<Customer>());
            mockDefaultCustomerDbRepository.Setup(s => s.Remove(It.IsAny<Customer>()));
            var mockDefaultRepository = new Mock<IDefaultRepository>();
            var defaultLogic = new DefaultLogic(
                logger,
                mockDefaultDbManager.Object,
                mockDefaultCustomerDbRepository.Object,
                mockDefaultRepository.Object);
            await defaultLogic.RunDbRepositoryRemove();
            mockDefaultCustomerDbRepository.Verify(v => v.Query(
                It.IsAny<bool>(),
                It.IsAny<Func<IQueryable<Customer>, IQueryable<Customer>>>(),
                It.IsAny<Func<IQueryable<Customer>, IOrderedQueryable<Customer>>>())
            , Times.Once);
            //mockDefaultCustomerDbRepository.Verify(v => v.Remove(It.IsAny<Customer>()), Times.Once);
            mockDefaultCustomerDbRepository.Verify(v => v.Remove(It.IsAny<Customer>()), Times.Never);
            //mockDefaultDbManager.Verify(s => s.SaveChangesAsync(), Times.Once);
            mockDefaultDbManager.Verify(s => s.SaveChangesAsync(), Times.Never);
        }
        [TestMethod]
        public async Task RunDbRepositoryPagedQuery()
        {
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultLogic>();
            var mockDefaultDbManager = new Mock<IDbManager<DefaultDbContext>>();
            mockDefaultDbManager.Setup(s => s.SaveChangesAsync()).ReturnsAsync(1);
            var mockDefaultCustomerDbRepository = new Mock<IDefaultCustomerDbRepository>();
            mockDefaultCustomerDbRepository
                .Setup(s => s.PagedQuery(
                    It.IsAny<CommonPageDto>(),
                    It.IsAny<bool>(),
                    It.IsAny<Func<IQueryable<Customer>, IQueryable<Customer>>>(),
                    It.IsAny<Func<IQueryable<Customer>, IOrderedQueryable<Customer>>>()))
                .ReturnsAsync(new CommonPagedResultDto<Customer>());
            mockDefaultCustomerDbRepository.Setup(s => s.Remove(It.IsAny<Customer>()));
            var mockDefaultRepository = new Mock<IDefaultRepository>();
            var defaultLogic = new DefaultLogic(
                logger,
                mockDefaultDbManager.Object,
                mockDefaultCustomerDbRepository.Object,
                mockDefaultRepository.Object);
            await defaultLogic.RunDbRepositoryPagedQuery();
            mockDefaultCustomerDbRepository.Verify(v => v.PagedQuery(
                It.IsAny<CommonPageDto>(),
                It.IsAny<bool>(),
                It.IsAny<Func<IQueryable<Customer>, IQueryable<Customer>>>(),
                It.IsAny<Func<IQueryable<Customer>, IOrderedQueryable<Customer>>>())
            , Times.Once);
        }
    }
}