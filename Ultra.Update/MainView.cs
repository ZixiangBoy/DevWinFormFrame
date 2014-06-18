using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ultra.Update {
    public partial class MainView : Form {
        ClientConfig clientconfig;
        ServerConfig serverConfig;
        UpdateCore updateCore = new UpdateCore();
        string execPath = Path.GetDirectoryName(Application.ExecutablePath);
        static Ultra.Log.ApplicationLog AppLog = new Ultra.Log.ApplicationLog();

        public MainView() {
            InitializeComponent();
            this.Load += MainView_Load;
        }

        void MainView_Load(object sender, EventArgs e) {
            clientconfig = updateCore.XmlDeserialize<ClientConfig>(Path.Combine(execPath, "clientconfig.xml"), typeof(ClientConfig));
            serverConfig = updateCore.XmlDeserializeWeb<ServerConfig>(clientconfig.ServerUrl, typeof(ServerConfig));
            var pros=Process.GetProcessesByName(clientconfig.ProcessName);
            if (pros.Length > 0) {
                MessageBox.Show("程序正在运行,请先关闭再更新!");
                return;
            }

            try {
                if (!clientconfig.Version.Equals(serverConfig.Version)) {
                    lblmsg.Text = "正在下载更新文件....";
                    updateCore.DownFile(serverConfig.FileUrl, Path.Combine(execPath, clientconfig.ClientFileName));
                    lblmsg.Text = "正在备份文件....";
                    updateCore.BackupTo(execPath, Path.Combine(execPath, clientconfig.BackupDirectory));
                    lblmsg.Text = "正在解压文件....";
                    updateCore.UnZip(Path.Combine(execPath, clientconfig.ClientFileName), execPath);
                    lblmsg.Text = "正在保存更新记录....";
                    clientconfig.Version = serverConfig.Version;
                    updateCore.XmlSerialize<ClientConfig>(Path.Combine(execPath, "clientconfig.xml"), typeof(ClientConfig), clientconfig);
                    lblmsg.Text = "更新完成!";
                    //启动程序
                    Process.Start(clientconfig.ProcessName);
                }
            } catch (Exception ex) {
                AppLog.DebugException(ex);
                throw;
            }
        }

        private void lnklbldownload_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start(serverConfig.FileUrl);
        }
    }
}
