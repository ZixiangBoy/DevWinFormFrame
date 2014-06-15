using System;
using System.Runtime.InteropServices;

namespace Ultra.Web.Core.Common
{
    public static class FileMoveEx
    {
        [DllImport("kernel32.dll", CharSet=CharSet.Unicode, SetLastError=true)]
        public static extern bool MoveFileEx(string lpExistingFileName, string lpNewFileName, MoveFileFlags dwFlags);
    }
}

