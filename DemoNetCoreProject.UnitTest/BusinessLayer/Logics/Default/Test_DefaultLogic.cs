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
using System.Data;

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
            var mockDefaultPersonDbRepository = new Mock<IDefaultPersonDbRepository>();
            mockDefaultPersonDbRepository
                .Setup(s => s.Query(
                    It.IsAny<Func<IQueryable<Person>, IQueryable<Person>>>(),
                    It.IsAny<Func<IQueryable<Person>, IOrderedQueryable<Person>>>()))
                .ReturnsAsync(new List<Person>() { new Person() });
            var mockDefaultRepository = new Mock<IDefaultRepository>();
            var defaultLogic = new DefaultLogic(
                logger,
                mockDefaultDbManager.Object,
                mockDefaultPersonDbRepository.Object,
                mockDefaultRepository.Object);
            await defaultLogic.RunDbRepositoryQuery();
            mockDefaultPersonDbRepository.Verify(v => v.Query(
                It.IsAny<Func<IQueryable<Person>, IQueryable<Person>>>(),
                It.IsAny<Func<IQueryable<Person>, IOrderedQueryable<Person>>>())
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
            mockDefaultDbManager
                .Setup(s => s.BeginTransactionAsync(It.IsAny<IsolationLevel>()));
            mockDefaultDbManager
                .Setup(s => s.CommitAsync());
            mockDefaultDbManager
                .Setup(s => s.RollbackAsync());
            var mockDefaultPersonDbRepository = new Mock<IDefaultPersonDbRepository>();
            mockDefaultPersonDbRepository.Setup(s => s.Create(It.IsAny<Person>())).ReturnsAsync(1);
            var mockDefaultRepository = new Mock<IDefaultRepository>();
            var defaultLogic = new DefaultLogic(
                logger,
                mockDefaultDbManager.Object,
                mockDefaultPersonDbRepository.Object,
                mockDefaultRepository.Object);
            await defaultLogic.RunDbRepositoryCreate();
            mockDefaultDbManager.Verify(v => v.BeginTransactionAsync(It.IsAny<IsolationLevel>()), Times.Once);
            mockDefaultPersonDbRepository.Verify(v => v.Create(It.IsAny<Person>()), Times.Once);
            mockDefaultDbManager.Verify(v => v.CommitAsync(), Times.Once);
            mockDefaultDbManager.Verify(v => v.RollbackAsync(), Times.Never);
        }
        [TestMethod]
        public async Task RunDbRepositoryModify()
        {
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultLogic>();
            var mockDefaultDbManager = new Mock<IDbManager<DefaultDbContext>>();
            mockDefaultDbManager
                .Setup(s => s.BeginTransactionAsync(It.IsAny<IsolationLevel>()));
            mockDefaultDbManager
                .Setup(s => s.CommitAsync());
            mockDefaultDbManager
                .Setup(s => s.RollbackAsync());
            var mockDefaultPersonDbRepository = new Mock<IDefaultPersonDbRepository>();
            mockDefaultPersonDbRepository
                .Setup(s => s.Query(
                    It.IsAny<Func<IQueryable<Person>, IQueryable<Person>>>(),
                    It.IsAny<Func<IQueryable<Person>, IOrderedQueryable<Person>>>()))
                .ReturnsAsync(new List<Person>() { new Person() });
                //.ReturnsAsync(new List<Person>());
            mockDefaultPersonDbRepository.Setup(s => s.Modify(It.IsAny<Person>())).ReturnsAsync(1);
            var mockDefaultRepository = new Mock<IDefaultRepository>();
            var defaultLogic = new DefaultLogic(
                logger,
                mockDefaultDbManager.Object,
                mockDefaultPersonDbRepository.Object,
                mockDefaultRepository.Object);
            await defaultLogic.RunDbRepositoryModify();
            mockDefaultPersonDbRepository.Verify(v => v.Query(
                It.IsAny<Func<IQueryable<Person>, IQueryable<Person>>>(),
                It.IsAny<Func<IQueryable<Person>, IOrderedQueryable<Person>>>())
            , Times.Once);
            mockDefaultDbManager.Verify(v => v.BeginTransactionAsync(It.IsAny<IsolationLevel>()), Times.Once);
            mockDefaultPersonDbRepository.Verify(v => v.Modify(It.IsAny<Person>()), Times.Once);
            //mockDefaultPersonDbRepository.Verify(v => v.Modify(It.IsAny<Person>()), Times.Never);
            mockDefaultDbManager.Verify(v => v.CommitAsync(), Times.Once);
            mockDefaultDbManager.Verify(v => v.RollbackAsync(), Times.Never);
        }
        [TestMethod]
        public async Task RunDbRepositoryRemove()
        {
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultLogic>();
            var mockDefaultDbManager = new Mock<IDbManager<DefaultDbContext>>();
            mockDefaultDbManager
                .Setup(s => s.BeginTransactionAsync(It.IsAny<IsolationLevel>()));
            mockDefaultDbManager
                .Setup(s => s.CommitAsync());
            mockDefaultDbManager
                .Setup(s => s.RollbackAsync());
            var mockDefaultPersonDbRepository = new Mock<IDefaultPersonDbRepository>();
            mockDefaultPersonDbRepository
                .Setup(s => s.Query(
                    It.IsAny<Func<IQueryable<Person>, IQueryable<Person>>>(),
                    It.IsAny<Func<IQueryable<Person>, IOrderedQueryable<Person>>>()))
                //.ReturnsAsync(new List<Person>() { new Customer() });
                .ReturnsAsync(new List<Person>());
            mockDefaultPersonDbRepository.Setup(s => s.Remove(It.IsAny<Person>())).ReturnsAsync(1);
            var mockDefaultRepository = new Mock<IDefaultRepository>();
            var defaultLogic = new DefaultLogic(
                logger,
                mockDefaultDbManager.Object,
                mockDefaultPersonDbRepository.Object,
                mockDefaultRepository.Object);
            await defaultLogic.RunDbRepositoryRemove();
            mockDefaultPersonDbRepository.Verify(v => v.Query(
                It.IsAny<Func<IQueryable<Person>, IQueryable<Person>>>(),
                It.IsAny<Func<IQueryable<Person>, IOrderedQueryable<Person>>>())
            , Times.Once);
            mockDefaultDbManager.Verify(v => v.BeginTransactionAsync(It.IsAny<IsolationLevel>()), Times.Never);
            //mockDefaultPersonDbRepository.Verify(v => v.Remove(It.IsAny<Person>()), Times.Once);
            mockDefaultPersonDbRepository.Verify(v => v.Remove(It.IsAny<Person>()), Times.Never);
            mockDefaultDbManager.Verify(v => v.CommitAsync(), Times.Never);
            mockDefaultDbManager.Verify(v => v.RollbackAsync(), Times.Never);
        }
        [TestMethod]
        public async Task RunDbRepositoryPagedQuery()
        {
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultLogic>();
            var mockDefaultDbManager = new Mock<IDbManager<DefaultDbContext>>();
            var mockDefaultPersonDbRepository = new Mock<IDefaultPersonDbRepository>();
            mockDefaultPersonDbRepository
                .Setup(s => s.PagedQuery(
                    It.IsAny<CommonPageInputDto>(),
                    It.IsAny<Func<IQueryable<Person>, IQueryable<Person>>>(),
                    It.IsAny<Func<IQueryable<Person>, IOrderedQueryable<Person>>>()))
                .ReturnsAsync(new CommonPagedQueryOutputDto<Person>());
            mockDefaultPersonDbRepository.Setup(s => s.Remove(It.IsAny<Person>()));
            var mockDefaultRepository = new Mock<IDefaultRepository>();
            var defaultLogic = new DefaultLogic(
                logger,
                mockDefaultDbManager.Object,
                mockDefaultPersonDbRepository.Object,
                mockDefaultRepository.Object);
            await defaultLogic.RunDbRepositoryPagedQuery();
            mockDefaultPersonDbRepository.Verify(v => v.PagedQuery(
                It.IsAny<CommonPageInputDto>(),
                It.IsAny<Func<IQueryable<Person>, IQueryable<Person>>>(),
                It.IsAny<Func<IQueryable<Person>, IOrderedQueryable<Person>>>())
            , Times.Once);
        }
    }
}