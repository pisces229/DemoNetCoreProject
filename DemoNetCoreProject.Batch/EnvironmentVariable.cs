namespace DemoNetCoreProject.Batch
{
    public class EnvironmentVariable
    {
        public static string? ASPNETCORE_ENVIRONMENT => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    }
}
