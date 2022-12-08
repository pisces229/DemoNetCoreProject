using Microsoft.AspNetCore.Mvc.Testing;

namespace DemoNetCoreProject.IntegrationTest
{
    public class TestBackendInitialize
    {
        protected readonly HttpClient _httpClient;
        public TestBackendInitialize()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    //builder.UseEnvironment("Developement");
                });
            _httpClient = application.CreateClient();
        }
    }
}
