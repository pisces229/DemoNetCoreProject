using Microsoft.AspNetCore.Mvc.Testing;

namespace DemoNetCoreProject.IntegrationTest
{
    public class Test_Backend_Initialize
    {
        protected readonly HttpClient _httpClient;
        public Test_Backend_Initialize()
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
