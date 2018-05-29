using System;
using System.Text;
using System.Security.Cryptography;

namespace Lplfw.DAL
{

    public partial class User
    {
        /// <summary>
        /// 加密算法
        /// </summary>
        /// <param name="express">待加密字符串</param>
        /// <returns></returns>
        static public string Encryption(string express)
        {
            CspParameters param = new CspParameters();
            param.KeyContainerName = "KING_Secrect";
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param))
            {
                byte[] plaindata = Encoding.Default.GetBytes(express);
                byte[] encryptdata = rsa.Encrypt(plaindata, false);
                return Convert.ToBase64String(encryptdata);
            }
        }

        /// <summary>
        /// 解密算法
        /// </summary>
        /// <param name="ciphertext">待解密字符串</param>
        /// <returns></returns>
        static public string Decrypt(string ciphertext)
        {
            CspParameters param = new CspParameters();
            param.KeyContainerName = "KING_Secrect";
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param))
            {
                byte[] encryptdata = Convert.FromBase64String(ciphertext);
                byte[] decryptdata = rsa.Decrypt(encryptdata, false);
                return Encoding.Default.GetString(decryptdata);
            }
        }
    }
}
