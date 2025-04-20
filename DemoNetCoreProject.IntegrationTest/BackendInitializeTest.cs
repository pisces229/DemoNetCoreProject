using Microsoft.AspNetCore.Mvc.Testing;

namespace DemoNetCoreProject.IntegrationTest
{
    public class BackendInitializeTest
    {
        protected readonly HttpClient _httpClient;
        public BackendInitializeTest()
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
