using DemoNetCoreProject.Common.Utilities;

namespace DemoNetCoreProject.UnitTest.Common.Utilities.Test_SecretUtility
{
    [TestClass]
    public class Test_Encrypt
    {
        [Ignore]
        [TestMethod]
        public void Run()
        {
            var secret = "";
            var appsettings_String = "";
            var encrypt_String = SecretUtility.Encrypt(appsettings_String, secret);
            Console.WriteLine(encrypt_String);
            var decrypt_String = SecretUtility.Decrypt(encrypt_String, secret);
            Assert.AreEqual(appsettings_String, decrypt_String);

        }
    }
}
