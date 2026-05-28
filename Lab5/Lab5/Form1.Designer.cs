namespace Lab5
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        // ── Public form controls (used by model FillForm/FillFromForm) ─────────
        public System.Windows.Forms.TextBox txtName;
        public System.Windows.Forms.TextBox txtPrice;
        public System.Windows.Forms.TextBox txtGenre;
        public System.Windows.Forms.TextBox txtExtra1;
        public System.Windows.Forms.TextBox txtExtra2;

        public System.Windows.Forms.Label lblExtra1;
        public System.Windows.Forms.Label lblExtra2;

        // ── Private controls ──────────────────────────────────────────────────
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblGenre;
        private System.Windows.Forms.Label lblType;

        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnReloadPlugins;
        private System.Windows.Forms.ListBox listBox1;

        // ── Menu strip ────────────────────────────────────────────────────────
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuPlugins;
        private System.Windows.Forms.ToolStripMenuItem menuPluginsManage;
        private System.Windows.Forms.ToolStripMenuItem menuPluginsLoadDll;
        private System.Windows.Forms.ToolStripMenuItem menuPluginsLoadXmlDll;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // ── Controls instantiation ─────────────────────────────────────────
            this.txtName         = new System.Windows.Forms.TextBox();
            this.txtPrice        = new System.Windows.Forms.TextBox();
            this.txtGenre        = new System.Windows.Forms.TextBox();
            this.txtExtra1       = new System.Windows.Forms.TextBox();
            this.txtExtra2       = new System.Windows.Forms.TextBox();

            this.lblName         = new System.Windows.Forms.Label();
            this.lblPrice        = new System.Windows.Forms.Label();
            this.lblGenre        = new System.Windows.Forms.Label();
            this.lblType         = new System.Windows.Forms.Label();
            this.lblExtra1       = new System.Windows.Forms.Label();
            this.lblExtra2       = new System.Windows.Forms.Label();

            this.cmbType         = new System.Windows.Forms.ComboBox();

            this.btnAdd          = new System.Windows.Forms.Button();
            this.btnUpdate       = new System.Windows.Forms.Button();
            this.btnDelete       = new System.Windows.Forms.Button();
            this.btnSave         = new System.Windows.Forms.Button();
            this.btnLoad         = new System.Windows.Forms.Button();
            this.btnReloadPlugins= new System.Windows.Forms.Button();

            this.listBox1        = new System.Windows.Forms.ListBox();

            this.menuStrip           = new System.Windows.Forms.MenuStrip();
            this.menuPlugins         = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPluginsManage   = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPluginsLoadDll  = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPluginsLoadXmlDll = new System.Windows.Forms.ToolStripMenuItem();

            this.SuspendLayout();

            // ── MenuStrip ─────────────────────────────────────────────────────
            this.menuStrip.Items.Add(this.menuPlugins);
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Size = new System.Drawing.Size(440, 24);

            this.menuPlugins.Text = "Plugins";
            this.menuPlugins.DropDownItems.Add(this.menuPluginsManage);
            this.menuPlugins.DropDownItems.Add(this.menuPluginsLoadDll);
            this.menuPlugins.DropDownItems.Add(this.menuPluginsLoadXmlDll);

            this.menuPluginsManage.Text = "Manage XML Plugins...";
            this.menuPluginsManage.Click += new System.EventHandler(this.menuPluginsManage_Click);

            this.menuPluginsLoadDll.Text = "Load Game Plugin DLL...";
            this.menuPluginsLoadDll.Click += new System.EventHandler(this.menuPluginsLoadDll_Click);

            this.menuPluginsLoadXmlDll.Text = "Load XML Plugin DLL...";
            this.menuPluginsLoadXmlDll.Click += new System.EventHandler(this.menuPluginsLoadXmlDll_Click);

            // ── Labels ────────────────────────────────────────────────────────
            int top = 34; // below the menu strip

            lblName.Text     = "Name:";
            lblName.Location = new System.Drawing.Point(20, top + 0);

            lblPrice.Text     = "Price:";
            lblPrice.Location = new System.Drawing.Point(20, top + 30);

            lblGenre.Text     = "Genre:";
            lblGenre.Location = new System.Drawing.Point(20, top + 60);

            lblType.Text     = "Type:";
            lblType.Location = new System.Drawing.Point(20, top + 90);

            lblExtra1.Text     = "Field1:";
            lblExtra1.Location = new System.Drawing.Point(20, top + 120);

            lblExtra2.Text     = "Field2:";
            lblExtra2.Location = new System.Drawing.Point(20, top + 150);

            // ── TextBoxes ─────────────────────────────────────────────────────
            txtName.Location  = new System.Drawing.Point(120, top + 0);
            txtName.Width     = 160;
            txtPrice.Location = new System.Drawing.Point(120, top + 30);
            txtPrice.Width    = 160;
            txtGenre.Location = new System.Drawing.Point(120, top + 60);
            txtGenre.Width    = 160;
            txtExtra1.Location= new System.Drawing.Point(120, top + 120);
            txtExtra1.Width   = 160;
            txtExtra2.Location= new System.Drawing.Point(120, top + 150);
            txtExtra2.Width   = 160;

            // ── ComboBox ──────────────────────────────────────────────────────
            cmbType.Location      = new System.Drawing.Point(120, top + 90);
            cmbType.Size          = new System.Drawing.Size(160, 21);
            cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);

            // ── Buttons ───────────────────────────────────────────────────────
            btnAdd.Text     = "Add";
            btnAdd.Location = new System.Drawing.Point(300, top + 0);
            btnAdd.Width    = 120;
            btnAdd.Click   += new System.EventHandler(this.btnAdd_Click);

            btnUpdate.Text     = "Update";
            btnUpdate.Location = new System.Drawing.Point(300, top + 30);
            btnUpdate.Width    = 120;
            btnUpdate.Click   += new System.EventHandler(this.btnUpdate_Click);

            btnDelete.Text     = "Delete";
            btnDelete.Location = new System.Drawing.Point(300, top + 60);
            btnDelete.Width    = 120;
            btnDelete.Click   += new System.EventHandler(this.btnDelete_Click);

            btnSave.Text     = "Save (XML)";
            btnSave.Location = new System.Drawing.Point(300, top + 90);
            btnSave.Width    = 120;
            btnSave.Click   += new System.EventHandler(this.btnSave_Click);

            btnLoad.Text     = "Load (XML)";
            btnLoad.Location = new System.Drawing.Point(300, top + 120);
            btnLoad.Width    = 120;
            btnLoad.Click   += new System.EventHandler(this.btnLoad_Click);

            btnReloadPlugins.Text     = "Reload Plugins";
            btnReloadPlugins.Location = new System.Drawing.Point(300, top + 150);
            btnReloadPlugins.Width    = 120;
            btnReloadPlugins.Click   += new System.EventHandler(this.btnReloadPlugins_Click);

            // ── ListBox ───────────────────────────────────────────────────────
            listBox1.Location             = new System.Drawing.Point(20, top + 195);
            listBox1.Size                 = new System.Drawing.Size(400, 150);
            listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);

            // ── Form ──────────────────────────────────────────────────────────
            this.ClientSize = new System.Drawing.Size(440, top + 370);
            this.Text       = "Game Library — Lab5 (XML Plugins)";
            this.MainMenuStrip = this.menuStrip;

            this.Controls.Add(this.menuStrip);
            this.Controls.Add(lblName);
            this.Controls.Add(lblPrice);
            this.Controls.Add(lblGenre);
            this.Controls.Add(lblType);
            this.Controls.Add(lblExtra1);
            this.Controls.Add(lblExtra2);
            this.Controls.Add(txtName);
            this.Controls.Add(txtPrice);
            this.Controls.Add(txtGenre);
            this.Controls.Add(txtExtra1);
            this.Controls.Add(txtExtra2);
            this.Controls.Add(cmbType);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnUpdate);
            this.Controls.Add(btnDelete);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnLoad);
            this.Controls.Add(btnReloadPlugins);
            this.Controls.Add(listBox1);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
