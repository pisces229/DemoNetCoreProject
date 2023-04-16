using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace DemoNetCoreProject.Common.Utilities
{
    public class SecretUtility
    {
        public static string Create()
        {
            var randomNumberGenerator = RandomNumberGenerator.Create();
            var bytes = new byte[256 / 8];
            randomNumberGenerator.GetBytes(bytes);
            return BinaryToHex(bytes);
        }
        public static string Encrypt(string plainText, string securityKey)
        {
            var keyBinary = HexToBinary(securityKey);
            var viBinary = GetViBinary(keyBinary);
            var plainBinary = Encoding.UTF8.GetBytes(plainText);
            var gcmBlockCipher = new GcmBlockCipher(new AesEngine());
            var aeadParameters = new AeadParameters(new KeyParameter(keyBinary), 128, viBinary, null);
            gcmBlockCipher.Init(true, aeadParameters);
            var encryptedBinary = new byte[gcmBlockCipher.GetOutputSize(plainBinary.Length)];
            var len = gcmBlockCipher.ProcessBytes(plainBinary, 0, plainBinary.Length, encryptedBinary, 0);
            gcmBlockCipher.DoFinal(encryptedBinary, len);
            return Convert.ToBase64String(encryptedBinary, Base64FormattingOptions.None);
        }
        public static string Decrypt(string encryptedText, string securityKey)
        {
            var keyBinary = HexToBinary(securityKey);
            var viBinary = GetViBinary(keyBinary);
            var encryptedBinary = Convert.FromBase64String(encryptedText);
            var gcmBlockCipher = new GcmBlockCipher(new AesEngine());
            var aeadParameters = new AeadParameters(new KeyParameter(keyBinary), 128, viBinary, null);
            gcmBlockCipher.Init(false, aeadParameters);
            var plainBinary = new byte[gcmBlockCipher.GetOutputSize(encryptedBinary.Length)];
            var len = gcmBlockCipher.ProcessBytes(encryptedBinary, 0, encryptedBinary.Length, plainBinary, 0);
            gcmBlockCipher.DoFinal(plainBinary, len);
            return Encoding.UTF8.GetString(plainBinary);
        }
        private static string BinaryToHex(byte[] data)
        {
            var result = string.Empty;
            foreach (var c in data)
            {
                result += c.ToString("X2");
            }
            return result;
        }
        private static byte[] HexToBinary(string hexStr)
        {
            var result = new byte[hexStr.Length / 2];
            for (var i = 0; i < (hexStr.Length / 2); i++)
            {
                var firstNibble = byte.Parse(hexStr.Substring((2 * i), 1), NumberStyles.HexNumber);
                var secondNibble = byte.Parse(hexStr.Substring((2 * i) + 1, 1), NumberStyles.HexNumber);
                result[i] = Convert.ToByte((firstNibble << 4) | (secondNibble));
            }
            return result;
        }
        private static byte[] GetViBinary(byte[] keyBinary)
        {
            var result = new byte[keyBinary.Length / 2];
            for (var i = 0; i < keyBinary.Length / 2; ++i)
            {
                result[i] = Convert.ToByte(keyBinary[i * 2] ^ keyBinary[i * 2 + 1]);
            }
            return result;
        }
    }
}
