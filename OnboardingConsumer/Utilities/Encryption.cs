using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OnboardingConsumer.Utilities
{
    public class Encryption
    {
        //REPLACE WITH YOUR INFORMATION
        private readonly string aesKey = "AESKEY";
        private readonly byte[] salt = Encoding.UTF8.GetBytes("SALT");

        internal async Task<string> AES_Encrypt(object objectToBeEncrypted)
        {
            var current = WindowsIdentity.GetCurrent();
            if (current != null)
            {
                return AES_Encrypt(objectToBeEncrypted, aesKey, salt);
            }
            return null;
        }

        public string AES_Encrypt(object objectToBeEncrypted, string keyPassword, byte[] saltBytes)
        {
            var encrypt = new EncryptionVM();
            var json = JsonConvert.SerializeObject(objectToBeEncrypted);
            using (var ms = new MemoryStream())
            {
                using (var AES = new RijndaelManaged())
                {
                    AES.KeySize = 128;

                    AES.BlockSize = 128;

                    AES.Padding = PaddingMode.PKCS7;

                    var key = new Rfc2898DeriveBytes(keyPassword, saltBytes, 1000);

                    AES.GenerateIV();

                    AES.Key = key.GetBytes(AES.KeySize/8);

                    encrypt.iv = Convert.ToBase64String(AES.IV);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(AES.Key, AES.IV), CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(cs))
                        {
                            swEncrypt.Write(json);
                        }
                    }
                    encrypt.ct = Convert.ToBase64String(ms.ToArray());
                }
            }
            return JsonConvert.SerializeObject(encrypt);
        }

        internal async Task<T> AES_Decrypt<T>(string bytebytestodecrypt)
        {
            var current = WindowsIdentity.GetCurrent();
            if (current != null)
            {
                return AES_Decrypt<T>(bytebytestodecrypt, aesKey, salt);
            }
            return default(T);
        }

        public T AES_Decrypt<T>(string encryptedBytes, string keyPassword, byte[] saltBytes)
        {
            var encryptModel = JsonConvert.DeserializeObject<EncryptionVM>(encryptedBytes);

            var encBytes = Convert.FromBase64String(encryptModel.ct);

            string retJson;
            using (var AES = new RijndaelManaged())
            {
                AES.KeySize = 128;
                AES.BlockSize = 128;
                AES.Padding = PaddingMode.PKCS7;
                var key = new Rfc2898DeriveBytes(keyPassword, saltBytes, 1000);
                AES.Key = key.GetBytes(AES.KeySize/8);
                AES.IV = Convert.FromBase64String(encryptModel.iv);
                AES.Mode = CipherMode.CBC;
                using (var msDecrypt = new MemoryStream(encBytes))
                {
                    using (
                        var cs = new CryptoStream(msDecrypt, AES.CreateDecryptor(AES.Key, AES.IV), CryptoStreamMode.Read)
                        )
                    {
                        using (var srDecrypt = new StreamReader(cs))
                        {
                            retJson = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return JsonConvert.DeserializeObject<T>(retJson);
        }

        public class EncryptionVM
        {
            public string iv { get; set; }
            public string ct { get; set; }
        }
    }

    public static class encryptHelpers
    {
        public static async Task<string> AES_Encrypt(this object ObjectToBeEncrypted)
        {
            var enc = new Encryption();
            return await enc.AES_Encrypt(ObjectToBeEncrypted);
        }

        public static async Task<T> AES_Decrypt<T>(this string ObjectToBeDecrypted, string keyPassword, byte[] saltBytes)
        {
            var enc = new Encryption();
            return await Task.Run(() => enc.AES_Decrypt<T>(ObjectToBeDecrypted, keyPassword, saltBytes));
        }

        public static async Task<T> AES_Decrypt<T>(this string ObjectToBeDecrypted)
        {
            var enc = new Encryption();
            return await enc.AES_Decrypt<T>(ObjectToBeDecrypted);
        }

        public static async Task<T> AES_Decrypt<T>(this byte[] ObjectToBeDecrypted, string keyPassword, byte[] saltBytes)
        {
            var obj = Convert.ToBase64String(ObjectToBeDecrypted);
            var enc = new Encryption();
            return await Task.Run(() => enc.AES_Decrypt<T>(obj, keyPassword, saltBytes));
        }

        public static async Task<T> AES_Decrypt<T>(this byte[] ObjectToBeDecrypted)
        {
            var obj = Convert.ToBase64String(ObjectToBeDecrypted);
            var enc = new Encryption();
            return await enc.AES_Decrypt<T>(obj);
        }
    }
}