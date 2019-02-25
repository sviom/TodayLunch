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

}
