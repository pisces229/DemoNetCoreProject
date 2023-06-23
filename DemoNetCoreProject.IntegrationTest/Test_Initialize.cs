using DemoNetCoreProject.BusinessLayer;
using DemoNetCoreProject.DataLayer;
using DemoNetCoreProject.DataLayer.IServices;
using DemoNetCoreProject.DataLayer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DemoNetCoreProject.IntegrationTest
{
    public class Test_Initialize
    {
        protected readonly ServiceProvider _serviceProvider;
        public Test_Initialize()
        {
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: false)
                .Build();
            services.AddSingleton<IConfiguration>(_ => configuration);
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.SetMinimumLevel(LogLevel.Trace);
                builder.AddConsole();
            });
            //services.AddOptions();

            // Make HTTP requests using IHttpClientFactory in ASP.NET Core
            services.AddHttpClient("Default", client =>
            {
                client.BaseAddress = new Uri("https://localhost:9110/api/default/");
            });

            #region DbContext
            services.AddDbContext<DefaultDbContext>(option =>
            {
                var connectionName = "Default";
                //option.UseInMemoryDatabase(databaseName: connectionName);
                option.UseSqlServer(configuration.GetConnectionString(connectionName),
                    sqlServerOption =>
                    {
                        sqlServerOption.MinBatchSize(10);
                        sqlServerOption.MaxBatchSize(1000);
                        sqlServerOption.CommandTimeout(configuration.GetValue<int>($"ConnectionTimeout:{connectionName}"));
                        //sqlServerOption.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
                option.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                option.EnableSensitiveDataLogging();
                option.EnableDetailedErrors();
            });
            #endregion

            #region Cache
            {
                #region Memory
                services.AddSingleton((Func<IServiceProvider, IMemoryCache>)(factory =>
                {
                    var cache = new Microsoft.Extensions.Caching.Memory.MemoryCache(new MemoryCacheOptions());
                    return cache;
                }));
                services.AddSingleton<ICache, DemoNetCoreProject.DataLayer.Services.MemoryCache>();
                #endregion

                #region Redis
                //services.AddSingleton<ICache, DemoNetCoreProject.DataLayer.Services.RedisCache>();
                #endregion

                #region Database
                //services.AddDistributedSqlServerCache(options =>
                //{
                //    options.ConnectionString = configuration.GetConnectionString("Redis");
                //    options.SchemaName = "dbo";
                //    options.TableName = "DataCache";
                //    options.ExpiredItemsDeletionInterval = TimeSpan.FromMinutes(5);
                //});
                //services.AddSingleton<ICache, DemoNetCoreProject.DataLayer.Services.DatabaseCache>();
                #endregion
            }
            #endregion

            services.AddScoped<IUserService, TestUserService>();
            LoadBusinessLayerRegister.LoadServices(services);
            LoadDataLayerRegister.LoadServices(services);
            services.AddAutoMapper(configure =>
            {
                //configure.AllowNullDestinationValues = false;
                configure.AddProfiles(LoadBusinessLayerRegister.Profiles());
                configure.AddProfiles(LoadDataLayerRegister.Profiles());
            });
            _serviceProvider = services.BuildServiceProvider();
        }
        [TestMethod]
        public void TestRun()
        {
            // Arrange
            // Act
            // Assert
        }
        [TestCleanup]
        public async Task Cleanup()
        {
            await _serviceProvider.DisposeAsync();
        }
    }
}
