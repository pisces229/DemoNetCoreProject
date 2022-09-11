using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace DemoNetCoreProject.Batch
{
    public static class SerilogHostBuilderExtensions
    {
        public static IHostBuilder UseDemoBatchSerilog(this IHostBuilder builder)
        {
            //"outputTemplate": "{Properties} [{Timestamp:o}] [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}"
            builder.UseSerilog(new LoggerConfiguration()
                .Enrich.WithProperty("Application", "DemoNetCoreProject.Batch")
                .Enrich.WithProperty("ProcessId", Guid.NewGuid().ToString())
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .WriteTo.Console(
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    outputTemplate: "[{Timestamp:o}] [{Level:u3}] [{ProcessId}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    outputTemplate: "[{Timestamp:o}] [{Level:u3}] [{ProcessId}] [{SourceContext}] {Message:lj}{NewLine}{Exception}",
                    path: $"c:/Workspace/DemoNetCoreProject/batch/{CommandLineArguments.PROG_ID}-.txt",
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 1024 * 1024,
                    retainedFileCountLimit: null)
                .CreateLogger());
            return builder;
        }
    }
}
