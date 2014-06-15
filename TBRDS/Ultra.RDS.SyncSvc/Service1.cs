using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace Ultra.RDS.SyncSvc
{
    public partial class Service1 : ServiceBase
    {
        Ultra.Log.ApplicationLog AppLog;
        string RunExe = "RDS3.exe";

        private string CurDir
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory); }
        }

        public Service1()
        {
            InitializeComponent();
            AppLog = new Log.ApplicationLog();
        }

        Thread thDetect = null;
        long KeepAlive = 1;
        Process per = null;

        protected override void OnStart(string[] args)
        {
            var f = Path.Combine(CurDir, RunExe);
            if (!File.Exists(f)) return;
            var pe = per = Process.Start(new ProcessStartInfo
            {
                FileName = f
            });
            CreateDetect(pe);
        }

        void CreateDetect(Process pe)
        {
            per = pe;
          var  tDetect = new Thread(arg =>
            {
                try
                {
                    var p = arg as Process;
                    if (null == p) return;
                    var st = p.StartInfo;
                    var pid = p.Id; var pn = p.ProcessName;
                    AppLog.DebugException(new Exception(string.Format("PN:{1} PID:{0} begin detect.", p.Id, p.ProcessName)));
                    p.WaitForExit();
                    AppLog.DebugException(new Exception(string.Format("PN:{1} PID:{0} exit.", pid, pn)));
                    if (Interlocked.Read(ref KeepAlive) > 0)
                    {
                        p = Process.Start(st);
                        CreateDetect(p);
                        return;
                    }
                    else
                        return;
                }
                catch (ThreadAbortException) { }
                catch (Exception ex)
                {
                    AppLog.DebugException(ex);
                }
            });
          tDetect.IsBackground = true;
          tDetect.SetApartmentState(ApartmentState.STA);
          tDetect.Start(pe);
        }

        protected override void OnStop()
        {
            Interlocked.Exchange(ref KeepAlive, 0);
            per.Kill();
            Thread.Sleep(1000);

            AppLog.DebugException(new Exception("Svc Stop"));
            //if (null != thDetect)
            //{
            //    try
            //    {
            //        while (thDetect.IsAlive)
            //        {
            //            thDetect.Abort();
            //            Thread.Sleep(100);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        AppLog.DebugException(ex);
            //    }
            //}
            var ps = Process.GetProcessesByName(RunExe);
            if (null == ps || ps.Length < 1) return;
            foreach (var p in ps)
            {
                try
                {
                    p.Kill();
                }
                finally { }
            }
            //Thread.Sleep(2000);
            //ps = Process.GetProcessesByName(RunExe);
            //if (null == ps || ps.Length < 1) return;
            //foreach (var p in ps)
            //{
            //    try
            //    {
            //        p.Kill();
            //    }
            //    finally { }
            //}
        }
    }
}
