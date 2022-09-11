using DemoNetCoreProject.BusinessLayer;
using DemoNetCoreProject.DataLayer;
using DemoNetCoreProject.DataLayer.IServices;
using DemoNetCoreProject.DataLayer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DemoNetCoreProject.IntegrationTest
{
    public class UnitTestInitialize
    {
        protected IHost Host { private set; get; } = null!;
        [TestInitialize]
        public void Initialize()
        {
            Host = new HostBuilder()
                .ConfigureAppConfiguration((hostBuilder, configurationBuilder) =>
                {
                    var environment = hostBuilder.HostingEnvironment;
                    configurationBuilder
                        .SetBasePath(environment.ContentRootPath)
                        .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: false);
                })
                .ConfigureLogging((hostBuilder, builder) =>
                {
                    builder.ClearProviders();
                    builder.AddConsole();
                    builder.SetMinimumLevel(LogLevel.Debug);
                })
                .ConfigureServices((hostBuilder, services) =>
                {
                    var configuration = hostBuilder.Configuration;
                    // 註冊 Options Pattern 服務，將配置內容註冊到容器裡，來獲取對應的服務 Provider 對象
                    services.AddOptions();

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
                                //sqlServerOption.CommandTimeout(0);
                            });
                        option.EnableSensitiveDataLogging();
                        option.EnableDetailedErrors();
                    });
                    #endregion

                    #region Cache
                    {
                        #region Memory
                        services.AddSingleton<IMemoryCache>(factory =>
                        {
                            var cache = new MemoryCache(new MemoryCacheOptions());
                            return cache;
                        });
                        services.AddSingleton<ICacheService, MemoryCacheService>();
                        #endregion

                        #region Redis
                        //services.AddSingleton<ICacheService, RedisCacheService>();
                        #endregion

                        #region Database
                        //services.AddDistributedSqlServerCache(options =>
                        //{
                        //    options.ConnectionString = configuration.GetConnectionString("Redis");
                        //    options.SchemaName = "dbo";
                        //    options.TableName = "DataCache";
                        //    options.ExpiredItemsDeletionInterval = TimeSpan.FromMinutes(5);
                        //});
                        //services.AddSingleton<ICacheService, DatabaseCacheService>();
                        #endregion
                    }
                    #endregion

                    // Service
                    services.AddScoped<IUserService, UserService>();
                    LoadDataLayerRegister.LoadServices(services);
                    LoadBusinessLayerRegister.LoadServices(services);
                    services.AddAutoMapper(configure =>
                    {
                        //configure.AllowNullDestinationValues = false;
                        LoadBusinessLayerRegister.LoadAutoMappers(configure);
                    });
                })
            .Build();
        }
        [TestCleanup]
        public void Cleanup()
        {
            Host.Dispose();
        }
    }
}
