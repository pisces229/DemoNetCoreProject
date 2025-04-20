using DemoNetCoreProject.Backend.Models.Common;
using DemoNetCoreProject.Backend.Models.Default;
using DemoNetCoreProject.Common.Dtos;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace DemoNetCoreProject.IntegrationTest.Backend.Controllers
{
    [TestClass]
    public class DefaultControllerTests : BackendInitializeTest
    {
        private const string BASE_URL = "/api/default/";
        [TestMethod]
        public async Task Run()
        {
            var requestUri = $"{BASE_URL}Run";
            Console.WriteLine($"[{requestUri}]");
            var response = await _httpClient.GetAsync(requestUri);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        [TestMethod]
        public async Task FromQueryString()
        {
            var inputModel = "123";
            //var inputModel = "abc";
            //var inputModel = "測試";
            var query = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("inputModel", inputModel),
            };
            var requestUri = QueryHelpers.AddQueryString($"{BASE_URL}FromQueryString", query!);
            Console.WriteLine($"[{requestUri}]");
            var response = await _httpClient.GetAsync(requestUri);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        [TestMethod]
        public async Task FromBodyString()
        {
            var inputModel = "123";
            //var inputModel = "abc";
            //var inputModel = "測試";
            var requestJson = JsonConvert.SerializeObject(inputModel);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
            var requestUri = $"{BASE_URL}FromBodyString";
            Console.WriteLine($"[{requestUri}]");
            var response = await _httpClient.PostAsync(requestUri, content);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        [TestMethod]
        public async Task FromQueryModel()
        {
            var query = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("value", "測試"),
                new KeyValuePair<string, string>("values[]", "1"),
                new KeyValuePair<string, string>("values[]", "2"),
            };
            var requestUri = QueryHelpers.AddQueryString($"{BASE_URL}FromQueryModel", query!);
            Console.WriteLine($"[{requestUri}]");
            var response = await _httpClient.GetAsync(requestUri);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseContent);
            var outputModel = JsonConvert.DeserializeObject<CommonOutputModel<string>>(responseContent);
            Assert.IsTrue(outputModel.Success);
        }
        [TestMethod]
        public async Task FromBodyModel()
        {
            var requestJson = JsonConvert.SerializeObject(new
            {
                value = "測試",
                values = new[] { "1", "2" },
            });
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
            var requestUri = $"{BASE_URL}FromBodyModel";
            Console.WriteLine($"[{requestUri}]");
            var response = await _httpClient.PostAsync(requestUri, content);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseContent);
            var outputModel = JsonConvert.DeserializeObject<CommonOutputModel<string>>(responseContent);
            Assert.IsTrue(outputModel.Success);
        }
        [TestMethod]
        public async Task FromFormModel()
        {
            using var byteArrayContent = new ByteArrayContent(Encoding.UTF8.GetBytes("Upload"));
            using var multipartFormDataContent = new MultipartFormDataContent
            {
                { new StringContent("Name"), "value" },
                { new StringContent("1"), "values" },
                { new StringContent("2"), "values" },
                { byteArrayContent, "file", "Upload.txt" }
            };
            var requestUri = $"{BASE_URL}FromFormModel";
            Console.WriteLine($"[{requestUri}]");
            var response = await _httpClient.PostAsync(requestUri, multipartFormDataContent);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseContent);
            var outputModel = JsonConvert.DeserializeObject<CommonOutputModel<string>>(responseContent);
            Assert.IsTrue(outputModel.Success);
        }
        [TestMethod]
        public async Task PageQueryBind()
        {
            var query = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("pageSize", "10"),
                new KeyValuePair<string, string>("pageNo", "1"),
                new KeyValuePair<string, string>("value", "測試"),
                new KeyValuePair<string, string>("values[]", "1"),
                new KeyValuePair<string, string>("values[]", "2"),
            };
            var requestUri = QueryHelpers.AddQueryString($"{BASE_URL}PageQueryBind", query!);
            Console.WriteLine($"[{requestUri}]");
            var response = await _httpClient.GetAsync(requestUri);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseContent);
            var outputModel = JsonConvert.DeserializeObject<CommonOutputModel<CommonPageOutputDto<DefaultPageQueryOutputModel>>>(responseContent);
            Assert.IsTrue(outputModel.Success);
            Console.WriteLine(outputModel.Data?.TotalCount);
        }
        [TestMethod]
        public async Task PageQueryJson()
        {
            var requestJson = JsonConvert.SerializeObject(new
            {
                pageSize = "10",
                pageNo = "1",
                value = "測試",
                values = new[] { "1", "2" },
            });
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
            var requestUri = $"{BASE_URL}PageQueryJson";
            Console.WriteLine($"[{requestUri}]");
            var response = await _httpClient.PostAsync(requestUri, content);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseContent);
            var outputModel = JsonConvert.DeserializeObject<CommonOutputModel<CommonPageOutputDto<DefaultPageQueryOutputModel>>>(responseContent);
            Assert.IsTrue(outputModel.Success);
            Console.WriteLine(outputModel.Data?.TotalCount);
        }
        [TestMethod]
        public async Task Download()
        {
            var requestUri = $"{BASE_URL}Download";
            Console.WriteLine($"[{requestUri}]");
            var response = await _httpClient.GetAsync(requestUri);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            foreach (var header in response.Headers)
            {
                Console.WriteLine($"[{header.Key}]:[{string.Join(",", header.Value)}]");
            }
            foreach (var header in response.Content.Headers)
            {
                Console.WriteLine($"[{header.Key}]:[{string.Join(",", header.Value)}]");
            }
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        [TestMethod]
        public async Task RunSignIn()
        {
            await DoSignIn();
        }
        [TestMethod]
        public async Task RunOK()
        {
            var jwt = await DoSignIn();
            {
                var httpStatusCode = await DoValidate(jwt);
                switch (httpStatusCode)
                {
                    case HttpStatusCode.OK:
                        Console.WriteLine("JWT is working");
                        break;
                    default:
                        Assert.Fail($"[{httpStatusCode}]");
                        break;
                }
            }
            await Task.Delay(3000);
            {
                var httpStatusCode = await DoValidate(jwt);
                switch (httpStatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        jwt = await DoRefresh(jwt);
                        break;
                    case HttpStatusCode.Forbidden:
                        return;
                    default:
                        Assert.Fail($"[{httpStatusCode}]");
                        break;
                }
            }
            {
                var requestJson = JsonConvert.SerializeObject(jwt);
                var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                Console.WriteLine("SignOut...");
                var response = await _httpClient.PostAsync($"{BASE_URL}SignOut", content);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                var responseContent = await response.Content.ReadAsStringAsync();
                var commonOutputModel = JsonConvert.DeserializeObject<CommonOutputDto<string>>(responseContent);
                Assert.IsTrue(commonOutputModel.Success);
            }
            {
                var httpStatusCode = await DoValidate(jwt);
                switch (httpStatusCode)
                {
                    case HttpStatusCode.Forbidden:
                        Console.WriteLine("JWT is not working");
                        break;
                    default:
                        Assert.Fail($"[{httpStatusCode}]");
                        break;
                }
            }
        }
        [TestMethod]
        public async Task RunForbidden()
        {
            var jwt = await DoSignIn();
            await Task.Delay(6000);
            var requestJson = JsonConvert.SerializeObject(jwt);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
            Console.WriteLine("Refresh...");
            var response = await _httpClient.PostAsync($"{BASE_URL}Refresh", content);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            var commonOutputModel = JsonConvert.DeserializeObject<CommonOutputDto<string>>(responseContent);
            Assert.IsFalse(commonOutputModel.Success);
        }
        private async Task<string> DoSignIn()
        {
            var requestJson = JsonConvert.SerializeObject(new
            {
                Account = "Account",
                Password = "Password",
            });
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
            Console.WriteLine("SignIn...");
            var response = await _httpClient.PostAsync($"{BASE_URL}SignIn", content);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            var commonOutputModel = JsonConvert.DeserializeObject<CommonOutputDto<string>>(responseContent);
            Assert.IsTrue(commonOutputModel.Success);
            return commonOutputModel.Data!;
        }
        private async Task<HttpStatusCode> DoValidate(string jwt)
        {
            var name = "Authorization";
            var keys = _httpClient.DefaultRequestHeaders.Where(p => p.Key == name);
            if (keys.Any())
            {
                _httpClient.DefaultRequestHeaders.Remove(name);
            }
            _httpClient.DefaultRequestHeaders.Add(name, $"Bearer {jwt}");
            Console.WriteLine("Validate...");
            var response = await _httpClient.GetAsync($"{BASE_URL}Validate");
            return response.StatusCode;
        }
        private async Task<string> DoRefresh(string jwt)
        {
            var requestJson = JsonConvert.SerializeObject(jwt);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
            Console.WriteLine("Refresh...");
            var response = await _httpClient.PostAsync($"{BASE_URL}Refresh", content);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            var commonOutputModel = JsonConvert.DeserializeObject<CommonOutputDto<string>>(responseContent);
            Assert.IsTrue(commonOutputModel.Success);
            return commonOutputModel.Data!;
        }
    }
}
