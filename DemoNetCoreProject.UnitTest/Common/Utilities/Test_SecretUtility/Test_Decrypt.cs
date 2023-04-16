using DemoNetCoreProject.Common.Utilities;

namespace DemoNetCoreProject.UnitTest.Common.Utilities.Test_SecretUtility
{
    [TestClass]
    public class Test_Decrypt
    {
        [Ignore]
        [TestMethod]
        public void Run()
        {
            var secret = "";
            var decrypt_String = "";
            Console.WriteLine(SecretUtility.Decrypt(decrypt_String, secret));
        }
    }
}
