using Dapper;
using DemoNetCoreProject.BusinessLayer.Logics.Default;
using DemoNetCoreProject.Common.Dtos;
using DemoNetCoreProject.DataLayer.Entities;
using DemoNetCoreProject.DataLayer.IRepositories;
using DemoNetCoreProject.DataLayer.IServices;
using DemoNetCoreProject.DataLayer.Repositories;
using DemoNetCoreProject.DataLayer.Services;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data;
using System.Data.Common;
using static Dapper.SqlMapper;

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
        public async Task RunDbRepositoryQuery()
        {
            // Arrange
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultLogic>();
            //var mapper = new Mapper(new MapperConfiguration(c => DefaultAutoMapper.Load(c)));
            var mockDefaultDbManager = new Mock<IDbManager<DefaultDbContext>>();
            var mockDefaultCustomerRepository = new Mock<IDefaultCustomerDbRepository>();
            mockDefaultCustomerRepository
                .Setup(s => s.Query(
                    It.IsAny<bool>(),
                    It.IsAny<Func<IQueryable<Customer>, IQueryable<Customer>>>(),
                    It.IsAny<Func<IQueryable<Customer>, IOrderedQueryable<Customer>>>()))
                .ReturnsAsync(new List<Customer>() { new Customer() });
            var mockDefaultDapperService = new Mock<IDapperService<DefaultDbContext>>();
            var defaultLogic = new DefaultLogic(
                logger,
                mockDefaultDbManager.Object,
                mockDefaultCustomerRepository.Object,
                mockDefaultDapperService.Object);
            // Act
            await defaultLogic.RunDbRepositoryQuery();
            // Assert
            mockDefaultCustomerRepository.Verify(v => v.Query(
                It.IsAny<bool>(),
                It.IsAny<Func<IQueryable<Customer>, IQueryable<Customer>>>(),
                It.IsAny<Func<IQueryable<Customer>, IOrderedQueryable<Customer>>>())
            , Times.Once);
            //Assert.IsNotNull(output);
            //Assert.IsTrue(output.Success);
        }
        [TestMethod]
        public async Task RunDbRepositoryCreate()
        {
            // Arrange
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultLogic>();
            //var mapper = new Mapper(new MapperConfiguration(c => DefaultAutoMapper.Load(c)));
            var mockDefaultDbManager = new Mock<IDbManager<DefaultDbContext>>();
            mockDefaultDbManager.Setup(s => s.SaveChangesAsync()).ReturnsAsync(1);
            var mockDefaultCustomerRepository = new Mock<IDefaultCustomerDbRepository>();
            mockDefaultCustomerRepository.Setup(s => s.Create(It.IsAny<Customer>()));
            var mockDefaultDapperService = new Mock<IDapperService<DefaultDbContext>>();
            var defaultLogic = new DefaultLogic(
                logger,
                mockDefaultDbManager.Object,
                mockDefaultCustomerRepository.Object,
                mockDefaultDapperService.Object);
            // Act
            await defaultLogic.RunDbRepositoryCreate();
            // Assert
            mockDefaultCustomerRepository.Verify(v => v.Create(It.IsAny<Customer>()), Times.Once);
            mockDefaultDbManager.Verify(s => s.SaveChangesAsync(), Times.Once);
            //Assert.IsNotNull(output);
            //Assert.IsTrue(output.Success);
        }
        [TestMethod]
        public async Task RunDbRepositoryModify()
        {
            // Arrange
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultLogic>();
            //var mapper = new Mapper(new MapperConfiguration(c => DefaultAutoMapper.Load(c)));
            var mockDefaultDbManager = new Mock<IDbManager<DefaultDbContext>>();
            mockDefaultDbManager.Setup(s => s.SaveChangesAsync()).ReturnsAsync(1);
            var mockDefaultCustomerRepository = new Mock<IDefaultCustomerDbRepository>();
            mockDefaultCustomerRepository
                .Setup(s => s.Query(
                    It.IsAny<bool>(),
                    It.IsAny<Func<IQueryable<Customer>, IQueryable<Customer>>>(),
                    It.IsAny<Func<IQueryable<Customer>, IOrderedQueryable<Customer>>>()))
                .ReturnsAsync(new List<Customer>() { new Customer() });
                //.ReturnsAsync(new List<Customer>());
            mockDefaultCustomerRepository.Setup(s => s.Modify(It.IsAny<Customer>()));
            var mockDefaultDapperService = new Mock<IDapperService<DefaultDbContext>>();
            var defaultLogic = new DefaultLogic(
                logger,
                mockDefaultDbManager.Object,
                mockDefaultCustomerRepository.Object,
                mockDefaultDapperService.Object);
            // Act
            await defaultLogic.RunDbRepositoryModify();
            // Assert
            mockDefaultCustomerRepository.Verify(v => v.Query(
                It.IsAny<bool>(),
                It.IsAny<Func<IQueryable<Customer>, IQueryable<Customer>>>(),
                It.IsAny<Func<IQueryable<Customer>, IOrderedQueryable<Customer>>>())
            , Times.Once);
            mockDefaultCustomerRepository.Verify(v => v.Modify(It.IsAny<Customer>()), Times.Once);
            //mockDefaultCustomerRepository.Verify(v => v.Modify(It.IsAny<Customer>()), Times.Never);
            mockDefaultDbManager.Verify(s => s.SaveChangesAsync(), Times.Once);
            //mockDefaultDbManager.Verify(s => s.SaveChangesAsync(), Times.Never);
            //Assert.IsNotNull(output);
            //Assert.IsTrue(output.Success);
        }
        [TestMethod]
        public async Task RunDbRepositoryRemove()
        {
            // Arrange
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultLogic>();
            //var mapper = new Mapper(new MapperConfiguration(c => DefaultAutoMapper.Load(c)));
            var mockDefaultDbManager = new Mock<IDbManager<DefaultDbContext>>();
            mockDefaultDbManager.Setup(s => s.SaveChangesAsync()).ReturnsAsync(1);
            var mockDefaultCustomerRepository = new Mock<IDefaultCustomerDbRepository>();
            mockDefaultCustomerRepository
                .Setup(s => s.Query(
                    It.IsAny<bool>(),
                    It.IsAny<Func<IQueryable<Customer>, IQueryable<Customer>>>(),
                    It.IsAny<Func<IQueryable<Customer>, IOrderedQueryable<Customer>>>()))
                //.ReturnsAsync(new List<Customer>() { new Customer() });
                .ReturnsAsync(new List<Customer>());
            mockDefaultCustomerRepository.Setup(s => s.Remove(It.IsAny<Customer>()));
            var mockDefaultDapperService = new Mock<IDapperService<DefaultDbContext>>();
            var defaultLogic = new DefaultLogic(
                logger,
                mockDefaultDbManager.Object,
                mockDefaultCustomerRepository.Object,
                mockDefaultDapperService.Object);
            // Act
            await defaultLogic.RunDbRepositoryRemove();
            // Assert
            mockDefaultCustomerRepository.Verify(v => v.Query(
                It.IsAny<bool>(),
                It.IsAny<Func<IQueryable<Customer>, IQueryable<Customer>>>(),
                It.IsAny<Func<IQueryable<Customer>, IOrderedQueryable<Customer>>>())
            , Times.Once);
            //mockDefaultCustomerRepository.Verify(v => v.Remove(It.IsAny<Customer>()), Times.Once);
            mockDefaultCustomerRepository.Verify(v => v.Remove(It.IsAny<Customer>()), Times.Never);
            //mockDefaultDbManager.Verify(s => s.SaveChangesAsync(), Times.Once);
            mockDefaultDbManager.Verify(s => s.SaveChangesAsync(), Times.Never);
            //Assert.IsNotNull(output);
            //Assert.IsTrue(output.Success);
        }
        [TestMethod]
        public async Task RunDbRepositoryPagedQuery()
        {
            // Arrange
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultLogic>();
            //var mapper = new Mapper(new MapperConfiguration(c => DefaultAutoMapper.Load(c)));
            var mockDefaultDbManager = new Mock<IDbManager<DefaultDbContext>>();
            mockDefaultDbManager.Setup(s => s.SaveChangesAsync()).ReturnsAsync(1);
            var mockDefaultCustomerRepository = new Mock<IDefaultCustomerDbRepository>();
            mockDefaultCustomerRepository
                .Setup(s => s.PagedQuery(
                    It.IsAny<CommonPageDto>(),
                    It.IsAny<bool>(),
                    It.IsAny<Func<IQueryable<Customer>, IQueryable<Customer>>>(),
                    It.IsAny<Func<IQueryable<Customer>, IOrderedQueryable<Customer>>>()))
                .ReturnsAsync(new CommonPagedResultDto<Customer>());
            mockDefaultCustomerRepository.Setup(s => s.Remove(It.IsAny<Customer>()));
            var mockDefaultDapperService = new Mock<IDapperService<DefaultDbContext>>();
            var defaultLogic = new DefaultLogic(
                logger,
                mockDefaultDbManager.Object,
                mockDefaultCustomerRepository.Object,
                mockDefaultDapperService.Object);
            // Act
            await defaultLogic.RunDbRepositoryPagedQuery();
            // Assert
            mockDefaultCustomerRepository.Verify(v => v.PagedQuery(
                It.IsAny<CommonPageDto>(),
                It.IsAny<bool>(),
                It.IsAny<Func<IQueryable<Customer>, IQueryable<Customer>>>(),
                It.IsAny<Func<IQueryable<Customer>, IOrderedQueryable<Customer>>>())
            , Times.Once);
            //Assert.IsNotNull(output);
            //Assert.IsTrue(output.Success);
        }
        [TestMethod]
        public async Task RunDapperQuery()
        {
            // Arrange
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultLogic>();
            //var mapper = new Mapper(new MapperConfiguration(c => DefaultAutoMapper.Load(c)));
            var mockDefaultDbManager = new Mock<IDbManager<DefaultDbContext>>();
            var moclDefaultCustomerRepository = new Mock<IDefaultCustomerDbRepository>();
            var mockDefaultDapperService = new Mock<IDapperService<DefaultDbContext>>();
            mockDefaultDapperService
                .Setup(s => s.Query<Customer>(
                    It.IsAny<string>(), 
                    It.IsAny<DynamicParameters>(), 
                    It.IsAny<int?>(), 
                    It.IsAny<CommandType>()))
                .Callback(DapperGeneralCallback);
            var defaultLogic = new DefaultLogic(
                logger,
                mockDefaultDbManager.Object,
                moclDefaultCustomerRepository.Object,
                mockDefaultDapperService.Object);
            // Act
            await defaultLogic.RunDapperQuery();
            // Assert
            mockDefaultDapperService.Verify(s => s.Query<Customer>(
                It.IsAny<string>(), 
                It.IsAny<DynamicParameters>(),
                It.IsAny<int?>(), 
                It.IsAny<CommandType>())
            ,Times.Once);
            //Assert.IsNotNull(output);
            //Assert.IsTrue(output.Success);
        }
        [TestMethod]
        public async Task RunDapperExecuteScalar()
        {
            // Arrange
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultLogic>();
            //var mapper = new Mapper(new MapperConfiguration(c => DefaultAutoMapper.Load(c)));
            var mockDefaultDbManager = new Mock<IDbManager<DefaultDbContext>>();
            var moclDefaultCustomerRepository = new Mock<IDefaultCustomerDbRepository>();
            var mockDefaultDapperService = new Mock<IDapperService<DefaultDbContext>>();
            mockDefaultDapperService
                .Setup(s => s.ExecuteScalar<int>(
                    It.IsAny<string>(),
                    It.IsAny<DynamicParameters>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType>()))
                .Callback(DapperGeneralCallback)
                .ReturnsAsync(1);
            var defaultLogic = new DefaultLogic(
                logger,
                mockDefaultDbManager.Object,
                moclDefaultCustomerRepository.Object,
                mockDefaultDapperService.Object);
            // Act
            await defaultLogic.RunDapperExecuteScalar();
            // Assert
            mockDefaultDapperService.Verify(s => s.ExecuteScalar<int>(
                It.IsAny<string>(),
                It.IsAny<DynamicParameters>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType>())
            , Times.Once);
            //Assert.IsNotNull(output);
            //Assert.IsTrue(output.Success);
        }
        [TestMethod]
        public async Task RunDapperQueryMultiple()
        {
            // Arrange
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultLogic>();
            var mockGridReader = new Mock<SqlMapper.GridReader>();
            //var mapper = new Mapper(new MapperConfiguration(c => DefaultAutoMapper.Load(c)));
            var mockDefaultDbManager = new Mock<IDbManager<DefaultDbContext>>();
            var moclDefaultCustomerRepository = new Mock<IDefaultCustomerDbRepository>();
            var mockDefaultDapperService = new Mock<IDapperService<DefaultDbContext>>();
            mockDefaultDapperService
                .Setup(s => s.QueryMultiple(
                    It.IsAny<string>(),
                    It.IsAny<DynamicParameters>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType>()))
                .Callback(DapperGeneralCallback);
            var defaultLogic = new DefaultLogic(
                logger,
                mockDefaultDbManager.Object,
                moclDefaultCustomerRepository.Object,
                mockDefaultDapperService.Object);
            // Act
            await defaultLogic.RunDapperQueryMultiple();
            // Assert
            mockDefaultDapperService.Verify(s => s.QueryMultiple(
                It.IsAny<string>(),
                It.IsAny<DynamicParameters>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType>())
            , Times.Once);
            //Assert.IsNotNull(output);
            //Assert.IsTrue(output.Success);
        }
        [TestMethod]
        public async Task RunDapperExecuteReader()
        {
            // Arrange
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultLogic>();
            var mockDataReader = new Mock<DbDataReader>();
            mockDataReader.Setup(s => s.HasRows).Returns(true);
            //mockDataReader.Setup(s => s.ReadAsync()).ReturnsAsync(true);
            //var mapper = new Mapper(new MapperConfiguration(c => DefaultAutoMapper.Load(c)));
            var mockDefaultDbManager = new Mock<IDbManager<DefaultDbContext>>();
            var moclDefaultCustomerRepository = new Mock<IDefaultCustomerDbRepository>();
            var mockDefaultDapperService = new Mock<IDapperService<DefaultDbContext>>();
            mockDefaultDapperService
                .Setup(s => s.ExecuteReader(
                    It.IsAny<string>(),
                    It.IsAny<DynamicParameters>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType>()))
                .Callback(DapperGeneralCallback)
                .ReturnsAsync(mockDataReader.Object);
            var defaultLogic = new DefaultLogic(
                logger,
                mockDefaultDbManager.Object,
                moclDefaultCustomerRepository.Object,
                mockDefaultDapperService.Object);
            // Act
            await defaultLogic.RunDapperExecuteReader();
            // Assert
            mockDefaultDapperService.Verify(s => s.ExecuteReader(
                It.IsAny<string>(),
                It.IsAny<DynamicParameters>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType>())
            , Times.Once);
            //Assert.IsNotNull(output);
            //Assert.IsTrue(output.Success);
        }
        [TestMethod]
        public async Task RunDapperPagedQuery()
        {
            // Arrange
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultLogic>();
            //var mapper = new Mapper(new MapperConfiguration(c => DefaultAutoMapper.Load(c)));
            var mockDefaultDbManager = new Mock<IDbManager<DefaultDbContext>>();
            var moclDefaultCustomerRepository = new Mock<IDefaultCustomerDbRepository>();
            var mockDefaultDapperService = new Mock<IDapperService<DefaultDbContext>>();
            mockDefaultDapperService
                .Setup(s => s.PagedQuery<Customer>(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DynamicParameters>(),
                    It.IsAny<CommonPageDto>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType>()))
                .Callback(DapperPagedQueryCallback);
            var defaultLogic = new DefaultLogic(
                logger,
                mockDefaultDbManager.Object,
                moclDefaultCustomerRepository.Object,
                mockDefaultDapperService.Object);
            // Act
            await defaultLogic.RunDapperPagedQuery();
            // Assert
            mockDefaultDapperService.Verify(s => s.PagedQuery<Customer>(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DynamicParameters>(),
                It.IsAny<CommonPageDto>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType>())
            , Times.Once);
            //Assert.IsNotNull(output);
            //Assert.IsTrue(output.Success);
        }
        [TestMethod]
        public async Task RunSqlCondition()
        {
            // Arrange
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultLogic>();
            //var mapper = new Mapper(new MapperConfiguration(c => DefaultAutoMapper.Load(c)));
            var mockDefaultDbManager = new Mock<IDbManager<DefaultDbContext>>();
            var moclDefaultCustomerRepository = new Mock<IDefaultCustomerDbRepository>();
            var mockDefaultDapperService = new Mock<IDapperService<DefaultDbContext>>();
            mockDefaultDapperService
                .Setup(s => s.Query<Customer>(
                    It.IsAny<string>(), 
                    It.IsAny<DynamicParameters>(),
                    It.IsAny<int?>(), 
                    It.IsAny<CommandType>()))
                .Callback(DapperGeneralCallback)
                .ReturnsAsync(new List<Customer>());
            var defaultLogic = new DefaultLogic(
                logger,
                mockDefaultDbManager.Object,
                moclDefaultCustomerRepository.Object,
                mockDefaultDapperService.Object);
            // Act
            await defaultLogic.RunSqlCondition();
            // Assert
            mockDefaultDapperService.Verify(s => s.Query<Customer>(
                It.IsAny<string>(), 
                It.IsAny<DynamicParameters>(), 
                It.IsAny<int?>(), 
                It.IsAny<CommandType>()),
                Times.Never);
            //Assert.IsNotNull(output);
            //Assert.IsTrue(output.Success);
        }
    }
}