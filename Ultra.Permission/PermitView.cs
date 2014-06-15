using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using DevExpress.XtraTreeList.Nodes;
using DbEntity;
using Ultra.Surface.Common;
using Ultra.Surface.Form;
using Ultra.Surface.Interfaces;
using Ultra.Surface.Lanuch;
using Ultra.Web.Core.Common;
using PetaPoco;

namespace Ultra.Permission
{
    public partial class PermitView : DialogViewEx
    {
        public PermitView()
        {
            InitializeComponent();
            //Visible = false;
        }
        DevExpress.Utils.WaitDialogForm dlg;
        private void PermitView_Load(object sender, EventArgs e)
        {
            this.btnClose.Left = this.Width - this.btnClose.Width - 30;
            this.btnOK.Left = this.btnClose.Left - this.btnOK.Width - 10;
            roleGridEdit1.LoadData();
            treeCtl1.AfterCheckNode += treeCtl1_AfterCheckNode; treeCtl1.Enabled = false;
            t_menu mnu;
            using (var db = new Database())
            {
                mnu = db.FirstOrDefault<t_menu>("select * from t_menu");
                //解析菜单XML
                treeCtl1.ClearNodes();
                ExtractMenu(mnu.MenuXml);
            }
            dlg = new DevExpress.Utils.WaitDialogForm("正在加载权限设置信息 ...",
                "数据加载中");


            var t = new Thread(() =>
            {
                //加载菜单
                this.Invoke(new Action(() =>
                {
                    BuildlRoleTree(); Visible = true;
                }));
                this.Invoke(new Action(() => dlg.Close()));
            });
            t.SetApartmentState(ApartmentState.STA);
            t.IsBackground = true;
            t.Start();
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

        private void Refc(string cls, string mod, TreeListNode mnu)
        {
            try
            {
                var pth = Path.Combine(Lanucher.AppDir, mod);
                var md5 = string.Empty;
                if (File.Exists(pth))
                {
                    md5 = ByteStringUtil.ByteArrayToHexStr(HashDigest.FileDigest(pth));
                }
                var m = Lanucher.Start(cls, mod);
                var ip = m as ISurfacePermission;
                if (null == ip) return;
                var toolbar = ip.ToolBarItems;
                var grids = ip.Grids;
                var btns = ip.ButtonItems;
                if (null != toolbar && toolbar.Count > 0)
                {
                    var td = mnu.Nodes.Add(new object[] { "主按钮" });
                    foreach (var ti in toolbar)
                    {
                        var tds = td.Nodes.Add(new object[] { ti.Caption });
                        tds.Tag = new MenuCtlData
                        {
                            ControlName = ti.Name,
                            TextName = ti.Caption,
                            CtlType = EnCtlType.ToolBarItems,
                            IsEnabled = false,
                            ClsName = cls,
                            ModName = mod,
                            ModMD5 = md5
                        };
                    }
                }
                if (null != grids && grids.Count > 0)
                {
                    foreach (var ti in grids)
                    {
                        var td = mnu.Nodes.Add(new object[] { ti.Name });
                        td.Tag = new MenuCtlData
                        {
                            ControlName = ti.Gv.Name,
                            TextName = ti.Name,
                            CtlType = EnCtlType.Grids,
                            IsEnabled = false,
                            ClsName = cls,
                            ModName = mod,
                            ModMD5 = md5
                        };
                        foreach (DevExpress.XtraGrid.Columns.GridColumn col in ti.Gv.Columns)
                        {
                            var tdc = td.Nodes.Add(new object[] { col.Caption });

                            tdc.Tag = new MenuCtlData
                            {
                                ControlName = col.Name,
                                ParentCtlName = ti.Gv.Name,
                                TextName = col.Caption,
                                CtlType = EnCtlType.GridCol,
                                IsEnabled = false,
                                ClsName = cls,
                                ModName = mod,
                                ModMD5 = md5
                            };
                        }
                    }
                }
                if (null != btns && btns.Count > 0)
                {
                    var td = mnu.Nodes.Add(new object[] { "自定义按钮" });
                    foreach (var ti in btns)
                    {
                        var tds = td.Nodes.Add(new object[] {  ti.Tag==null ? ti.Text : ti.Tag.ToString() /*ti.Name*/ });
                        tds.Tag = new MenuCtlData
                        {
                            ControlName = ti.Name,
                            TextName = ti.Text,
                            CtlType = EnCtlType.ButtonItems,
                            IsEnabled = false,
                            ClsName = cls,
                            ModName = mod,
                            ModMD5 = md5
                        };
                    }
                }
            }
            catch //(Exception)
            {

                //throw;
            }
        }

        private void BuildlRoleTree()
        {
            foreach (TreeListNode td in treeCtl1.Nodes)
            {
                foreach (TreeListNode tm in td.Nodes)
                {
                    var md = tm.Tag as MenuData;
                    if (null == md || md.MenuItem == null) continue;
                    Refc(md.MenuItem.MenuClsName, md.MenuItem.MenuAsmName, tm);
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs ea)
        {
            var k = roleGridEdit1.GetSelectedValue();
            if (null == k) return;
            List<MenuCtlData> lst = new List<MenuCtlData>(100);
            foreach (TreeListNode td in treeCtl1.Nodes)
            {
                var mnugrp = td.GetValue(0).ToString();
                foreach (TreeListNode tds in td.Nodes)
                {
                    var mnuname = tds.GetValue(0).ToString();
                    foreach (TreeListNode tmd in tds.Nodes)
                    {
                        var mcd = tmd.Tag as MenuCtlData;
                        if (null != mcd)
                        {
                            mcd.IsEnabled = tmd.Checked;
                            mcd.MenuGrpName = mnugrp;
                            mcd.MenuName = mnuname;
                            lst.Add(mcd);
                        }
                        foreach (TreeListNode tkd in tmd.Nodes)
                        {
                            var md = tkd.Tag as MenuCtlData;
                            if (null == md) continue;
                            md.IsEnabled = tkd.Checked;
                            md.MenuGrpName = mnugrp; md.MenuName = mnuname;
                            lst.Add(md);
                        }
                    }
                }
            }
            var usr = GetCurUser<t_user>();
            using (var db = new Database())
            {

                var et = db.FirstOrDefault<t_roleset>("select * from t_roleset where RoleId=@0", k.Id);

                var roleset = et;
                if (et != null)
                {
                    roleset.RoleSetTree = Ultra.Web.Core.Common.ObjectHelper.SerializeJson(lst);
                }
                else
                {
                    roleset = new t_roleset
                    {
                        RoleSetTree = Ultra.Web.Core.Common.ObjectHelper.SerializeJson(lst),
                        RoleId = k.Id,
                        IsUsing = true,
                        RoleName = k.Name,
                        Guid = Guid.NewGuid(),
                        Creator =this.CurUser,
                        CreateDate = TimeSync.Default.CurrentSyncTime
                    };
                }
                db.Save(roleset);
            }
            MsgBox.ShowMessage(null, "保存成功!");
        }

        private void ClearNodeCheck()
        {
            foreach (TreeListNode td in treeCtl1.Nodes)
            {
                td.Checked = false;
                foreach (TreeListNode tds in td.Nodes)
                {
                    tds.Checked = false;
                    foreach (TreeListNode tmd in tds.Nodes)
                    {
                        tmd.Checked = false;
                        foreach (TreeListNode tkd in tmd.Nodes)
                        {
                            tkd.Checked = false;
                        }
                    }
                }
            }
        }

        private void SetNodeRole(List<MenuCtlData> roledata)
        {
            foreach (TreeListNode td in treeCtl1.Nodes)
            {
                foreach (TreeListNode tds in td.Nodes)
                {
                    foreach (TreeListNode tmd in tds.Nodes)
                    {
                        var md = tmd.Tag as MenuCtlData;
                        if (null != md)
                        {
                            var et = roledata.Where(k => k.ClsName == md.ClsName && k.ModName == md.ModName &&
                                md.ControlName == k.ControlName).FirstOrDefault();
                            if (null == et)
                            {
                                tmd.Checked = false;
                                NodeNext(tmd);
                                NodePrev(tmd);
                            }
                            else
                            {
                                tmd.Checked = et.IsEnabled;
                                NodeNext(tmd);
                                NodePrev(tmd);
                            }
                        }
                        foreach (TreeListNode tkd in tmd.Nodes)
                        {
                            var mdt = tkd.Tag as MenuCtlData;
                            if (null == mdt) continue;
                            var et = roledata.Where(k => k.ClsName == mdt.ClsName
                                && k.ModName == mdt.ModName &&
                               mdt.ControlName == k.ControlName).FirstOrDefault();
                            if (null == et)
                            {
                                tkd.Checked = false;
                                NodeNext(tkd);
                                NodePrev(tkd);
                            }
                            else
                            {
                                tkd.Checked = et.IsEnabled;
                                NodeNext(tkd);
                                NodePrev(tkd);
                            }
                        }
                    }
                }
            }
        }

        private void roleGridEdit1_EditValueChanged(object sender, EventArgs e)
        {
            var m = roleGridEdit1.SelectedValue;
            if (null == m) { treeCtl1.Enabled = false; return; }
            treeCtl1.Enabled = true;
            using (var db = new Database())
            {
                var et = db.FirstOrDefault<t_roleset>("select * from t_roleset where RoleId=@0",m.Id);
                if (null == et)
                {
                    //treeCtl1.AfterCheckNode -= treeCtl1_AfterCheckNode;
                    ClearNodeCheck();
                    //treeCtl1.AfterCheckNode += treeCtl1_AfterCheckNode;
                }
                else
                {
                    //treeCtl1.AfterCheckNode -= treeCtl1_AfterCheckNode;
                    var roletree = Ultra.Web.Core.Common.ObjectHelper.DeSerialize<List<MenuCtlData>>(et.RoleSetTree);
                    SetNodeRole(roletree);
                    //List<TreeListNode> lst = null;
                    //CollectNode(null, ref lst);
                }
            }
        }

        void treeCtl1_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            NodeNext(e.Node);
            NodePrev(e.Node);
        }

        void CollectNode(TreeListNode tdk, ref List<TreeListNode> lsttd)
        {
            if (null == lsttd) lsttd = new List<TreeListNode>(100);
            var tds = null == tdk ? treeCtl1.Nodes : tdk.Nodes;
            foreach (TreeListNode td in tds)
            {
                if (td.Nodes != null)
                    CollectNode(td, ref lsttd);
                else
                {
                    if (td.Checked)
                        lsttd.Add(td);
                    CollectNode(td, ref lsttd);
                }

            }
            lsttd.ForEach(k =>
            {
                NodeNext(k);
                NodePrev(k);
            });
        }

        private void NodePrev(TreeListNode td)
        {
            if (td.ParentNode != null)
            {
                if (td.Checked)
                    td.ParentNode.Checked = true;
                else
                {
                    var flg = false;
                    foreach (TreeListNode tnd in td.ParentNode.Nodes)
                    {
                        if (tnd.Checked)
                        {
                            flg = true;
                            break;
                        }
                    }

                    td.ParentNode.Checked = flg;
                }
                NodePrev(td.ParentNode);
            }
            else
            {
                if (!td.Checked)
                {
                    foreach (TreeListNode tkd in td.Nodes)
                    {
                        if (tkd.Checked)
                        { td.Checked = true; break; }
                    }
                }
            }
        }

        private void NodeNext(TreeListNode td)
        {
            foreach (TreeListNode tnd in td.Nodes)
            {
                tnd.Checked = td.Checked;
                if (tnd.Nodes != null)
                    NodeNext(tnd);
            }

        }
    }
}
