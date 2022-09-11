using Serilog;
using Serilog.Events;

namespace DemoNetCoreProject.Backend
{
    public static class SerilogHostBuilderExtensions
    {
        public static IHostBuilder UseDemoBackendSerilog(this IHostBuilder builder)
        {
            //"outputTemplate": "{Properties} [{Timestamp:o}] [{Level:u3}] [{RequestId}] [{RequestPath}] [{SourceContext}] {Message:lj}{NewLine}{Exception}"
            builder.UseSerilog(new LoggerConfiguration()
                .Enrich.WithProperty("Application", "DemoNetCoreProject.Backend")
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .WriteTo.Console(
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    outputTemplate: "[{Timestamp:o}] [{Level:u3}] [{RequestId}] [{RequestPath}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    outputTemplate: "[{Timestamp:o}] [{Level:u3}] [{RequestId}] [{RequestPath}] [{SourceContext}] {Message:lj}{NewLine}{Exception}",
                    path: "c:/Workspace/DemoNetCoreProject/backend/info-.txt",
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 1024 * 1024,
                    retainedFileCountLimit: null)
                .WriteTo.File(
                    restrictedToMinimumLevel: LogEventLevel.Warning,
                    outputTemplate: "[{Timestamp:o}] [{Level:u3}] [{RequestId}] [{RequestPath}] [{SourceContext}] {Message:lj}{NewLine}{Exception}",
                    path: "c:/Workspace/DemoNetCoreProject/backend/warn-.txt",
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 1024 * 1024,
                    retainedFileCountLimit: null)
                .CreateLogger());
            return builder;
        }
    }
}
