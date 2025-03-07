using Dapper;
using DemoNetCoreProject.DataLayer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Data;

namespace DemoNetCoreProject.UnitTest
{
    [TestClass]
    public class Test_Initialize
    {
        protected readonly ServiceProvider _serviceProvider;
        protected readonly ILoggerFactory _loggerFactory;
        protected readonly DefaultDbContext _defaultDbContext;
        public Test_Initialize()
        {
            _serviceProvider = new ServiceCollection()
                .AddLogging(builder => builder.AddConsole())
                .BuildServiceProvider();
            _loggerFactory = _serviceProvider.GetService<ILoggerFactory>()!;
            var optionsBuilder = new DbContextOptionsBuilder<DefaultDbContext>();
            optionsBuilder.UseInMemoryDatabase(databaseName: "DefaultDbContext");
            _defaultDbContext = new DefaultDbContext(optionsBuilder.Options);
        }
        [TestMethod]
        public void TestRun()
        {
            // Arrange
            // do mock...
            // Act
            // do run...
            // Assert
            // do verify...
        }
        [TestCleanup]
        public void Cleanup()
        {
            _serviceProvider.Dispose();
        }
        protected Action<string, DynamicParameters, int?, CommandType> DapperGeneralCallback =
            (c1, c2, c3, c4) =>
        {
            Console.WriteLine(c1);
            Console.WriteLine(c2);
        };
        protected Action<string, string, DynamicParameters, int, int, int?, CommandType> DapperPagedQueryCallback =
            (c1, c2, c3, c4, c5, c6, c7) =>
        {
            Console.WriteLine(c1);
            Console.WriteLine(c2);
            Console.WriteLine(c3);
            Console.WriteLine(c4);
            Console.WriteLine(c5);
        };
    }
}