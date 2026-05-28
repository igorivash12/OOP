namespace Lab5
{
    partial class FormPluginManager
    {
        private System.ComponentModel.IContainer components = null;

        // ── Controls ──────────────────────────────────────────────────────────
        private System.Windows.Forms.CheckedListBox clbPlugins;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Button btnLoadDll;
        private System.Windows.Forms.Button btnEnableAll;
        private System.Windows.Forms.Button btnDisableAll;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblPlugins;
        private System.Windows.Forms.Label lblDescription;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.clbPlugins    = new System.Windows.Forms.CheckedListBox();
            this.txtDescription= new System.Windows.Forms.TextBox();
            this.btnLoadDll    = new System.Windows.Forms.Button();
            this.btnEnableAll  = new System.Windows.Forms.Button();
            this.btnDisableAll = new System.Windows.Forms.Button();
            this.btnClose      = new System.Windows.Forms.Button();
            this.lblPlugins    = new System.Windows.Forms.Label();
            this.lblDescription= new System.Windows.Forms.Label();

            this.SuspendLayout();

            // ── Labels ────────────────────────────────────────────────────────
            lblPlugins.Text     = "XML Plugins (check to enable):";
            lblPlugins.Location = new System.Drawing.Point(10, 10);
            lblPlugins.Size     = new System.Drawing.Size(220, 18);

            lblDescription.Text     = "Plugin info:";
            lblDescription.Location = new System.Drawing.Point(10, 200);
            lblDescription.Size     = new System.Drawing.Size(100, 18);

            // ── Checked list box ──────────────────────────────────────────────
            clbPlugins.Location     = new System.Drawing.Point(10, 30);
            clbPlugins.Size         = new System.Drawing.Size(250, 160);
            clbPlugins.CheckOnClick = true;
            clbPlugins.ItemCheck   += new System.Windows.Forms.ItemCheckEventHandler(this.clbPlugins_ItemCheck);
            clbPlugins.SelectedIndexChanged += new System.EventHandler(this.clbPlugins_SelectedIndexChanged);

            // ── Description text ──────────────────────────────────────────────
            txtDescription.Location  = new System.Drawing.Point(10, 220);
            txtDescription.Size      = new System.Drawing.Size(360, 100);
            txtDescription.Multiline = true;
            txtDescription.ReadOnly  = true;
            txtDescription.ScrollBars= System.Windows.Forms.ScrollBars.Vertical;

            // ── Buttons ───────────────────────────────────────────────────────
            btnEnableAll.Text     = "Enable All";
            btnEnableAll.Location = new System.Drawing.Point(270, 30);
            btnEnableAll.Size     = new System.Drawing.Size(100, 30);
            btnEnableAll.Click   += new System.EventHandler(this.btnEnableAll_Click);

            btnDisableAll.Text     = "Disable All";
            btnDisableAll.Location = new System.Drawing.Point(270, 70);
            btnDisableAll.Size     = new System.Drawing.Size(100, 30);
            btnDisableAll.Click   += new System.EventHandler(this.btnDisableAll_Click);

            btnLoadDll.Text     = "Load DLL...";
            btnLoadDll.Location = new System.Drawing.Point(270, 110);
            btnLoadDll.Size     = new System.Drawing.Size(100, 30);
            btnLoadDll.Click   += new System.EventHandler(this.btnLoadDll_Click);

            btnClose.Text     = "Close";
            btnClose.Location = new System.Drawing.Point(270, 290);
            btnClose.Size     = new System.Drawing.Size(100, 30);
            btnClose.Click   += new System.EventHandler(this.btnClose_Click);

            // ── Form ──────────────────────────────────────────────────────────
            this.Text          = "XML Plugin Manager — Lab5";
            this.ClientSize    = new System.Drawing.Size(390, 340);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox   = false;
            this.MinimizeBox   = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;

            this.Controls.Add(lblPlugins);
            this.Controls.Add(clbPlugins);
            this.Controls.Add(lblDescription);
            this.Controls.Add(txtDescription);
            this.Controls.Add(btnEnableAll);
            this.Controls.Add(btnDisableAll);
            this.Controls.Add(btnLoadDll);
            this.Controls.Add(btnClose);

            this.ResumeLayout(false);
        }
    }
}
