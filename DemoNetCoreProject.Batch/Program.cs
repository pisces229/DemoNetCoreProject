using DemoNetCoreProject.BusinessLayer;
using DemoNetCoreProject.DataLayer;
using DemoNetCoreProject.Batch.Services;
using DemoNetCoreProject.Batch;
using DemoNetCoreProject.Batch.Runners;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DemoNetCoreProject.DataLayer.Services;
using DemoNetCoreProject.DataLayer.IServices;

Console.WriteLine(EnvironmentVariable.ASPNETCORE_ENVIRONMENT);
Console.WriteLine(CommandLineArguments.PROG_ID);

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
    builder.UseDemoBackendSerilog();
});
services.AddOptions();

// Make HTTP requests using IHttpClientFactory in ASP.NET Core
services.AddHttpClient("Default", client =>
{
    client.BaseAddress = new Uri("https://localhost:9110/api/default/");
});

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
    //        sqlServerOption.CommandTimeout(0);
    //        sqlServerOption.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
    //    });
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
    //services.AddSingleton<ICache, RedisCache>();
    #endregion

    #region Database
    //services.AddDistributedSqlServerCache(options =>
    //{
    //    options.ConnectionString = configuration.GetConnectionString("Redis");
    //    options.SchemaName = "dbo";
    //    options.TableName = "DataCache";
    //    options.ExpiredItemsDeletionInterval = TimeSpan.FromMinutes(5);
    //});
    //services.AddSingleton<ICache, DatabaseCache>();
    #endregion
}
#endregion

services.AddScoped<IUserService, UserBachService>();
LoadBusinessLayerRegister.LoadServices(services);
LoadDataLayerRegister.LoadServices(services);
services.AddAutoMapper(configure =>
{
    //configure.AllowNullDestinationValues = false;
    LoadBusinessLayerRegister.LoadAutoMappers(configure);
    LoadDataLayerRegister.LoadAutoMappers(configure);
});

switch (CommandLineArguments.PROG_ID)
{
    case "Default":
        services.AddScoped<IRunner, DefaultRunner>();
        break;
}

using var serviceProvider = services.BuildServiceProvider();
serviceProvider.GetRequiredService<IRunner>().Run().Wait();
