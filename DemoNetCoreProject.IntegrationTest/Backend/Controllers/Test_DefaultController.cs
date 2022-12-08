using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace DemoNetCoreProject.IntegrationTest.Backend.Controllers
{
    [TestClass]
    public class Test_DefaultController
    {
        private readonly HttpClient _httpClient;
        public Test_DefaultController()
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
        public async Task Json()
        {
            var requestJson = JsonConvert.SerializeObject(new
            {
                valueString = "ValueString",
                valueDate = "2000-01-01",
            });
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/default/json", content);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        [TestMethod]
        public async Task Validatable()
        {
            var requestJson = JsonConvert.SerializeObject(new
            {
                value = "",
                first = "first",
                second = "second",
            });
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/default/validatable", content);
            //Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        [TestMethod]
        public async Task JsonHttpGet()
        {
            var response = await _httpClient.GetAsync("/api/default/jsonHttpGet?text=Value&value=9&date=2020-01-01");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        [TestMethod]
        public async Task JsonHttpPost()
        {
            var json = JsonConvert.SerializeObject(new 
            { 
                text = "Value", value = 9, date = DateTime.Now 
            });
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/default/jsonHttpPost", stringContent);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        [TestMethod]
        public async Task CommonPagedQuery()
        {
            var json = JsonConvert.SerializeObject(new
            {
                data = new 
                { 
                    text = "Value",
                    value = 9,
                    date = DateTime.Now,
                },
                page = new
                {
                    pageNo = 1,
                    pageSize = 10,
                },
            });
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/default/commonPagedQuery", stringContent);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        [TestMethod]
        public async Task Upload()
        {
            using var multipartFormDataContent = new MultipartFormDataContent();
            using var byteArrayContent = new ByteArrayContent(Encoding.UTF8.GetBytes("Upload"));
            multipartFormDataContent.Add(byteArrayContent, "file", "Upload.txt");
            multipartFormDataContent.Add(new StringContent("nameUpload"), "Name");
            var response = await _httpClient.PostAsync("/api/default/upload", multipartFormDataContent);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        [TestMethod]
        public async Task Download()
        {
            var json = JsonConvert.SerializeObject(new
            {
                filename = "name",
            });
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/default/download", stringContent);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            response.Headers.ToList().ForEach(f =>
            {
                Console.WriteLine($"[{f.Key}]:[{f.Value}]");
            });
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        [TestMethod]
        public async Task SignIn()
        {
            var requestJson = JsonConvert.SerializeObject(new
            {
                account = "account",
                password = "password",
            });
            var stringContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/default/signIn", stringContent);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        [TestMethod]
        public async Task Validate()
        {
            var response = await _httpClient.GetAsync("/api/default/validate");
            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
