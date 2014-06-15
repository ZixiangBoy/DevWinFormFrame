using System;
using System.Security.Cryptography;

namespace Ultra.Web.Core.Common
{
    [Serializable]
    public class RSAEncrypt
    {
        public byte[] Decrypt(byte[] srcData, string keyPri)
        {
            if (((srcData == null) || string.IsNullOrEmpty(keyPri)) || (srcData.Length < 1))
            {
                return null;
            }
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider(0x400);
            provider.FromXmlString(keyPri);
            return provider.Decrypt(srcData, false);
        }

        public byte[] Encrypt(byte[] srcData, string keyPub)
        {
            if (((srcData == null) || string.IsNullOrEmpty(keyPub)) || (srcData.Length < 1))
            {
                return null;
            }
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider(0x400);
            provider.FromXmlString(keyPub);
            return provider.Encrypt(srcData, false);
        }
    }
}

