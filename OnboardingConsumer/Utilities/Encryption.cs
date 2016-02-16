using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OnboardingConsumer.Utilities
{
    public class Encryption
    {

        //REPLACE WITH YOUR INFORMATION
        private readonly string aesKey = "lr1Jwa9IO6l6iF5EccZ8S5fAkFMwkkkfHKyzRLntrJQ=";
        private readonly byte[] salt = System.Text.Encoding.UTF8.GetBytes("ReykerExample");

        internal async Task<string> AES_Encrypt(object objectToBeEncrypted)
        {
            var current = WindowsIdentity.GetCurrent();
            if (current != null)
            {
                return AES_Encrypt(objectToBeEncrypted, aesKey, salt);
            }
            else
            {
                return null;
            }
        }

        public string AES_Encrypt(Object objectToBeEncrypted, string keyPassword, byte[] saltBytes)
        {
            var encrypt = new EncryptionVM();
            var json = JsonConvert.SerializeObject(objectToBeEncrypted);
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;

                    AES.BlockSize = 128;

                    AES.Padding = PaddingMode.ISO10126;

                    var key = new Rfc2898DeriveBytes(keyPassword, saltBytes, 1000);

                    AES.GenerateIV();

                    AES.Key = key.GetBytes(AES.KeySize / 8);

                    encrypt.iv = Convert.ToBase64String(AES.IV);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(AES.Key, AES.IV), CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(cs))
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
            else
            {
                return default(T);
            }
        }

        public T AES_Decrypt<T>(string encryptedBytes, string keyPassword, byte[] saltBytes)
        {
            var encryptModel = JsonConvert.DeserializeObject<EncryptionVM>(encryptedBytes);

            byte[] encBytes = Convert.FromBase64String(encryptModel.ct);

            string retJson;
            using (RijndaelManaged AES = new RijndaelManaged())
            {
                AES.KeySize = 256;
                AES.BlockSize = 128;
                AES.Padding = PaddingMode.ISO10126;
                var key = new Rfc2898DeriveBytes(keyPassword, saltBytes, 1000);
                AES.Key = key.GetBytes(AES.KeySize / 8);
                AES.IV = Convert.FromBase64String(encryptModel.iv);
                AES.Mode = CipherMode.CBC;
                using (MemoryStream msDecrypt = new MemoryStream(encBytes))
                {
                    using (var cs = new CryptoStream(msDecrypt, AES.CreateDecryptor(AES.Key, AES.IV), CryptoStreamMode.Read))
                    {

                        using (StreamReader srDecrypt = new StreamReader(cs))
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
        public static async Task<string> AES_Encrypt(this Object ObjectToBeEncrypted)
        {
            Encryption enc = new Encryption();
            return await enc.AES_Encrypt(ObjectToBeEncrypted);
        }

        public static async Task<T> AES_Decrypt<T>(this string ObjectToBeDecrypted, string keyPassword, byte[] saltBytes)
        {
            Encryption enc = new Encryption();
            return await Task.Run(() => enc.AES_Decrypt<T>(ObjectToBeDecrypted, keyPassword, saltBytes));
        }

        public static async Task<T> AES_Decrypt<T>(this string ObjectToBeDecrypted)
        {
            Encryption enc = new Encryption();
            return await enc.AES_Decrypt<T>(ObjectToBeDecrypted);
        }

        public static async Task<T> AES_Decrypt<T>(this byte[] ObjectToBeDecrypted, string keyPassword, byte[] saltBytes)
        {
            var obj = Convert.ToBase64String(ObjectToBeDecrypted);
            Encryption enc = new Encryption();
            return await Task.Run(() => enc.AES_Decrypt<T>(obj, keyPassword, saltBytes));
        }

        public static async Task<T> AES_Decrypt<T>(this byte[] ObjectToBeDecrypted)
        {
            var obj = Convert.ToBase64String(ObjectToBeDecrypted);
            Encryption enc = new Encryption();
            return await enc.AES_Decrypt<T>(obj);
        }
    }
}