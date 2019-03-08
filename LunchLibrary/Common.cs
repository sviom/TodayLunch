using LunchLibrary.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LunchLibrary
{
    public static class PreDefined
    {
#if DEBUG
        public static string ServiceApiUrl => "http://localhost:7011/api/";
#else
        public static string ServiceApiUrl => "http://todaylunchapi.azurewebsites.net/api/";
#endif
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
    }

    /// <summary>
    /// Model들이 무조건 가져야 하는 행동들 정의
    /// </summary>
    public abstract class ModelActionGuide
    {
        public abstract bool Insert<T>(T input) where T : class, ICommon;
        public abstract bool Update<T>(T input) where T : class, ICommon;
    }

    /// <summary>
    /// 모델들이 직접 DB에 접근하게 하지 않기 위한 확장
    /// </summary>
    public static class ModelExtension
    {
        public static T Insert<T>(this T input) where T : class, ICommon
        {
            return SqlLauncher.Insert(input);
        }
        public static T Update<T>(this T input) where T : class, ICommon
        {
            return SqlLauncher.Update(input);
        }
    }
}
