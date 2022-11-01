using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace DemoNetCoreProject.IntegrationTest.Backend.Controllers
{
    [TestClass]
    public class UnitTest_DefaultController
    {
        private readonly HttpClient _httpClient;
        public UnitTest_DefaultController()
        {
            // Arrange
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    // ... Configure test services
                });
            _httpClient = application.CreateClient();
        }
        [TestMethod]
        public async Task Run()
        {
            // Act
            var response = await _httpClient.GetAsync("/api/default/run");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            // Assert
            Assert.AreEqual("Run", responseString);
        }
        [TestMethod]
        public async Task Validate()
        {
            // Act
            var response = await _httpClient.GetAsync("/api/default/validate");
            // Assert
            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
