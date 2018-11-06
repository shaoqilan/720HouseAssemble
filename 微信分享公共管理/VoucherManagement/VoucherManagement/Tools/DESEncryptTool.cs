using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace VoucherManagement.Tools
{
    /// <summary>
    /// CBC模式  填充方式：PKCS7Padding      和PKCS5Padding
    /// </summary>
    public class DESEncryptTool
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Content">加密的内容</param>
        /// <param name="Key">加密的key</param>
        /// <returns></returns>
        public static string Encrypt(string EncryptString, string Key= "40BE0AD45AE894397FC89F7E2D82662C")
        {
            try
            {
                string RGBIV = Key.Substring(0, 24);
                string SecretKey = Key.Substring(24, 8);
                byte[] rgbKey = Encoding.UTF8.GetBytes(SecretKey);
                byte[] rgbIV = Encoding.UTF8.GetBytes(RGBIV);
                byte[] inputByteArray = Encoding.UTF8.GetBytes(EncryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                DCSP.Mode = CipherMode.CBC;
                DCSP.Padding = PaddingMode.PKCS7;
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                //进行编码返回
                return HttpUtility.UrlEncode(Convert.ToBase64String(mStream.ToArray()), System.Text.Encoding.UTF8);
            }
            catch
            {
                return EncryptString;
            }
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="DecryptString"></param>
        /// <param name="Key"></param>
        /// <param name="RGBIV"></param>
        /// <returns></returns>
        public static string Decode(string DecryptString, string Key= "40BE0AD45AE894397FC89F7E2D82662C")
        {
            try
            {
                //进行解码
                DecryptString = HttpUtility.UrlDecode(DecryptString, System.Text.Encoding.UTF8);
                string RGBIV = Key.Substring(0, 24);
                string SecretKey = Key.Substring(24, 8);
                byte[] rgbKey = Encoding.UTF8.GetBytes(SecretKey);
                byte[] rgbIV = Encoding.UTF8.GetBytes(RGBIV);
                byte[] inputByteArray = Convert.FromBase64String(DecryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                DCSP.Mode = CipherMode.CBC;
                DCSP.Padding = PaddingMode.PKCS7;
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return DecryptString;
            }
        }
    }
}
