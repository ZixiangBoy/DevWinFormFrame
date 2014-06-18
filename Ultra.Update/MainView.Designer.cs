namespace Ultra.Update {
    partial class MainView {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            this.lblmsg = new System.Windows.Forms.Label();
            this.lnklbldownload = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lblmsg
            // 
            this.lblmsg.AutoSize = true;
            this.lblmsg.Location = new System.Drawing.Point(86, 29);
            this.lblmsg.Name = "lblmsg";
            this.lblmsg.Size = new System.Drawing.Size(41, 12);
            this.lblmsg.TabIndex = 0;
            this.lblmsg.Text = "lblmsg";
            // 
            // lnklbldownload
            // 
            this.lnklbldownload.AutoSize = true;
            this.lnklbldownload.Location = new System.Drawing.Point(86, 76);
            this.lnklbldownload.Name = "lnklbldownload";
            this.lnklbldownload.Size = new System.Drawing.Size(101, 12);
            this.lnklbldownload.TabIndex = 1;
            this.lnklbldownload.TabStop = true;
            this.lnklbldownload.Text = "手动下载更新文件";
            this.lnklbldownload.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnklbldownload_LinkClicked);
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 108);
            this.Controls.Add(this.lnklbldownload);
            this.Controls.Add(this.lblmsg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainView";
            this.ShowIcon = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblmsg;
        private System.Windows.Forms.LinkLabel lnklbldownload;
    }
}

