using Dapper;
using DemoNetCoreProject.BusinessLayer.Logics.Default;
using DemoNetCoreProject.DataLayer.Entities;
using DemoNetCoreProject.DataLayer.IServices;
using DemoNetCoreProject.DataLayer.Repositories;
using DemoNetCoreProject.DataLayer.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data;

namespace DemoNetCoreProject.UnitTest.BusinessLayer.Logics.Default
{
    [TestClass]
    public class UnitTest_DefaultLogic : UnitTestInitialize
    {
        [TestMethod]
        public async Task RunRepository()
        {
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultLogic>();
            //var mapper = new Mapper(new MapperConfiguration(c => DefaultAutoMapper.Load(c)));
            var defaultCustomerRepository = new DefaultCustomerRepository(_defaultDbContext);
            var mockDefaultDapperService = new Mock<IDapperService<DefaultDbContext>>();
            mockDefaultDapperService
                .Setup(s => s.Query<Customer>(It.IsAny<string>(), It.IsAny<DynamicParameters>(), It.IsAny<int?>(), It.IsAny<CommandType>()))
                .Callback(DapperGeneralCallback)
                .ReturnsAsync(new List<Customer>());
            var defaultLogic = new DefaultLogic(
                logger,
                _defaultDbContext,
                defaultCustomerRepository,
                mockDefaultDapperService.Object);
            await defaultLogic.RunRepository();
            //mockDefaultDapperService.Verify(s => s.Query<Customer>(It.IsAny<string>(), It.IsAny<DynamicParameters>(), It.IsAny<int?>(), It.IsAny<CommandType>()), 
            //    Times.Once);
            //Assert.IsNotNull(output);
            //Assert.IsTrue(output.Success);
        }
        [TestMethod]
        public async Task RunSqlStatement()
        {
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultLogic>();
            //var mapper = new Mapper(new MapperConfiguration(c => DefaultAutoMapper.Load(c)));
            var defaultCustomerRepository = new DefaultCustomerRepository(_defaultDbContext);
            var mockDefaultDapperService = new Mock<IDapperService<DefaultDbContext>>();
            // Mock
            var defaultLogic = new DefaultLogic(
                logger,
                _defaultDbContext,
                defaultCustomerRepository,
                mockDefaultDapperService.Object);
            await defaultLogic.RunSqlStatement();
            //Assert.IsNotNull(output);
            //Assert.IsTrue(output.Success);
        }
        [TestMethod]
        public async Task RunSqlCondition()
        {
            var service = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();
            var logger = service.GetRequiredService<ILoggerFactory>().CreateLogger<DefaultLogic>();
            //var mapper = new Mapper(new MapperConfiguration(c => DefaultAutoMapper.Load(c)));
            var defaultCustomerRepository = new DefaultCustomerRepository(_defaultDbContext);
            var mockDefaultDapperService = new Mock<IDapperService<DefaultDbContext>>();
            mockDefaultDapperService
                .Setup(s => s.Query<Customer>(It.IsAny<string>(), It.IsAny<DynamicParameters>(), It.IsAny<int?>(), It.IsAny<CommandType>()))
                .Callback(DapperGeneralCallback)
                .ReturnsAsync(new List<Customer>());
            var defaultLogic = new DefaultLogic(
                logger,
                _defaultDbContext,
                defaultCustomerRepository,
                mockDefaultDapperService.Object);
            await defaultLogic.RunSqlCondition();
            mockDefaultDapperService.Verify(s => s.Query<Customer>(It.IsAny<string>(), It.IsAny<DynamicParameters>(), It.IsAny<int?>(), It.IsAny<CommandType>()),
                Times.Once);
            //Assert.IsNotNull(output);
            //Assert.IsTrue(output.Success);
        }
    }
}