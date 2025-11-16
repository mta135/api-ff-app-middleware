using System.Security.Cryptography;
using System.Text;

namespace FFappMiddleware.DataBase.EncryptionService
{
    public class AesEncryptionHelper
    {
        private static string PrivateKey
        {
            get
            {
                string key = "3fb7fe5dbb0643caa984f53de6fffd0f";
                return key;
            }
        }

        public static string Decrypt(string cipherText, string publicKey)
        {

            using var aesAlg = Aes.Create();

            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Key = CreateAesKey(PrivateKey);

            aesAlg.IV = Convert.FromBase64String(publicKey);

            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText));

            using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new StreamReader(csDecrypt);

            string plaintext = srDecrypt.ReadToEnd();

            return plaintext;
        }

        public static string Encrypt(string plainText, string publicKey)
        {
            byte[] encrypted;

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Key = CreateAesKey(PrivateKey);

                aesAlg.IV = Convert.FromBase64String(publicKey);

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);
        }

        private static byte[] CreateAesKey(string inputString)
        {
            return Encoding.UTF8.GetByteCount(inputString) == 32 ? Encoding.UTF8.GetBytes(inputString) : SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
    }
}
