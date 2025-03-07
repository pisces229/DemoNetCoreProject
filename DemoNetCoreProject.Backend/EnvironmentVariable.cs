namespace DemoNetCoreProject.Backend
{
    public class EnvironmentVariable
    {
        public static string ASPNETCORE_ENVIRONMENT => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;
        public static bool IsDevelopment() => "Development".Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
        public static bool IsProduction() => "Production".Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
    }
}
