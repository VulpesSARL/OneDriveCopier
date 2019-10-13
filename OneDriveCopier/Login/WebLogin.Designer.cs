namespace OneDriveCopier.Login
{
    partial class frmWebLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.www = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // www
            // 
            this.www.AllowWebBrowserDrop = false;
            this.www.Dock = System.Windows.Forms.DockStyle.Fill;
            this.www.IsWebBrowserContextMenuEnabled = false;
            this.www.Location = new System.Drawing.Point(0, 0);
            this.www.MinimumSize = new System.Drawing.Size(20, 20);
            this.www.Name = "www";
            this.www.ScriptErrorsSuppressed = true;
            this.www.Size = new System.Drawing.Size(478, 592);
            this.www.TabIndex = 0;
            // 
            // frmWebLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 592);
            this.Controls.Add(this.www);
            this.Name = "frmWebLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Login to Microsoft Account";
            this.Load += new System.EventHandler(this.frmWebLogin_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser www;
    }
}