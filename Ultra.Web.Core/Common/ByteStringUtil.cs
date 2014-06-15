using System;
using System.Text;

namespace Ultra.Web.Core.Common
{
    [Serializable]
    public class ByteStringUtil
    {
        public static byte[] ByteArrayFromHexStr(string hexStr)
        {
            if (string.IsNullOrEmpty(hexStr))
            {
                return null;
            }
            if ((hexStr.Length % 2) != 0)
            {
                return null;
            }
            if (hexStr.StartsWith("0x") || hexStr.StartsWith("0X"))
            {
                hexStr = hexStr.Substring(2, hexStr.Length - 2);
            }
            byte[] buffer = new byte[hexStr.Length / 2];
            char[] chArray = hexStr.ToCharArray();
            int index = 0;
            for (int i = 0; index < chArray.Length; i++)
            {
                buffer[i] = Convert.ToByte(string.Concat(new object[] { chArray[index], string.Empty, chArray[index + 1], string.Empty }), 0x10);
                index += 2;
            }
            return buffer;
        }

        public static string ByteArrayToHexStr(byte[] bytdata)
        {
            if ((bytdata == null) || (bytdata.Length < 1))
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder(0x100);
            foreach (byte num in bytdata)
            {
                builder.Append(string.Format("{0:X2}", num));
            }
            return builder.ToString();
        }
    }
}

