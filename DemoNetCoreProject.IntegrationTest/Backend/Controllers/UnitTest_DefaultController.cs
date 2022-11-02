using DemoNetCoreProject.Backend.Models.Default;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace DemoNetCoreProject.IntegrationTest.Backend.Controllers
{
    [TestClass]
    public class UnitTest_DefaultController
    {
        private readonly HttpClient _httpClient;
        public UnitTest_DefaultController()
        {
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
            var response = await _httpClient.GetAsync("/api/default/validate");
            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }
        [TestMethod]
        public async Task JsonHttpGet()
        {
            var response = await _httpClient.GetAsync("/api/default/jsonHttpGet?Text=Value&Value=9&Date=2020-01-01");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        [TestMethod]
        public async Task JsonHttpPost()
        {
            var json = JsonConvert.SerializeObject(new DefaultJsonHttpPostModel { Text = "Value", Value = 9, Date = DateTime.Now });
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/default/jsonHttpPost", stringContent);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        [TestMethod]
        public async Task Upload()
        {
            using var multipartFormDataContent = new MultipartFormDataContent();
            multipartFormDataContent.Add(new StringContent("NameUpload"), "Name");
            var byteArrayContent = new ByteArrayContent(Encoding.UTF8.GetBytes("Upload"));
            multipartFormDataContent.Add(byteArrayContent, "File", "Upload.txt");
            var response = await _httpClient.PostAsync("/api/default/upload", multipartFormDataContent);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        [TestMethod]
        public async Task Download()
        {
            var response = await _httpClient.GetAsync("/api/default/download");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }
}
