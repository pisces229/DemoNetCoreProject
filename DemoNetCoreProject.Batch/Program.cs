using DemoNetCoreProject.Batch;
using DemoNetCoreProject.Batch.EnumRunners;
using DemoNetCoreProject.Batch.Runners;
using DemoNetCoreProject.Batch.Services;
using DemoNetCoreProject.BusinessLayer;
using DemoNetCoreProject.Common.Utilities;
using DemoNetCoreProject.DataLayer;
using DemoNetCoreProject.DataLayer.IServices;
using DemoNetCoreProject.DataLayer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

Console.WriteLine(EnvironmentVariable.ASPNETCORE_ENVIRONMENT);
Console.WriteLine(CommandLineArguments.PROG_ID);

var hostbuilder = Host.CreateDefaultBuilder(args);

hostbuilder.ConfigureAppConfiguration((hostContext, configurationBuilder) =>
{
    configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
    var appsettings = "appsettings.json";
    if (!EnvironmentVariable.IsDevelopment())
    {
        appsettings = $"appsettings.{EnvironmentVariable.ASPNETCORE_ENVIRONMENT}.json";
    }
    configurationBuilder.AddJsonFile(path: appsettings, optional: false, reloadOnChange: false);
    var configuration = configurationBuilder.Build();
    // Load Resource
    try
    {
        Directory.CreateDirectory(configuration.GetValue<string>("Path:Temp"));
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
});

hostbuilder.ConfigureLogging((hostContext, loggingBuilder) =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.SetMinimumLevel(LogLevel.Trace);
    loggingBuilder.UseDefaultSerilog();
});

hostbuilder.ConfigureServices((hostContext, services) =>
{
    services.AddHostedService<HostedService>();
    // Make HTTP requests using IHttpClientFactory in ASP.NET Core
    services.AddHttpClient("Default", client =>
    {
        client.BaseAddress = new Uri("https://localhost:9110/api/default/");
    });
    services.AddOptions();
    var secret = hostContext.Configuration.GetValue<string>("Secret");
    if (!string.IsNullOrEmpty(secret))
    {
        var decryptStrings = hostContext.Configuration.GetSection("DecryptStrings").Get<string[]>();
        foreach (var decryptString in decryptStrings!)
        {
            hostContext.Configuration[decryptString] = SecretUtility.Decrypt(
                hostContext.Configuration[decryptString]!, secret);
        }
    }
    #region DbContext
    services.AddDbContext<DefaultDbContext>(option =>
    {
        var connectionName = "Default";
        //option.UseInMemoryDatabase(databaseName: connectionName);
        option.UseSqlServer(hostContext.Configuration.GetConnectionString(connectionName),
            sqlServerOption =>
            {
                sqlServerOption.MinBatchSize(10);
                sqlServerOption.MaxBatchSize(1000);
                sqlServerOption.CommandTimeout(hostContext.Configuration.GetValue<int>($"ConnectionTimeout:{connectionName}"));
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
        //services.AddSingleton<ICache, RedisCache>();
        #endregion

        #region Database
        //services.AddDistributedSqlServerCache(options =>
        //{
        //    options.ConnectionString = configuration.GetConnectionString("Cache");
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
        configure.AddProfiles(LoadBusinessLayerRegister.Profiles());
        configure.AddProfiles(LoadDataLayerRegister.Profiles());
    });

    services.AddSingleton<ChannelRunner>();

    switch (CommandLineArguments.PROG_ID)
    {
        case "Default":
            services.AddScoped<IRunner, DefaultRunner>();
            break;
    }

    services.AddKeyedScoped<IEnumRunner, FirstEnumRunner>(EnumRunner.First);
    services.AddKeyedScoped<IEnumRunner, SecondEnumRunner>(EnumRunner.Second);

});

var app = hostbuilder.Build();

#region Lifetime
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
var logger = app.Services.GetRequiredService<ILogger<Program>>();

lifetime.ApplicationStarted.Register(() =>
{
    logger.LogInformation("Started:[{time}]", DateTimeOffset.Now);
});
lifetime.ApplicationStopping.Register(() =>
{
    logger.LogInformation("Stopping:[{time}]", DateTimeOffset.Now);
});
lifetime.ApplicationStopped.Register(() =>
{
    logger.LogInformation("Stopped:[{time}]", DateTimeOffset.Now);
});
#endregion

logger.LogInformation("app Start");
app.Run();
logger.LogInformation("app End");
