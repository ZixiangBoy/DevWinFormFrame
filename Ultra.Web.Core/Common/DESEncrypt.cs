using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Ultra.Web.Core.Common
{
    [Serializable]
    public class DESEncrypt
    {
        private static byte[] Keys = new byte[] { 0x8e, 0x3d, 0x18, 0xe8, 30, 0xa1, 0x6f, 0x5f };

        public byte[] Decrypt(byte[] srcData, string strKey)
        {
            if ((srcData == null) || (srcData.Length < 1))
            {
                return null;
            }
            byte[] buffer = HashDigest.StringDigest(strKey, DigestType.MD5);
            SymmetricAlgorithm algorithm = SymmetricAlgorithm.Create("3DES");
            algorithm.Key = buffer;
            algorithm.IV = Keys;
            new DESCryptoServiceProvider();
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, algorithm.CreateDecryptor(), CryptoStreamMode.Write);
            stream2.Write(srcData, 0, srcData.Length);
            stream2.FlushFinalBlock();
            stream2.Clear();
            return stream.ToArray();
        }

        public string DecryptString(string srcStr)
        {
            return this.DecryptString(srcStr, this.DefaultKey);
        }

        public string DecryptString(string srcString, string strKey)
        {
            byte[] srcData = ByteStringUtil.ByteArrayFromHexStr(srcString);
            return Encoding.Default.GetString(this.Decrypt(srcData, strKey));
        }

        public byte[] Encrypt(byte[] srcData, string strKey)
        {
            byte[] buffer = HashDigest.StringDigest(strKey, DigestType.MD5);
            SymmetricAlgorithm algorithm = SymmetricAlgorithm.Create("3DES");
            algorithm.Key = buffer;
            algorithm.IV = Keys;
            new DESCryptoServiceProvider();
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, algorithm.CreateEncryptor(), CryptoStreamMode.Write);
            stream2.Write(srcData, 0, srcData.Length);
            stream2.FlushFinalBlock();
            stream2.Clear();
            return stream.ToArray();
        }

        public string EncryptString(string srcStr)
        {
            return this.EncryptString(srcStr, this.DefaultKey);
        }

        public string EncryptString(string srcString, string strKey)
        {
            return ByteStringUtil.ByteArrayToHexStr(this.Encrypt(Encoding.Default.GetBytes(srcString), strKey));
        }

        private string DefaultKey
        {
            get
            {
                return Encoding.Default.GetString(HashDigest.StringDigest("Ultra.Web.Core.Common-Serct-Key", DigestType.MD5));
            }
        }
    }
}

