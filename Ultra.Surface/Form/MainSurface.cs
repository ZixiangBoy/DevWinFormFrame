using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ultra.Surface.Form
{
    public partial class MainSurface : BaseSurface
    {
        public MainSurface()
        {
            InitializeComponent();
            this.bar1.OptionsBar.MultiLine = false;
            //if (this.imageList1.Images.Count < 25)
            //{
            this.imageList1.Images.Add(Ultra.Surface.Properties.Resources.PrintTemplates_Icon__174_);//24
            this.imageList1.Images.Add(Ultra.Surface.Properties.Resources.BO_Persons_Large);//25
            this.imageList1.Images.Add(Ultra.Surface.Properties.Resources.add_to_cart);//26
            this.imageList1.Images.Add(Ultra.Surface.Properties.Resources.mail_get);//27
            this.imageList1.Images.Add(Ultra.Surface.Properties.Resources.mail_get__1_);//28
            this.imageList1.Images.Add(Ultra.Surface.Properties.Resources.list_accept);//29
            this.imageList1.Images.Add(Ultra.Surface.Properties.Resources.make);//30
            this.imageList1.Images.Add(Ultra.Surface.Properties.Resources.user_student_assistant);//31
            this.imageList1.Images.Add(Ultra.Surface.Properties.Resources.tick);
            this.imageList1.Images.Add(Ultra.Surface.Properties.Resources.interact);//33
            this.imageList1.Images.Add(Ultra.Surface.Properties.Resources.busy);//34
            this.imageList1.Images.Add(Ultra.Surface.Properties.Resources.agt_action_success);//35
            this.imageList1.Images.Add(Ultra.Surface.Properties.Resources.reload);//36
            this.imageList1.Images.Add(Ultra.Surface.Properties.Resources.system_upgrade);//37
            //}
        }
    }
}
