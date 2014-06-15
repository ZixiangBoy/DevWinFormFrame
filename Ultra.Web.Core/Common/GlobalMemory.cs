using System;
using System.Runtime.InteropServices;

namespace Ultra.Web.Core.Common
{
    public class GlobalMemory
    {
        private const int ERROR_ALREADY_EXISTS = 0xb7;
        private const int FILE_MAP_ALL_ACCESS = 6;
        private const int FILE_MAP_COPY = 1;
        private const int FILE_MAP_READ = 4;
        private const int FILE_MAP_WRITE = 2;
        private const int INVALID_HANDLE_VALUE = -1;
        private bool m_bAlreadyExist;
        private bool m_bInit;
        private IntPtr m_hSharedMemoryFile = IntPtr.Zero;
        private long m_MemSize;
        private IntPtr m_pwData = IntPtr.Zero;
        private const int PAGE_EXECUTE = 0x10;
        private const int PAGE_EXECUTE_READ = 0x20;
        private const int PAGE_EXECUTE_READWRITE = 0x40;
        private const int PAGE_READONLY = 2;
        private const int PAGE_READWRITE = 4;
        private const int PAGE_WRITECOPY = 8;
        private const int SEC_COMMIT = 0x8000000;
        private const int SEC_IMAGE = 0x1000000;
        private const int SEC_NOCACHE = 0x10000000;
        private const int SEC_RESERVE = 0x4000000;

        public void Close()
        {
            if (this.m_bInit)
            {
                UnmapViewOfFile(this.m_pwData);
                CloseHandle(this.m_hSharedMemoryFile);
            }
        }

        [DllImport("Kernel32.dll", CharSet=CharSet.Auto)]
        public static extern bool CloseHandle(IntPtr handle);
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr CreateFileMapping(int hFile, IntPtr lpAttributes, uint flProtect, uint dwMaxSizeHi, uint dwMaxSizeLow, string lpName);
        ~GlobalMemory()
        {
            this.Close();
        }

        [DllImport("kernel32")]
        public static extern int GetLastError();
        public int Init(string strName, long lngSize)
        {
            if ((lngSize <= 0L) || (lngSize > 0x800000L))
            {
                lngSize = 0x800000L;
            }
            this.m_MemSize = lngSize;
            if (strName.Length <= 0)
            {
                return 1;
            }
            this.m_hSharedMemoryFile = CreateFileMapping(-1, IntPtr.Zero, 4, 0, (uint) lngSize, strName);
            if (this.m_hSharedMemoryFile == IntPtr.Zero)
            {
                this.m_bAlreadyExist = false;
                this.m_bInit = false;
                return 2;
            }
            if (GetLastError() == 0xb7)
            {
                this.m_bAlreadyExist = true;
            }
            else
            {
                this.m_bAlreadyExist = false;
            }
            this.m_pwData = MapViewOfFile(this.m_hSharedMemoryFile, 6, 0, 0, (uint) lngSize);
            if (this.m_pwData == IntPtr.Zero)
            {
                this.m_bInit = false;
                CloseHandle(this.m_hSharedMemoryFile);
                return 3;
            }
            this.m_bInit = true;
            if (!this.m_bAlreadyExist)
            {
            }
            return 0;
        }

        [DllImport("Kernel32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr MapViewOfFile(IntPtr hFileMapping, uint dwDesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow, uint dwNumberOfBytesToMap);
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr OpenFileMapping(int dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, string lpName);
        public int Read(ref byte[] bytData, int lngAddr, int lngSize)
        {
            if ((lngAddr + lngSize) > this.m_MemSize)
            {
                return 2;
            }
            if (this.m_bInit)
            {
                Marshal.Copy(this.m_pwData, bytData, lngAddr, lngSize);
                return 0;
            }
            return 1;
        }

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto)]
        public static extern bool UnmapViewOfFile(IntPtr pvBaseAddress);
        public int Write(byte[] bytData, int lngAddr, int lngSize)
        {
            if ((lngAddr + lngSize) > this.m_MemSize)
            {
                return 2;
            }
            if (this.m_bInit)
            {
                Marshal.Copy(bytData, lngAddr, this.m_pwData, (lngSize > bytData.Length) ? bytData.Length : lngSize);
                return 0;
            }
            return 1;
        }
    }
}

