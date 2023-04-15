using Dapper;
using DemoNetCoreProject.Common.Dtos;
using DemoNetCoreProject.DataLayer.Entities;
using DemoNetCoreProject.DataLayer.IServices;
using DemoNetCoreProject.DataLayer.Repositories.Default;
using DemoNetCoreProject.DataLayer.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data;
using System.Data.Common;

namespace DemoNetCoreProject.UnitTest.Domain.Utilities
{
    [TestClass]
    public class Test_DefaultRepository : Test_Initialize
    {
        public Test_DefaultRepository() : base()
        {
        }
        [TestInitialize]
        public void Initialize()
        {
            Console.WriteLine("UnitTest_DefaultRepository.Initialize");
        }
        [TestMethod]
        public async Task RunDapperQuery()
        {
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultRepository>();
            var mockDefaultDapperService = new Mock<IDapperService<DefaultDbContext>>();
            mockDefaultDapperService
                .Setup(s => s.Query<Person>(
                    It.IsAny<string>(),
                    It.IsAny<DynamicParameters>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType>()))
                .Callback(DapperGeneralCallback);
            var defaultRepository = new DefaultRepository(
                logger,
                _defaultDbContext,
                mockDefaultDapperService.Object);
            await defaultRepository.RunDapperQuery();
            mockDefaultDapperService.Verify(s => s.Query<Person>(
                It.IsAny<string>(),
                It.IsAny<DynamicParameters>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType>())
            , Times.Once);
        }
        [TestMethod]
        public async Task RunDapperExecuteScalar()
        {
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultRepository>();
            var mockDefaultDapperService = new Mock<IDapperService<DefaultDbContext>>();
            mockDefaultDapperService
                .Setup(s => s.ExecuteScalar<int>(
                    It.IsAny<string>(),
                    It.IsAny<DynamicParameters>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType>()))
                .Callback(DapperGeneralCallback)
                .ReturnsAsync(1);
            var defaultRepository = new DefaultRepository(
                logger,
                _defaultDbContext,
                mockDefaultDapperService.Object);
            await defaultRepository.RunDapperExecuteScalar();
            mockDefaultDapperService.Verify(s => s.ExecuteScalar<int>(
                It.IsAny<string>(),
                It.IsAny<DynamicParameters>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType>())
            , Times.Once);
        }
        [Ignore]
        [TestMethod]
        public async Task RunDapperQueryMultiple()
        {
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var mockGridReader = new Mock<SqlMapper.GridReader>();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultRepository>();
            var mockDefaultDapperService = new Mock<IDapperService<DefaultDbContext>>();
            mockDefaultDapperService
                .Setup(s => s.QueryMultiple(
                    It.IsAny<string>(),
                    It.IsAny<DynamicParameters>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType>()))
                .Callback(DapperGeneralCallback);
            var defaultRepository = new DefaultRepository(
                logger,
                _defaultDbContext,
                mockDefaultDapperService.Object);
            await defaultRepository.RunDapperQueryMultiple();
            mockDefaultDapperService.Verify(s => s.QueryMultiple(
                It.IsAny<string>(),
                It.IsAny<DynamicParameters>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType>())
            , Times.Once);
        }
        [Ignore]
        [TestMethod]
        public async Task RunDapperExecuteReader()
        {
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var mockDataReader = new Mock<DbDataReader>();
            mockDataReader.Setup(s => s.HasRows).Returns(true);
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultRepository>();
            var mockDefaultDapperService = new Mock<IDapperService<DefaultDbContext>>();
            mockDefaultDapperService
                .Setup(s => s.ExecuteReader(
                    It.IsAny<string>(),
                    It.IsAny<DynamicParameters>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType>()))
                .Callback(DapperGeneralCallback)
                .ReturnsAsync(mockDataReader.Object);
            var defaultRepository = new DefaultRepository(
                logger,
                _defaultDbContext,
                mockDefaultDapperService.Object);
            await defaultRepository.RunDapperExecuteReader();
            mockDefaultDapperService.Verify(s => s.ExecuteReader(
                It.IsAny<string>(),
                It.IsAny<DynamicParameters>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType>())
            , Times.Once);
        }
        [TestMethod]
        public async Task RunDapperPagedQuery()
        {
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultRepository>();
            var mockDefaultDapperService = new Mock<IDapperService<DefaultDbContext>>();
            mockDefaultDapperService
                .Setup(s => s.PagedQuery<Person>(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DynamicParameters>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType>()))
                .Callback(DapperPagedQueryCallback);
            var defaultRepository = new DefaultRepository(
                logger,
                _defaultDbContext,
                mockDefaultDapperService.Object);
            await defaultRepository.RunDapperPagedQuery();
            mockDefaultDapperService.Verify(s => s.PagedQuery<Person>(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DynamicParameters>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType>())
            , Times.Once);
        }
        [TestMethod]
        public async Task RunSqlCondition()
        {
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultRepository>();
            var mockDefaultDapperService = new Mock<IDapperService<DefaultDbContext>>();
            mockDefaultDapperService
                .Setup(s => s.Query<Person>(
                    It.IsAny<string>(),
                    It.IsAny<DynamicParameters>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType>()))
                .Callback(DapperGeneralCallback)
                .ReturnsAsync(new List<Person>());
            var defaultRepository = new DefaultRepository(
                logger,
                _defaultDbContext,
                mockDefaultDapperService.Object);
            await defaultRepository.RunSqlCondition();
            mockDefaultDapperService.Verify(s => s.Query<Person>(
                It.IsAny<string>(),
                It.IsAny<DynamicParameters>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType>()),
                Times.Never);
        }
    }
}