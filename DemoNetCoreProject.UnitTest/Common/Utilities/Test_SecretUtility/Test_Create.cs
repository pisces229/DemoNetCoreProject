using DemoNetCoreProject.Common.Utilities;

namespace DemoNetCoreProject.UnitTest.Common.Utilities.Test_SecretUtility
{
    [TestClass]
    public class Test_Create
    {
        //[Ignore]
        [TestMethod]
        public void Run()
        {
            Console.WriteLine(SecretUtility.Create());
        }
    }
}
