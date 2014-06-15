using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using Ultra.Web.Core.Configuration;
using Ultra.Web.Core.Interface;

namespace RDSClr {
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
            var tskid = Clear();
            if (tskid > 0) {
                GetTaskStatus(tskid);
            } else {
                AppLog.DebugException(new Exception("Clear Task Fail."));
            }
        }
        static string appkey = string.Empty;
        static string appsecret = string.Empty;
        static string sessionkey = string.Empty;
        static string url = "http://gw.api.taobao.com/router/rest";

        static long Clear() {
            IOptionConfig cfg = new OptionConfig(EnOptionConfigType.App);
            appkey = cfg.Get<string>("appkey");
            if (string.IsNullOrEmpty(appkey)) {
                cfg.Set<string>("appkey", "12517536");
                appkey = cfg.Get<string>("appkey");
            }
            appsecret = cfg.Get<string>("appsecret");
            if (string.IsNullOrEmpty(appsecret)) {
                cfg.Set<string>("appsecret", "d86d510e5a9dfab841e00ff7e7f63e4f");
                appsecret = cfg.Get<string>("appsecret");
            }
            //sessionkey = cfg.Get<string>("sessionkey");
            //if (string.IsNullOrEmpty(sessionkey))
            //{
            //    cfg.Set<string>("sessionkey", "610081760d1be039e230a41061de2a3bae98a9a4112df92143584903");
            //    sessionkey = cfg.Get<string>("sessionkey");
            //}

            ITopClient client = new DefaultTopClient(url, appkey, appsecret);
            TopatsJushitaJdpDatadeleteRequest req = new TopatsJushitaJdpDatadeleteRequest();
            req.SyncType = "tb_trade;tb_item;tb_refund;fx_trade;tm_refund";
            req.StartModified = DateTime.Now.AddDays(-10);
            DateTime end = DateTime.Now.AddDays(-5);
            req.EndModified = end;
            try {
                TopatsJushitaJdpDatadeleteResponse response = client.Execute(req);
                if (response.IsError) {
                    AppLog.DebugException(new Exception(string.Format("ErrCode:{0} ErrMsg:{1} SubErrCode:{2} SubErrMsg:{3}", response.ErrCode,
                        response.ErrMsg, response.SubErrCode, response.SubErrMsg)));
                    return -1L;
                } else
                    return response.Task.TaskId;
            } catch (Exception ex) {
                AppLog.DebugException(ex);
                return -1L;
            }
        }

        static void GetTaskStatus(long tskId) {
            CreateDeferExit();
            Thread.Sleep(10000);
            ITopClient client = new DefaultTopClient(url, appkey, appsecret);
            TopatsResultGetRequest req = new TopatsResultGetRequest();
            req.TaskId = tskId;// 179484794L;
            try {
            __RETRY:
                TopatsResultGetResponse response = client.Execute(req);
                if (response.IsError) {
                    AppLog.DebugException(new Exception(string.Format("ErrCode:{0} ErrMsg:{1} SubErrCode:{2} SubErrMsg:{3}", response.ErrCode,
                       response.ErrMsg, response.SubErrCode, response.SubErrMsg)));
                } else {
                    AppLog.DebugException(new Exception(string.Format("Task:{0} Status:{1}", tskId, response.Task.Status)));
                    if (string.Compare("done", response.Task.Status, true) == 0)
                        Environment.Exit(0);
                    else {
                        Thread.Sleep(60000);
                        goto __RETRY;
                    }
                }
            } catch (Exception ex) {
                AppLog.DebugException(ex);
                AppLog.DebugException(new Exception("GetTaskStatus Fail Force Exit."));
                Environment.Exit(0);
            }
        }

        static void CreateDeferExit() {
            var t = new Thread(() => {
                Thread.Sleep(5 * 60 * 1000);
                AppLog.DebugException(new Exception("Force Exit."));
                Environment.Exit(0);
            });
            t.IsBackground = true;
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e) {
            AppLog.DebugException(e.Exception);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
            AppLog.DebugException(e.ExceptionObject as Exception);
        }
    }
}
