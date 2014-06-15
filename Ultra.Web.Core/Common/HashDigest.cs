using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Ultra.Web.Core.Common
{
    [Serializable]
    public sealed class HashDigest
    {
        public static byte[] FileDigest(string filePath)
        {
            return FileDigest(filePath, DigestType.MD5);
        }

        public static byte[] FileDigest(string filePath, DigestType dgType)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return null;
            }
            Stream inputStream = File.OpenRead(filePath);
            byte[] buffer = GetHashAlgorithmByType(dgType).ComputeHash(inputStream);
            inputStream.Close();
            return buffer;
        }

        public static HashAlgorithm GetHashAlgorithmByType(DigestType dgType)
        {
            switch (dgType)
            {
                case DigestType.MD5:
                    return MD5.Create();

                case DigestType.SHA1:
                    return SHA1.Create();

                case DigestType.SHA256:
                    return SHA256.Create();

                case DigestType.SHA384:
                    return SHA384.Create();

                case DigestType.SHA512:
                    return SHA512.Create();

                case DigestType.RIPEMD160:
                    return RIPEMD160.Create();
            }
            return null;
        }

        public static byte[] StringDigest(string strSrc)
        {
            return StringDigest(strSrc, DigestType.MD5);
        }

        public static byte[] StringDigest(string strSrc, DigestType dgType)
        {
            byte[] bytes = Encoding.Default.GetBytes(strSrc);
            return GetHashAlgorithmByType(dgType).ComputeHash(bytes);
        }
    }

    public enum DigestType : short {
        MD5 = 0,
        RIPEMD160 = 5,
        SHA1 = 1,
        SHA256 = 2,
        SHA384 = 3,
        SHA512 = 4
    }
}

