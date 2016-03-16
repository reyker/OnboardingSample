using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using EncrpytionTest.Models;
using Newtonsoft.Json;

namespace EncrpytionTest.Utilities
{
    public class Encryption
    {
        internal async Task<string> AES_Encrypt(object objectToBeEncrypted, ApiUser user)
        {
            var salt = System.Text.Encoding.UTF8.GetBytes(user.Salt);
            var aesKey = user.AESKey;
            return await AES_Encrypt(objectToBeEncrypted, aesKey, salt);
        }

        public async Task<string> AES_Encrypt(Object objectToBeEncrypted, string keyPassword, byte[] saltBytes)
        {
            var encrypt = new EncryptionVm();
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

                    AES.Key = key.GetBytes(AES.KeySize / 8);

                    encrypt.iv = Convert.ToBase64String(AES.IV);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(AES.Key, AES.IV), CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(cs))
                        {
                            await swEncrypt.WriteAsync(json);
                        }

                    }
                    encrypt.ct = Convert.ToBase64String(ms.ToArray());
                }
            }
            return JsonConvert.SerializeObject(encrypt);
        }

        internal async Task<T> AES_Decrypt<T>(string bytebytestodecrypt, ApiUser user)
        {
            var salt = System.Text.Encoding.UTF8.GetBytes(user.Salt);
            var aesKey = user.AESKey;
            return await AES_Decrypt<T>(bytebytestodecrypt, aesKey, salt);
        }

        public async Task<T> AES_Decrypt<T>(string encryptedBytes, string keyPassword, byte[] saltBytes)
        {
            var encryptModel = JsonConvert.DeserializeObject<EncryptionVm>(encryptedBytes);

            var encBytes = Convert.FromBase64String(encryptModel.ct);

            string retJson;
            using (var AES = new RijndaelManaged())
            {
                AES.KeySize = 128;
                AES.BlockSize = 128;
                AES.Padding = PaddingMode.PKCS7;
                var key = new Rfc2898DeriveBytes(keyPassword, saltBytes, 1000);
                AES.Key = key.GetBytes(AES.KeySize / 8);
                AES.IV = Convert.FromBase64String(encryptModel.iv);
                AES.Mode = CipherMode.CBC;
                using (var msDecrypt = new MemoryStream(encBytes))
                {
                    using (var cs = new CryptoStream(msDecrypt, AES.CreateDecryptor(AES.Key, AES.IV), CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(cs))
                        {

                            retJson = await srDecrypt.ReadToEndAsync();
                        }
                    }
                }


            }
            return JsonConvert.DeserializeObject<T>(retJson);

        }
    }

    public class EncryptionVm
    {
        public string iv { get; set; }
        public string ct { get; set; }
    }

    public static class EncryptHelpers
    {
        public static async Task<string> AES_Encrypt(this Object objectToBeEncrypted, ApiUser user)
        {
            var enc = new Encryption();
            return await enc.AES_Encrypt(objectToBeEncrypted, user);
        }

        public static async Task<T> AES_Decrypt<T>(this string objectToBeDecrypted, string keyPassword, byte[] saltBytes)
        {
            var enc = new Encryption();
            return await Task.Run(() => enc.AES_Decrypt<T>(objectToBeDecrypted, keyPassword, saltBytes));
        }

        public static async Task<T> AES_Decrypt<T>(this string objectToBeDecrypted, ApiUser user)
        {

            var enc = new Encryption();
            return await enc.AES_Decrypt<T>(objectToBeDecrypted, user);
        }

        public static async Task<T> AES_Decrypt<T>(this byte[] objectToBeDecrypted, string keyPassword, byte[] saltBytes)
        {
            var obj = Convert.ToBase64String(objectToBeDecrypted);
            var enc = new Encryption();
            return await Task.Run(() => enc.AES_Decrypt<T>(obj, keyPassword, saltBytes));
        }

        public static async Task<T> AES_Decrypt<T>(this byte[] objectToBeDecrypted, ApiUser user)
        {
            var obj = Convert.ToBase64String(objectToBeDecrypted);
            var enc = new Encryption();
            return await enc.AES_Decrypt<T>(obj, user);
        }
    }
}