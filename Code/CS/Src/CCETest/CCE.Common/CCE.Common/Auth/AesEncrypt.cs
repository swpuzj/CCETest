using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace CCE.Common.Auth
{
    public class AesEncrypt
    {
        /// <summary> 
        /// 获取向量 
        /// </summary> 
        private static string Iv => "TestaTest1234567";

        #region ========加密========

        /// <summary>
        /// 默认Key加密(Key=SobeyHive1234567)
        /// </summary>
        /// <param name="text">需要加密的明文</param>
        /// <returns>加密的明文文</returns>
        public static string Encrypt(string text, bool retHexStr = false)
        {
            return Encrypt(text, Iv, retHexStr);
        }

        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="text">需要加密的明文</param> 
        /// <param name="sKey">密钥</param> 
        /// <returns>加密的明文文</returns> 
        public static string Encrypt(string text, string sKey, bool retHexStr = false)
        {
            return Encrypt(text, sKey, Iv, retHexStr);
        }

        /// <summary>
        /// 加密数据 
        /// </summary> 
        /// <param name="text">需要加密的明文</param> 
        /// <param name="sKey">密钥</param> 
        /// <param name="vector">向量</param>
        /// <returns>加密后的密文</returns>
        public static string Encrypt(string text, string sKey, string vector, bool retHexStr = false)
        {
            var dataKey = Encoding.UTF8.GetBytes(text);
            var encryptData = Encrypt(dataKey, sKey, vector);
            if (retHexStr)
            {
                var hex = BitConverter.ToString(encryptData).Replace("-", string.Empty).ToLower();
                return hex;
            }
            else
            {
                return Convert.ToBase64String(encryptData);
            }
        }

        /// <summary>
        /// 加密数据 
        /// </summary> 
        /// <param name="data">需要加密的明文</param> 
        /// <param name="sKey">密钥</param> 
        /// <returns>加密后的密文</returns>
        public static string Encrypt(byte[] data, string sKey)
        {
            var dataKey = data;
            var encryptData = Encrypt(dataKey, sKey, Iv);
            return Convert.ToBase64String(encryptData);
        }

        /// <summary>
        /// AES加密byte[]
        /// </summary>
        /// <param name="data">被加密的明文</param>
        /// <param name="key">密钥</param>
        /// <param name="vector">向量</param>
        /// <returns>密文</returns>
        public static byte[] Encrypt(byte[] data, string key, string vector)
        {
            var bKey = Encoding.ASCII.GetBytes(key);
            var bVector = Encoding.ASCII.GetBytes(vector);

            byte[] cryptograph; // 加密后的密文

            var aes = Rijndael.Create();

            try
            {
                aes.Key = bKey;
                aes.IV = bVector;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                var cTransform = aes.CreateEncryptor();
                var resultArray = cTransform.TransformFinalBlock(data, 0, data.Length);
                cryptograph = resultArray;
            }
            catch
            {
                cryptograph = null;
            }

            return cryptograph;
        }

        #endregion

        #region ========解密========

        /// <summary>
        /// 默认Key解密(Key=SobeyHive1234567)
        /// </summary>
        /// <param name="text">要解密的密文</param>
        /// <returns>解密的明文</returns>
        public static string Decrypt(string text, bool isHexStr = false)
        {
            return Decrypt(text, Iv, isHexStr);
        }

        /// <summary>
        /// 解密数据 
        /// </summary>
        /// <param name="text">要解密的密文</param>
        /// <param name="sKey">密钥</param>
        /// <returns>解密的明文</returns>
        public static string Decrypt(string text, string sKey, bool isHexStr = false)
        {
            return Decrypt(text, sKey, Iv, isHexStr);
        }

        /// <summary>
        /// 解密数据 
        /// </summary> 
        /// <param name="text">要解密的密文</param> 
        /// <param name="sKey">密钥</param> 
        /// <param name="vector">向量</param>
        /// <returns>解密的明文</returns>
        public static string Decrypt(string text, string sKey, string vector, bool isHexStr = false)
        {
            byte[] dataKey;
            if (isHexStr)
            {
                //Regex.Replace("12345678", "(.{2})", "$0-");
                dataKey = Regex.Matches(text, @".{2}")
                    .Cast<Match>()
                    .Select(match => Convert.ToByte(match.Value, 16))
                    .ToArray();
            }
            else
            {
                dataKey = Convert.FromBase64String(text);
            }

            var encryptData = Decrypt(dataKey, sKey, vector);
            return Encoding.UTF8.GetString(encryptData);
        }

        /// <summary>
        /// 解密数据 
        /// </summary> 
        /// <param name="data">要解密的密文</param> 
        /// <param name="sKey">密钥</param>
        /// <returns>解密的明文</returns>
        public static string Decrypt(byte[] data, string sKey)
        {
            var dataKey = data;
            var encryptData = Decrypt(dataKey, sKey, Iv);
            return Encoding.UTF8.GetString(encryptData);
        }

        /// <summary>
        /// AES解密byte[]
        /// </summary>
        /// <param name="data">被解密的密文</param>
        /// <param name="key">密钥</param>
        /// <param name="vector">向量</param>
        /// <returns>明文</returns>
        public static byte[] Decrypt(byte[] data, string key, string vector)
        {
            var bKey = Encoding.ASCII.GetBytes(key);
            var bVector = Encoding.ASCII.GetBytes(vector);

            byte[] original; // 解密后的明文

            var aes = Rijndael.Create();

            try
            {
                aes.Key = bKey;
                aes.IV = bVector;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                var cTransform = aes.CreateDecryptor();
                var resultArray = cTransform.TransformFinalBlock(data, 0, data.Length);

                original = resultArray;
            }
            catch
            {
                original = null;
            }

            return original;
        }

        #endregion
    }
}
