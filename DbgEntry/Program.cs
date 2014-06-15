using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ultra.Surface.Lanuch;
using Ultra.Web.Core.Common;
using Ultra.Web.Core.Cache;
using DbEntity;

namespace DbgEntry
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main( string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (null == args || args.Length < 1)
                return;
            //Path.GetDirectoryName(Application.StartupPath);   
            var vm = Lanucher.Start(args[0]);
            var date = (DateTime)SqlHelper.ExecuteScalar(Lanucher.ConnectonString, System.Data.CommandType.Text, "select getdate()");
            TimeSync.Default.StartSync(date);
            vm.Cacher.Put<t_user>("CurrentUser", new t_user() {
                UserName = "admin",
                Guid = Guid.NewGuid()
            });
            vm.Cacher.Put<string>("CurUser", "admin");
            Application.Run(vm);

        }
    }
}
