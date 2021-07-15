using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OperationAdminApi.Utils
{
    public static class EncryptDecryptHelper
    {
        private static byte[] keyAndIvBytes;
        static EncryptDecryptHelper()
        {
            keyAndIvBytes = Encoding.UTF8.GetBytes(Startup.FrontEndKeyStr);
        }

        public static string EncryptAndEncode(this string plaintext)
        {
            return ByteArrayToHexString(AesEncrypt(plaintext));
        }

        public static string DecodeAndDecrypt(this string cipherText)
        {
            string DecodeAndDecrypt = AesDecrypt(Convert.FromBase64String(cipherText));
            return (DecodeAndDecrypt);
        }



        private static string ByteArrayToHexString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", string.Empty);
        }

        private static string AesDecrypt(byte[] inputBytes)
        {
            string plaintext = string.Empty;

            try
            {
                byte[] outputBytes = inputBytes;
                using (MemoryStream memoryStream = new MemoryStream(outputBytes))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, GetCryptoAlgorithm().CreateDecryptor(keyAndIvBytes, keyAndIvBytes), CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(cryptoStream))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            catch
            {
                plaintext = "keyError";
            }
            return plaintext;
        }

        private static byte[] AesEncrypt(string inputText)
        {
            byte[] result = null;
            try
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(inputText);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, GetCryptoAlgorithm().CreateEncryptor(keyAndIvBytes, keyAndIvBytes), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(inputBytes, 0, inputBytes.Length);
                        cryptoStream.FlushFinalBlock();

                        result = memoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private static RijndaelManaged GetCryptoAlgorithm()
        {
            RijndaelManaged algorithm = new RijndaelManaged();
            algorithm.Padding = PaddingMode.PKCS7;
            algorithm.Mode = CipherMode.CBC;
            algorithm.KeySize = 128;
            algorithm.BlockSize = 128;
            algorithm.FeedbackSize = 128;
            return algorithm;
        }

    }
}
