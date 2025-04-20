using DemoNetCoreProject.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoNetCoreProject.UnitTest.Common.Utilities
{
    [TestClass]
    public class SecretUtilityTests
    {

        [Ignore]
        [TestMethod]
        public void Create()
        {
            Console.WriteLine(SecretUtility.Create());
        }

        [Ignore]
        [TestMethod]
        public void Decrypt()
        {
            var secret = "";
            var decrypt_String = "";
            Console.WriteLine(SecretUtility.Decrypt(decrypt_String, secret));
        }

        [Ignore]
        [TestMethod]
        public void EncryptDecrypt()
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
