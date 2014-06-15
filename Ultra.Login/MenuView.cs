using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using Ultra.Surface.Common;
using Ultra.Surface.Form;
using Ultra.Web.Core.Common;
using DbEntity;
using PetaPoco;

namespace Ultra.Login
{
    public partial class MenuView : BaseSurface
    {
        public MenuView()
        {
            InitializeComponent();
        }

        private void treeCtl1_MouseUp(object sender, MouseEventArgs e)
        {
            //var nd = treeCtl1.FocusedNode;
            //if (null != nd) {
            //    barbtnMnuItem.Enabled = nd.ParentNode == null;
            //}
            try
            {
                Point pt = treeCtl1.PointToClient(MousePosition);
                TreeListHitInfo info = treeCtl1.CalcHitInfo(pt);
                if (info.HitInfoType == HitInfoType.Cell)
                    treeCtl1.FocusedNode = info.Node;
            }
            finally { }
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                this.popMenuCtl1.ShowPopup(Control.MousePosition);
        }

        private void barbtnDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.treeCtl1.FocusedNode == null) return;
            if (MsgBox.ShowYesNoMessage(null, "确定要删除吗?") == System.Windows.Forms.DialogResult.No)
                return;
            treeCtl1.Nodes.Remove(treeCtl1.FocusedNode);
        }

        private void barbtnMnuGrp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var md = new MenuData
            {
                IsUsing = true,
                MenuGrpName = "新建菜单组",
                MenuItem = null
            };
            var nd = this.treeCtl1.Nodes.Add(new object[] { "新建菜单组", md });
            nd.Tag = md;
            this.treeCtl1.FocusedNode = nd;
        }

        private void barbtnMnuItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var nd = this.treeCtl1.FocusedNode;
            var pnd = nd.ParentNode == null ? nd : nd.ParentNode;
            var md = new MenuData
            {
                IsUsing = true,
                MenuGrpName = pnd.GetValue(0).ToString(),
                MenuItem = null
            };
            var md2 = new MenuData
            {
                IsUsing = true,
                MenuGrpName = pnd.GetValue(0).ToString(),
                MenuItem = new MenuItemData
                {
                    IsUsing = true,
                    MenuName = "新建菜单项",
                    MenuClsName = string.Empty,
                    MenuAsmName = string.Empty
                }
            };
            var tnd = pnd.Nodes.Add(new object[] { "新建菜单项", md2 });
            tnd.Tag = md2;
            this.treeCtl1.FocusedNode = tnd;
        }

        private void treeCtl1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            if (e.Node == null) return;
            var pnd = e.Node.ParentNode;
            btnSaveMnuItem.Enabled = pnd != null;
            btnSaveMnuGrp.Enabled = pnd == null;
            txtmnuGrp.Text = pnd == null ? e.Node.GetValue(0).ToString() : pnd.GetValue(0).ToString();

            var tg = e.Node.Tag as MenuData;
            if (pnd != null)//菜单项
            {
                txtmnuName.Text = e.Node.GetValue(0).ToString();

                if (null != tg)
                {
                    txtmnuAsmName.Text = tg.MenuItem.MenuAsmName;
                    txtmnuClassName.Text = tg.MenuItem.MenuClsName;
                    chkUsing.Checked = tg.MenuItem.IsUsing;
                }
            }
            else
            {

                txtmnuName.Text = string.Empty;
                txtmnuAsmName.Text = string.Empty;
                txtmnuClassName.Text = string.Empty;
                chkUsing.Checked = null == tg ? chkUsing.Checked : tg.IsUsing;
            }
        }

        private void btnSaveMnuGrp_Click(object sender, EventArgs e)
        {
            var nd = treeCtl1.FocusedNode;
            if (nd.ParentNode != null) return;
            var tg = nd.Tag as MenuData;
            tg = tg == null ? new MenuData
            {
                MenuItem = null,
                IsUsing = true,
                MenuGrpName = nd.GetValue(0).ToString()
            } : tg;
            tg.MenuGrpName = txtmnuGrp.Text;
            nd.Tag = tg;
            nd.SetValue(0, txtmnuGrp.Text);
        }

        private void btnSaveMnuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtmnuName.Text))
            {
                return;
            }
            var nd = treeCtl1.FocusedNode;
            if (nd.ParentNode == null) return;
            var tg = nd.Tag as MenuData;
            tg = tg == null ? new MenuData
            {
                MenuItem = new MenuItemData
                {
                    IsUsing = true
                },
                IsUsing = true,
                MenuGrpName = nd.ParentNode.GetValue(0).ToString()
            } : tg;
            tg.MenuItem.MenuClsName = txtmnuClassName.Text;
            tg.MenuItem.MenuAsmName = txtmnuAsmName.Text;
            tg.MenuItem.MenuName = txtmnuName.Text;
            tg.MenuItem.IsUsing = chkUsing.Checked;
            nd.SetValue(0, txtmnuName.Text);
            nd.Tag = tg;
        }

        private void MenuView_Load(object sender, EventArgs e)
        {
            t_menu mnu;
            using (var db = new Database()) {
                mnu = db.FirstOrDefault<t_menu>("select * from t_menu");
                if (null == mnu || string.IsNullOrEmpty(mnu.MenuXml))
                {
                    treeCtl1.ClearNodes();
                    mnu = new t_menu
                    {
                        MenuXml = BuildMenuXml(),
                        Version = "1.0"
                    };
                }
            }
            //解析菜单XML
            treeCtl1.ClearNodes();
            ExtractMenu(mnu.MenuXml);
        }

        private void ExtractMenu(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return;
            XDocument xdoc = XDocument.Parse(xml);
            foreach (XElement item in xdoc.Element("Menus").Elements())
            {
                var md = new MenuData
                {
                    IsUsing = true,
                    MenuGrpName = item.Attribute("Name").Value,
                    MenuItem = null
                };
                var td = treeCtl1.Nodes.Add(new object[] { item.Attribute("Name").Value, md });
                td.Tag = md;
                foreach (XElement xd in item.Elements())
                {
                    var mi = new MenuData
                    {
                        IsUsing = true,
                        MenuGrpName = md.MenuGrpName,
                        MenuItem = new MenuItemData
                        {
                            MenuAsmName = xd.Attribute("AsmName").Value,
                            MenuClsName = xd.Attribute("ClsName").Value,
                            MenuName = xd.Attribute("Name").Value,
                            IsUsing = string.Compare(true.ToString(), xd.Attribute("IsUsing").Value) == 0
                        }
                    };
                    var tsd = td.Nodes.Add(new object[] { 
                        mi.MenuItem.MenuName,mi
                    });
                    tsd.Tag = mi;
                }
            }
        }

        private string BuildMenuXml()
        {
            XDocument xdoc = new XDocument(
                new XElement("Menus")
                );
            if (treeCtl1.Nodes.Count < 1)
                return xdoc.ToString();
            foreach (TreeListNode xd in treeCtl1.Nodes)
            {
                var ss = GetMenuGrp(xd);
                if (null != ss)
                    xdoc.Element("Menus").Add(ss);
            }
            return xdoc.ToString();
        }

        private XElement GetMenuGrp(TreeListNode nd)
        {
            var xe = new XElement("MenuGrp", new XAttribute("Name", nd.GetValue(0).ToString()));
            var ss = GetMenuItem(nd);
            if (null != ss)
                xe.Add(ss.ToArray());
            return xe;
        }

        private List<XElement> GetMenuItem(TreeListNode nd)
        {
            if (nd.Nodes == null || nd.Nodes.Count < 1) return null;
            List<XElement> xels = new List<XElement>(nd.Nodes.Count);
            foreach (TreeListNode xd in nd.Nodes)
            {
                MenuData tg = xd.Tag as MenuData;
                if (tg == null) continue;
                xels.Add(new XElement("MenuItem",
                    new XAttribute("Name", tg.MenuItem.MenuName),
                    new XAttribute("ClsName", tg.MenuItem.MenuClsName),
                    new XAttribute("AsmName", tg.MenuItem.MenuAsmName),
                    new XAttribute("IsUsing", tg.MenuItem.IsUsing.ToString())
                    ));
            }
            return xels;
        }

        private void btnSaveToSvr_Click(object sender, EventArgs e)
        {
            if (MsgBox.ShowYesNoMessage("", "确定要保存菜单至服务器?") == System.Windows.Forms.DialogResult.No)
                return;
            using (var db = new Database()) {
                var mnu = db.FirstOrDefault<t_menu>("select * from t_menu");
                if (mnu == null)
                {
                    mnu = new t_menu
                    {
                        Guid = Guid.NewGuid(),
                        Version = "1.0",
                        MenuXml = BuildMenuXml(),
                        CreateDate = TimeSync.Default.CurrentSyncTime
                    };
                }
                else {
                    mnu.MenuXml = BuildMenuXml();
                }
                db.Save(mnu);
            }
            MsgBox.ShowMessage("", "保存成功!");
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MsgBox.ShowYesNoMessage(null, "重新加载菜单，有可能丢失当前的更改，是否继续?") ==
                 System.Windows.Forms.DialogResult.No) return;
            t_menu mnu;
            using (var db = new Database())
            {
                mnu = db.Fetch<t_menu>("select * from t_menu").FirstOrDefault(k => k.Version == "1.0");
                //解析菜单XML
                treeCtl1.ClearNodes();
                ExtractMenu(mnu.MenuXml);
            }
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (fdlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                File.WriteAllText(fdlg.FileName, BuildMenuXml());
                if (MsgBox.ShowYesNoMessage(null, "导出成功，是否立即打开?") == System.Windows.Forms.DialogResult.Yes)
                {
                    SystemInvoke.OpenFile(fdlg.FileName);
                }
            }
        }
    }
}
