using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace DemoNetCoreProject.Batch
{
    public static class SerilogLoggingBuilderExtensions
    {
        public static ILoggingBuilder UseDefaultSerilog(this ILoggingBuilder builder)
            => EnvironmentVariable.ASPNETCORE_ENVIRONMENT switch
            {
                "Development" => UseDevelopment(builder),
                "Production" => UseProduction(builder),
                _ => builder,
            };
        private static ILoggingBuilder UseDevelopment(this ILoggingBuilder builder) =>
            builder.AddSerilog(new LoggerConfiguration()
                .Enrich.WithProperty("Application", "DemoNetCoreProject.Batch")
                .Enrich.WithProperty("ProcessId", Guid.NewGuid().ToString())
                .MinimumLevel.Debug()
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:o}] [{Level:u3}] [{ProcessId}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(
                    outputTemplate: "[{Timestamp:o}] [{Level:u3}] [{ProcessId}] [{SourceContext}] {Message:lj}{NewLine}{Exception}",
                    path: $"c:/Workspace/DemoNetCoreProject/batch/{CommandLineArguments.PROG_ID}-.txt",
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 1024 * 1024,
                    retainedFileCountLimit: null)
                .CreateLogger());
        private static ILoggingBuilder UseProduction(this ILoggingBuilder builder) =>
            builder.AddSerilog(new LoggerConfiguration()
                .Enrich.WithProperty("Application", "DemoNetCoreProject.Batch")
                .Enrich.WithProperty("ProcessId", Guid.NewGuid().ToString())
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:o}] [{Level:u3}] [{ProcessId}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(
                    outputTemplate: "[{Timestamp:o}] [{Level:u3}] [{ProcessId}] [{SourceContext}] {Message:lj}{NewLine}{Exception}",
                    path: $"c:/Workspace/DemoNetCoreProject/batch/{CommandLineArguments.PROG_ID}-.txt",
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 1024 * 1024,
                    retainedFileCountLimit: null)
                .CreateLogger());
    }
}
