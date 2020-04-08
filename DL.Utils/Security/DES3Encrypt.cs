using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DL.Utils.Security
{
    /// <summary>
    ///3DES加密解密程序
    /// --备用
    /// </summary>
    public class DES3Encrypt
    {

        //密钥
        private static string sKey = "qJzGEh6hESZDVJeCnFPGuxzaiFYTLQM3";
        //矢量，矢量可以为空
        private static string sIV = "qcDY6X+aPLw=";

        //构造一个对称算法
        private static SymmetricAlgorithm mCSP = new TripleDESCryptoServiceProvider();

        public DES3Encrypt() { }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Value">明文</param>
        /// <returns>加密后的密文</returns>
        public static string EncryptString(string Value)
        {
            try
            {

                ICryptoTransform ct;
                MemoryStream ms;
                CryptoStream cs;
                byte[] byt;

                mCSP.Key = Convert.FromBase64String(sKey);
                mCSP.IV = Convert.FromBase64String(sIV);

                //指定加密的运算模式
                mCSP.Mode = System.Security.Cryptography.CipherMode.ECB;

                //获取或设置加密算法的填充模式
                mCSP.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

                ct = mCSP.CreateEncryptor(mCSP.Key, mCSP.IV);
                byt = Encoding.UTF8.GetBytes(Value + "_0212YUAN");
                ms = new MemoryStream();
                cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
                cs.Write(byt, 0, byt.Length);
                cs.FlushFinalBlock();
                cs.Close();

                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                return Value;
            }

        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string DecryptString(string Value)
        {
            try
            {

                ICryptoTransform ct;
                MemoryStream ms;
                CryptoStream cs;
                byte[] byt;

                mCSP.Key = Convert.FromBase64String(sKey);
                mCSP.IV = Convert.FromBase64String(sIV);
                mCSP.Mode = System.Security.Cryptography.CipherMode.ECB;
                mCSP.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
                ct = mCSP.CreateDecryptor(mCSP.Key, mCSP.IV);
                byt = Convert.FromBase64String(Value);
                ms = new MemoryStream();
                cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
                cs.Write(byt, 0, byt.Length);
                cs.FlushFinalBlock();
                cs.Close();

                return Encoding.UTF8.GetString(ms.ToArray()).Remove(Encoding.UTF8.GetString(ms.ToArray()).Length - 9, 9);
            }
            catch (Exception ex)
            {
                return Value;
            }
        }

    }
}
