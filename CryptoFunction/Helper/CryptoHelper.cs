using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CryptoFunction.Helper
{
    public class CryptoHelper
    {
        static CryptoResult result = new CryptoResult();

        /// <summary>
        /// Encrypts given text with key
        /// </summary>
        /// <param name="textToEncrypt"></param>
        /// <param name="key"></param>
        /// <returns>CryptoResult Object</returns>
        public static CryptoResult Encrypt(string textToEncrypt, string key)
        {
            try
            {
                result = new CryptoResult();

                byte[] bytesBuff = Encoding.Unicode.GetBytes(textToEncrypt);
                using (Aes aes = Aes.Create())
                {
                    Rfc2898DeriveBytes crypto = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    aes.Key = crypto.GetBytes(32);
                    aes.IV = crypto.GetBytes(16);
                    using (MemoryStream mStream = new MemoryStream())
                    {
                        using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cStream.Write(bytesBuff, 0, bytesBuff.Length);
                            cStream.Close();
                        }
                        textToEncrypt = Convert.ToBase64String(mStream.ToArray());
                        result.Content = textToEncrypt;
                        result.HasError = false;
                        result.Error = "No Error";
                    }
                }
            }
            catch (Exception x)
            {
                result.HasError = true;
                result.Error = x.Message;
            }
            //return inText;
            return result;
        }

        /// <summary>
        /// Decrypts provided cryptoText with given key
        /// </summary>
        /// <param name="cryptoText"></param>
        /// <param name="key"></param>
        /// <returns>CryptoResult Object</returns>
        public static CryptoResult Decrypt(string cryptoText, string key)
        {
            try
            {
                result = new CryptoResult();

                cryptoText = cryptoText.Replace(" ", "+");
                byte[] bytesBuff = Convert.FromBase64String(cryptoText);
                using (Aes aes = Aes.Create())
                {
                    Rfc2898DeriveBytes crypto = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    aes.Key = crypto.GetBytes(32);
                    aes.IV = crypto.GetBytes(16);
                    using (MemoryStream mStream = new MemoryStream())
                    {
                        using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cStream.Write(bytesBuff, 0, bytesBuff.Length);
                            cStream.Close();
                        }
                        cryptoText = Encoding.Unicode.GetString(mStream.ToArray());
                        result.Content = cryptoText;
                        result.HasError = false;
                        result.Error = "No Error";
                    }
                }
            }
            catch (Exception x)
            {
                result.HasError = true;
                result.Error = x.Message;
            }
            //return cryptTxt;
            return result;
        }
    }
}
