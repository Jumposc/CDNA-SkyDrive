using System;
using System.Security.Cryptography;
using System.Text;

namespace CDNA_SkyDrive.API
{
    internal class AES
    {
        //AES密钥
        private const string keys = "CDNAwangpanysrhh";

        public static byte[] _key1 = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xED, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        //AES加密
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="text">明文</param>
        /// <param name="key">加密密钥</param>
        /// <param name="iv">加密密钥2</param>
        /// <returns>加密字符串</returns>
        public static string EncodeAES(string text, string key = keys, byte[] iv = null)
        {
            if (iv == null)
                iv = _key1;
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.Zeros;
            rijndaelCipher.KeySize = 128;
            rijndaelCipher.BlockSize = 128;
            byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes = new byte[16];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length)
                len = keyBytes.Length;
            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = iv;
            ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
            byte[] plainText = Encoding.UTF8.GetBytes(text);
            byte[] cipherBytes = transform.TransformFinalBlock(plainText, 0, plainText.Length);
            return Convert.ToBase64String(cipherBytes);
        }

        //AES界面
        public static string DecodeAES(string text, string key = keys, byte[] iv = null)
        {
            if (iv == null)
                iv = _key1;
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.Zeros;
            rijndaelCipher.KeySize = 128;
            rijndaelCipher.BlockSize = 128;
            byte[] encrypteData = Convert.FromBase64String(text);
            byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes = new byte[16];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length)
                len = keyBytes.Length;
            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = iv;
            ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
            byte[] plainText = transform.TransformFinalBlock(encrypteData, 0, encrypteData.Length); ;
            return Encoding.UTF8.GetString(plainText);
        }
    }
}