using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;

namespace Ultra.RDS.SyncSvc
{
    [RunInstaller(true)]
    public partial class Installer1 : System.Configuration.Install.Installer
    {
        public Installer1()
        {
            InitializeComponent();
            // 创建ServiceProcessInstaller对象和ServiceInstaller对象
            this.spInstaller =
     new System.ServiceProcess.ServiceProcessInstaller();
            this.sInstaller = new System.ServiceProcess.ServiceInstaller();

            // 设定ServiceProcessInstaller对象的帐号、用户名和密码等信息
            this.spInstaller.Account =
     System.ServiceProcess.ServiceAccount.LocalSystem;
            //this.spInstaller.Username = null;
            //this.spInstaller.Password = null;

            // 设定服务名称
            this.sInstaller.ServiceName = "UltraRDS";

            // 设定服务的启动方式
            this.sInstaller.StartType =
     System.ServiceProcess.ServiceStartMode.Automatic;

            this.Installers.AddRange(
     new System.Configuration.Install.Installer[] { this.spInstaller, this.sInstaller });
        }


        private System.ServiceProcess.ServiceProcessInstaller spInstaller;
        private System.ServiceProcess.ServiceInstaller sInstaller;
    }
}
