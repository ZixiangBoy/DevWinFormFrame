using DbEntity;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ultra.Surface.Common;
using Ultra.Surface.Form;
using Ultra.Web.Core.Common;

namespace Ultra.Login
{
    public partial class LoginView : BaseSurface
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private string LocalIP = string.Empty;
        private string RemoteIP = string.Empty;
        private string LocalMAC = string.Empty;
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!dxValidationProvider1.Validate())
                return;
            Common.GetMACs(out LocalMAC);
            SqlHelper.ExecuteNonQuery(ConnString, CommandType.Text, "delete t_forcedoffline where LoginMAC=@LoginMAC", new SqlParameter("@LoginMAC", LocalMAC));
            Common.GetLocalIpv4(out LocalIP);
            //try { RemoteIP = Common.GetRemoteIP(); }
            //catch { RemoteIP = string.Empty; }
            RemoteIP = string.Empty; 

            var pwd = Util.EncryptPwd(txtpwd.Text);
            using (var db = new Database())
            {
                db.Execute(Sql_AddDef);

                var kt = db.FirstOrDefault<t_user>("select * from t_user where username=@0 and pwd=@1 and IsUsing=1", txtAct.Text.Trim(), pwd);
                if (null == kt)
                {
                    MsgBox.ShowMessage("登录无效", "无效的登录名或密码不正确");
                    return;
                }
                DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Cacher.Put<t_user>("CurrentUser", kt);
                this.Cacher.Put<string>("CurUser", kt.UserName);
                if (!"admin".EqualIgnorCase(kt.UserName))
                {
                    //读取权限信息
                    var rst = db.Fetch<t_roleset>("select * from t_roleset where RoleId in (select RoleId from t_roleuser where userId=@0)", kt.Id);
                    var rle = db.Fetch<t_role>("select * from t_role where IsUsing=1");
                    rst = rst.Where(k => rle.Any(j => j.Id == k.RoleId)).ToList();
                    var rlet = rst.ToList();
                    List<MenuCtlData> mcd = null;
                    if (rlet.Count > 0)
                    {
                        mcd = new List<MenuCtlData>(rlet.Count);

                        rlet.ForEach(k =>
                        {
                            var ks = (Ultra.Web.Core.Common.ObjectHelper.DeSerialize<List<MenuCtlData>>(k.RoleSetTree));
                            ks = ks.Where(j => j.IsEnabled).ToList();
                            ks.ForEach(j =>
                            {
                                if (!mcd.Exists(m => m.ClsName == j.ClsName && m.ModName == j.ModName && j.CtlType == m.CtlType
                                    && j.ControlName == m.ControlName))
                                    mcd.Add(j);
                            });
                        });
                    }
                    this.Cacher.Put<List<MenuCtlData>>("Permission", mcd);
                }
            }
            this.OptConfig.Set<string>("LastLogin", txtAct.Text.Trim());
            Close();
        }

        private bool IsAllowLogin()
        {
            using (var db = new Database())
            {
                var macs = db.Fetch<t_mac>("select * from t_mac");
                var c = macs.Count();
                if (c < 1)
                    return true;
                var ks = macs.Where(k => string.Compare(k.Mac, LocalMAC, true) == 0).ToList();
                return ks != null && ks.Count > 0;
            }
        }

        private string Sql_AddLoginFail
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("update t_loginfail                                                    \n");
                sb.Append("	set                                                   \n");
                sb.Append("	ExternalIP=@ExternalIP                                                 \n");
                sb.Append("	,LocalIP=@LocalIP                                                       \n");
                sb.Append("	,LastUpdateTime=getdate()                                              \n");
                sb.Append("	where UserName=@UserName and LoginMAC=@LoginMAC;                                                \n");
                sb.Append("	insert t_loginfail(Creator,Guid,UserName,LoginMAC,ExternalIP,LocalIP)        \n");
                sb.Append("	select  top 1 'SYS',newid(),@UserName,@LoginMAC,@ExternalIP,@LocalIP           \n");
                sb.Append("	from t_user where not exists(select 1 from t_loginfail where UserName=@UserName and LoginMAC=@LoginMAC) \n");
                return sb.ToString();
            }
        }

        private string Sql_AddDef
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("insert t_user(guid,username,Pwd)                                           \n");
                sb.Append("	select newid(),'admin','97E5F9C5EAA6EDEE98559C877DCEC1B8'                          \n");
                sb.Append("	from t_user where not exists(select 1 from t_user where username='admin') \n");
                return sb.ToString();
            }
        }

        private void LoginView_Load(object sender, EventArgs e)
        {
            var lstlgn = this.OptConfig.Get<string>("LastLogin");
            if (!string.IsNullOrEmpty(lstlgn))
                txtAct.Text = lstlgn;
            txtpwd.Focus();
        }

        private void txtpwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnOK_Click(sender, e);
        }

        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);
            if (message.Msg == WM_NCHITTEST && (int)message.Result == HTCLIENT)
            {
                message.Result = (IntPtr)HTCAPTION;
            }
        }
    }
}
