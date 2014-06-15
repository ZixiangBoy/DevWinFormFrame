using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Ultra.Web.Core.Common;
using Ultra.Web.Core.Configuration;
using Ultra.Web.Core.Interface;

namespace RDS3 {
    static class Program {
        static Ultra.Log.ApplicationLog AppLog;

        private static string CurDir {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory); }
        }

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main() {
            AppLog = new Ultra.Log.ApplicationLog();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            int sec = 0; bool trc; string filepath;
            var conn = Init(out sec, out trc, out filepath);

            RunToDb r = new RunToDb(conn, AppLog, filepath, sec);
            r.Trace = trc;
            r.StartSync();
            AppLog.DebugException(new Exception("Sync Start"));
            var t = new Thread(() => {
                while (true) {
                    GC.Collect();
                    Thread.Sleep(5000);
                }
            });
            t.IsBackground = true;
            t.Start();
            t.Join();
        }


        static string Init(out int sec, out bool trc, out string filepath) {
            IOptionConfig cfg = new OptionConfig(EnOptionConfigType.App);
            var conn = cfg.Get<string>("conn");
            sec = cfg.Get<int>("sec");
            trc = cfg.Get<bool>("Trace");
            filepath = cfg.Get<string>("filepath");
            if (sec <= 0) {
                cfg.Set<int>("sec", 180000);
                sec = cfg.Get<int>("sec");
            }
            if (string.IsNullOrEmpty(conn)) {
                cfg.Set<string>("conn", "conn1804c7s7.sqlserver.rds.aliyuncs.com,3433;uid=usr0804c7s7;pwd=ABcd6634539;database=erp_power");
                conn = cfg.Get<string>("conn");
            }
            return conn;
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e) {
            AppLog.DebugException(e.Exception);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
            AppLog.DebugException(e.ExceptionObject as Exception);
        }
    }
}
