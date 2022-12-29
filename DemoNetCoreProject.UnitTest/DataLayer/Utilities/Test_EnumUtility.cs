using Dapper;
using DemoNetCoreProject.Common.Enums;
using DemoNetCoreProject.Common.Utilities;
using DemoNetCoreProject.DataLayer.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Data;
using System.Text;

namespace DemoNetCoreProject.UnitTest.DataLayer.Utilities
{
    [TestClass]
    public class Test_EnumUtility
    {
        [TestMethod]
        public void Test_GetOption()
        {
            var result = EnumUtility.GetOption<DefaultEnum>();
            Console.WriteLine(result);
        }
        [TestMethod]
        public void Test_GetDescription()
        {
            var result = EnumUtility.GetText<DefaultEnum>("1");
            Console.WriteLine(result);
        }
    }
}