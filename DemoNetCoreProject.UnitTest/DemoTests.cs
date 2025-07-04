using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Dapper.SqlMapper;

namespace DemoNetCoreProject.UnitTest
{
    [TestClass]
    public class DemoTests
    {

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            Console.WriteLine("ClassInitialize");
        }

        [TestInitialize]
        public void TestInitialize()
        {
            Console.WriteLine("TestInitialize");
        }

        [TestMethod]
        public Task TestMethod()
        {
            Console.WriteLine("TestMethod");

            Assert.IsTrue(true);

            return Task.CompletedTask;
        }

        [TestMethod]
        [DataTestMethod]
        [DataRow("1", DisplayName = "1")]
        [DataRow("2", DisplayName = "2")]
        public Task DataTestMethodDataRow(string name)
        {
            Console.WriteLine("DataTestMethodDataRow:" + name);

            Assert.IsTrue(true);

            return Task.CompletedTask;
        }

        [TestMethod]
        [DataTestMethod]
        [DynamicData(nameof(CreateTestData), DynamicDataSourceType.Method)]
        public Task DataTestMethodDynamicData(int number, string text)
        {
            Console.WriteLine($"Number: {number}");
            Console.WriteLine($"Text: {text}");

            Assert.IsTrue(true);

            return Task.CompletedTask;
        }

        public static IEnumerable<object[]> CreateTestData()
        {
            yield return new object[] { 1, "Test1" };
            yield return new object[] { 2, "Test2" };
        }

        [TestMethod("TestMethod1Odd")]
        [TestCategory("Odd")]
        public Task TestMethod1()
        {
            Console.WriteLine("TestMethod1");

            Assert.IsTrue(true);

            return Task.CompletedTask;
        }

        [TestMethod("TestMethod2Even")]
        [TestCategory("Even")]
        public Task TestMethod2()
        {
            Console.WriteLine("TestMethod2");

            Assert.IsTrue(true);

            return Task.CompletedTask;
        }

        [TestMethod("TestMethod3Odd")]
        public Task TestMethod3()
        {
            Console.WriteLine("TestMethod3");

            Assert.IsTrue(true);

            return Task.CompletedTask;
        }

        //[TestMethod]
        //public async Task Method_InvalidInput_ReturnsBadRequest()
        //{
        //    // 模擬驗證失敗
        //    var controller = CreateController();
        //    controller.ModelState.AddModelError("Id", "Id is required");
        //    var result = await controller.Method(new InvalidRequest());
        //    result.Should().BeOfType<BadRequestObjectResult>();
        //}

        [TestCleanup]
        public void TestCleanup()
        {
            Console.WriteLine("TestCleanup");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Console.WriteLine("ClassCleanup");
        }
    }
}
