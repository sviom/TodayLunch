using LunchLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;

namespace LunchLibrary
{
    public static class PreDefined
    {
        public const string SaltPassword = "todaylunchisbest";
    }

    public static class UtilityLauncher
    {
        /// <summary>
        /// SHA256 암호화
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string EncryptSHA256(string plainText)
        {
            try
            {
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                SHA256Managed sm = new SHA256Managed();
                byte[] encryptedBytes = sm.ComputeHash(plainBytes);
                string encryptedString = Convert.ToBase64String(encryptedBytes);
                return encryptedString;
            }
            catch
            {
                return null;
            }
        }

        public static T RandomPick<T>(List<T> randomlist) where T : class, ICommon
        {
            if (randomlist.Count > 0)
            {
                var random = new Random();
                int num = random.Next(0, randomlist.Count);

                var randomPicked = randomlist[num];

                if (randomPicked is Place place)
                {
                    place.UsingCount++;
                    //SqlLauncher.Update(place);
                }
                return randomPicked;
            }

            return default;
        }

        public static string ConvertGuidToBase64(Guid guid)
        {
            var urlSafeBase64String = Convert.ToBase64String(guid.ToByteArray());
            return urlSafeBase64String.Replace('+', '-').Replace('/', '_').TrimEnd('=');
        }

        public static Guid ConvertBase64ToGuid(string base64)
        {
            base64 = base64.Replace('-', '+').Replace('_', '/');
            int paddings = base64.Length % 4;
            if (paddings > 0)
            {
                base64 += new string('=', 4 - paddings);
            }

            var decodedByte = Convert.FromBase64String(base64);
            return new Guid(decodedByte);
        }

        /// <summary>
        /// AES256 암호화
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string EncryptAES256(string plainText, string password)
        {
            try
            {
                using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
                {
                    byte[] textBytes = System.Text.Encoding.Unicode.GetBytes(plainText);
                    byte[] salt = Encoding.ASCII.GetBytes(password.Length.ToString());
                    PasswordDeriveBytes deriveBytes = new PasswordDeriveBytes(password, salt);

                    ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(deriveBytes.GetBytes(32), deriveBytes.GetBytes(16));

                    using (var memoryStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            if (cryptoStream.CanWrite)
                            {
                                cryptoStream.Write(textBytes, 0, textBytes.Length);
                            }
                        }

                        var memoryArray = memoryStream.ToArray();
                        return Convert.ToBase64String(memoryArray);
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// AES256 복호화
        /// </summary>
        /// <param name="base64String"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string DecryptAES256(string base64String, string password)
        {
            try
            {
                string result = string.Empty;
                using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
                {

                    byte[] encryptData = Convert.FromBase64String(base64String);
                    byte[] salt = Encoding.ASCII.GetBytes(password.Length.ToString());
                    PasswordDeriveBytes deriveBytes = new PasswordDeriveBytes(password, salt);

                    ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(deriveBytes.GetBytes(32), deriveBytes.GetBytes(16));

                    using (var memoryStream = new MemoryStream(encryptData))
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            if (cryptoStream.CanRead)
                            {
                                byte[] decryptedData = new byte[encryptData.Length];

                                int count = cryptoStream.Read(decryptedData, 0, decryptedData.Length);
                                result = Encoding.Unicode.GetString(decryptedData, 0, count);
                            }
                        }
                    }

                    return result;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public static T ConvertType<T>(this object value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }

    /// <summary>
    /// Model들이 무조건 가져야 하는 행동들 정의
    /// </summary>
    public abstract class ModelActionGuide
    {
        public abstract List<T> GetAll<T>(Expression<Func<T, bool>> expression = null) where T : class, ICommon;
        public abstract T Get<T>(Expression<Func<T, bool>> expression = null) where T : class, ICommon;
        public abstract T Insert<T>(T input) where T : class, ICommon;
        public abstract T Update<T>(T input) where T : class, ICommon;
        public abstract bool Delete<T>(T input) where T : class, ICommon;
    }

    //public static class ModelExtension
    //{
    //    public static T Insert<T>(this T input) where T : class, ICommon
    //    {
    //        return SqlLauncher.Insert(input);
    //    }
    //    public static T Update<T>(this T input) where T : class, ICommon
    //    {
    //        return SqlLauncher.Update(input);
    //    }
    //    public static bool Delete<T>(this T input) where T : class, ICommon
    //    {
    //        return SqlLauncher.Delete(input);
    //    }
    //}
}
