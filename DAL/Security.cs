using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace DAL
{
    /// <summary>
    /// Security 的摘要描述
    /// </summary>
    public class Security
    {
        private string _Key;
        private string _IV;

        //// <summary> 
        /// 加密金鑰(8個英文字) 
        /// </summary> 
        public string Key
        {
            set
            {
                _Key = value.Length == 8 ? value : "ANEACKey";
            }
        }

        //// <summary> 
        /// 初始化向量(8個英文字) 
        /// </summary> 
        public string IV
        {
            set
            {
                _IV = value.Length == 8 ? value : "ANEACInf";
            }
        }

        //// <summary> 
        /// 初始化 Clib.Security 類別的新執行個體 
        /// </summary> 
        public Security()
        {
            _Key = "ANEACKey";
            _IV = "ANEACInf";
        }

        //// <summary> 
        /// 初始化 Clib.Security 類別的新執行個體 
        /// </summary> 
        /// <param name="newKey">加密金鑰</param> 
        /// <param name="newIV">初始化向量</param> 
        public Security(string newKey, string newIV)
        {
            this.Key = newKey;
            this.IV = newIV;
        }


        /// <!--加密字串--> 
        //// <summary> 
        /// 加密字串 - design By Phoenix 2008 - 
        /// </summary> 
        /// <param name="value">加密的字串</param> 
        /// <returns>加密過後的字串</returns> 
        public string Encrypt(string value)
        {
            return Encrypt(value, _Key, _IV);
        }

        /// <!--解密字串--> 
        //// <summary> 
        /// 解密字串 - design By Phoenix 2008 - 
        /// </summary> 
        /// <param name="value">解密的字串</param> 
        /// <returns>解密過後的字串</returns> 
        public string Decrypt(string value)
        {
            return Decrypt(value, _Key, _IV);
        }

        /// <!--DEC 加密法 --> 
        //// <summary> 
        /// DEC 加密法 - design By Phoenix 2008 - 
        /// </summary> 
        /// <param name="pToEncrypt">加密的字串</param> 
        /// <param name="sKey">加密金鑰</param> 
        /// <param name="sIV">初始化向量</param> 
        /// <returns></returns> 
        private string Encrypt(string pToEncrypt, string sKey, string sIV)
        {
            StringBuilder ret = new StringBuilder();
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                //將字元轉換為Byte 
                byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
                //設定加密金鑰(轉為Byte) 
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                //設定初始化向量(轉為Byte) 
                des.IV = ASCIIEncoding.ASCII.GetBytes(sIV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                    }
                    //輸出資料 
                    foreach (byte b in ms.ToArray())
                        ret.AppendFormat("{0:X2}", b);
                }
            }
            //回傳 
            return ret.ToString();
        }

        /// <!--DEC 解密法--> 
        /// <summary> 
        /// DEC 解密法 - design By Phoenix 2008 - 
        /// </summary> 
        /// <param name="pToDecrypt">解密的字串</param> 
        /// <param name="sKey">加密金鑰</param> 
        /// <param name="sIV">初始化向量</param> 
        /// <returns></returns> 
        private string Decrypt(string pToDecrypt, string sKey, string sIV)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {

                byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
                //反轉 
                for (int x = 0; x < pToDecrypt.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }
                //設定加密金鑰(轉為Byte) 
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                //設定初始化向量(轉為Byte) 
                des.IV = ASCIIEncoding.ASCII.GetBytes(sIV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        //例外處理 
                        try
                        {
                            cs.Write(inputByteArray, 0, inputByteArray.Length);
                            cs.FlushFinalBlock();
                            //輸出資料 
                            return System.Text.Encoding.Default.GetString(ms.ToArray());
                        }
                        catch (CryptographicException)
                        {
                            //若金鑰或向量錯誤，傳回N/A 
                            return "N/A";
                        }
                    }
                }
            }
        }

        /// <!--驗證加密字串--> 
        /// <summary> 
        /// 驗證加密字串 - design By Phoenix 2008 - 
        /// </summary> 
        /// <param name="EnString">加密後的字串</param> 
        /// <param name="FoString">加密前的字串</param> 
        /// <returns>是/否</returns> 
        public bool ValidateString(string EnString, string FoString)
        {
            //呼叫Decrypt解密 
            //判斷是否相符 
            //回傳結果 
            return Decrypt(EnString, _Key, _IV) == FoString.ToString() ? true : false;
        }

    }
}
