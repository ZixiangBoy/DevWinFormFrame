using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Ultra.Web.Core.Common
{
    public static class SystemInvoke
    {
        public static void OpenFile(string fileName)
        {
            ShellExecute(IntPtr.Zero, "open", fileName, string.Empty, string.Empty, ShowCommands.SW_SHOWNORMAL);
        }

        public static void SendPrtSc()
        {
            SendKeys.Send("{PrtSc}");
        }

        public static void SendWindowCloseKey()
        {
            SendKeys.Send("%{F4}");
        }

        [DllImport("shell32.dll ")]
        public static extern IntPtr ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, ShowCommands nShowCmd);
        public static Process StartProcess(string procFileName, string args , EventHandler exitHandler)
        {
            Process process = new Process {
                StartInfo = { FileName = procFileName }
            };
            if (!string.IsNullOrEmpty(args))
            {
                process.StartInfo.Arguments = args;
            }
            process.EnableRaisingEvents = true;
            if (exitHandler != null)
            {
                process.Exited += exitHandler;
            }
            return process;
        }

        public enum ShowCommands
        {
            SW_FORCEMINIMIZE = 11,
            SW_HIDE = 0,
            SW_MAX = 11,
            SW_MAXIMIZE = 3,
            SW_MINIMIZE = 6,
            SW_NORMAL = 1,
            SW_RESTORE = 9,
            SW_SHOW = 5,
            SW_SHOWDEFAULT = 10,
            SW_SHOWMAXIMIZED = 3,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOWNORMAL = 1
        }
    }
}

