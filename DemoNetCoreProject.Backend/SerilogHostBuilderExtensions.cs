using Serilog;
using Serilog.Events;

namespace DemoNetCoreProject.Backend
{
    public static class SerilogHostBuilderExtensions
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
                .Enrich.WithProperty("Application", "DemoNetCoreProject.Backend")
                .MinimumLevel.Information()
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:o}] [{Level:u3}] [{RequestId}] [{RequestPath}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(
                    outputTemplate: "[{Timestamp:o}] [{Level:u3}] [{RequestId}] [{RequestPath}] [{SourceContext}] {Message:lj}{NewLine}{Exception}",
                    path: "c:/workspace/DemoNetCoreProject/backend/log-.txt",
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 1024 * 1024,
                    retainedFileCountLimit: null)
                .CreateLogger());
        private static ILoggingBuilder UseProduction(this ILoggingBuilder builder) =>
            builder.AddSerilog(new LoggerConfiguration()
                .Enrich.WithProperty("Application", "DemoNetCoreProject.Backend")
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:o}] [{Level:u3}] [{RequestId}] [{RequestPath}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(
                    outputTemplate: "[{Timestamp:o}] [{Level:u3}] [{RequestId}] [{RequestPath}] [{SourceContext}] {Message:lj}{NewLine}{Exception}",
                    path: "c:/workspace/DemoNetCoreProject/backend/log-.txt",
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 1024 * 1024,
                    retainedFileCountLimit: null)
                .CreateLogger());
    }
}
