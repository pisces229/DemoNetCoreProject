using DemoNetCoreProject.BusinessLayer;
using DemoNetCoreProject.DataLayer;
using DemoNetCoreProject.Batch.Services;
using DemoNetCoreProject.Batch;
using DemoNetCoreProject.Batch.Runners;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DemoNetCoreProject.DataLayer.Services;
using DemoNetCoreProject.DataLayer.IServices;

Console.WriteLine(CommandLineArguments.ENVIRONMENT);
Console.WriteLine(CommandLineArguments.PROG_ID);

#region HostBuilder
var hostBuilder = new HostBuilder()
    .UseDemoBatchSerilog()
    //.UseConsoleLifetime()
    //.UseEnvironment(EnvironmentName.Development)
    //.UseContentRoot(Directory.GetCurrentDirectory())
    .UseEnvironment(CommandLineArguments.ENVIRONMENT)
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
        builder.SetMinimumLevel(LogLevel.Trace);
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
            option.UseInMemoryDatabase(databaseName: connectionName);
            //option.UseSqlServer(configuration.GetConnectionString(connectionName),
            //    sqlServerOption =>
            //    {
            //        sqlServerOption.MinBatchSize(10);
            //        sqlServerOption.MaxBatchSize(1000);
            //        //sqlServerOption.CommandTimeout(0);
            //    });
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
        services.AddScoped<IUserService, UserBachService>();
        LoadDataLayerRegister.LoadServices(services);
        LoadBusinessLayerRegister.LoadServices(services);
        services.AddAutoMapper(configure =>
        {
            //configure.AllowNullDestinationValues = false;
            LoadBusinessLayerRegister.LoadAutoMappers(configure);
        });
        switch (CommandLineArguments.PROG_ID)
        {
            case "Default":
                services.AddScoped<IRunner, DefaultRunner>();
                break;
        }
    });
#endregion

#region Host
var host = hostBuilder.Build();
try
{
    host.Services.GetRequiredService<IRunner>().Run().Wait();
}
finally
{
    host.Dispose();
}
#endregion