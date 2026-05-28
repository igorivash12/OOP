namespace lab6
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem miSettings;
        private System.Windows.Forms.ToolStripMenuItem miPluginSettings;
        private System.Windows.Forms.ToolStripMenuItem miLoadPlugin;
        private System.Windows.Forms.ToolStripMenuItem miViewLog;
        private System.Windows.Forms.ListBox listItems;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label labelCreator;
        private System.Windows.Forms.TextBox txtCreator;
        private System.Windows.Forms.Label labelYear;
        private System.Windows.Forms.NumericUpDown nudYear;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label labelDetails;
        private System.Windows.Forms.TextBox txtDetails;
        private System.Windows.Forms.Label labelPluginReport;
        private System.Windows.Forms.TextBox lblPluginReport;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.miSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.miPluginSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.miLoadPlugin = new System.Windows.Forms.ToolStripMenuItem();
            this.miViewLog = new System.Windows.Forms.ToolStripMenuItem();
            this.listItems = new System.Windows.Forms.ListBox();
            this.labelTitle = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.labelCreator = new System.Windows.Forms.Label();
            this.txtCreator = new System.Windows.Forms.TextBox();
            this.labelYear = new System.Windows.Forms.Label();
            this.nudYear = new System.Windows.Forms.NumericUpDown();
            this.labelType = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.labelDetails = new System.Windows.Forms.Label();
            this.txtDetails = new System.Windows.Forms.TextBox();
            this.labelPluginReport = new System.Windows.Forms.Label();
            this.lblPluginReport = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudYear)).BeginInit();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();

            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.miSettings });
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";

            this.miSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { this.miPluginSettings, this.miLoadPlugin, this.miViewLog });
            this.miSettings.Text = "Settings";

            this.miPluginSettings.Text = "Plugin settings...";
            this.miPluginSettings.Click += new System.EventHandler(this.miPluginSettings_Click);

            this.miLoadPlugin.Text = "Load plugin from file...";
            this.miLoadPlugin.Click += new System.EventHandler(this.miLoadPlugin_Click);

            this.miViewLog.Text = "View processing log (Decorator output)";
            this.miViewLog.Click += new System.EventHandler(this.miViewLog_Click);

            this.listItems.FormattingEnabled = true;
            this.listItems.ItemHeight = 16;
            this.listItems.Location = new System.Drawing.Point(12, 32);
            this.listItems.Size = new System.Drawing.Size(360, 388);
            this.listItems.SelectedIndexChanged += new System.EventHandler(this.listItems_SelectedIndexChanged);

            this.labelTitle.AutoSize = true;
            this.labelTitle.Location = new System.Drawing.Point(390, 40);
            this.labelTitle.Text = "Name:";

            this.txtTitle.Location = new System.Drawing.Point(480, 37);
            this.txtTitle.Size = new System.Drawing.Size(250, 22);

            this.labelCreator.AutoSize = true;
            this.labelCreator.Location = new System.Drawing.Point(390, 75);
            this.labelCreator.Text = "Coach:";

            this.txtCreator.Location = new System.Drawing.Point(480, 72);
            this.txtCreator.Size = new System.Drawing.Size(250, 22);

            this.labelYear.AutoSize = true;
            this.labelYear.Location = new System.Drawing.Point(390, 110);
            this.labelYear.Text = "Year:";

            this.nudYear.Location = new System.Drawing.Point(480, 108);
            this.nudYear.Maximum = new decimal(new int[] { 2100, 0, 0, 0 });
            this.nudYear.Minimum = new decimal(new int[] { 1900, 0, 0, 0 });
            this.nudYear.Size = new System.Drawing.Size(120, 22);
            this.nudYear.Value = new decimal(new int[] { 2024, 0, 0, 0 });

            this.labelType.AutoSize = true;
            this.labelType.Location = new System.Drawing.Point(390, 145);
            this.labelType.Text = "Sport type:";

            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.Location = new System.Drawing.Point(480, 142);
            this.cmbType.Size = new System.Drawing.Size(250, 24);

            this.btnAdd.Location = new System.Drawing.Point(393, 190);
            this.btnAdd.Size = new System.Drawing.Size(100, 30);
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            this.btnRemove.Location = new System.Drawing.Point(510, 190);
            this.btnRemove.Size = new System.Drawing.Size(100, 30);
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);

            this.btnUpdate.Location = new System.Drawing.Point(630, 190);
            this.btnUpdate.Size = new System.Drawing.Size(100, 30);
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);

            this.btnSave.Location = new System.Drawing.Point(393, 240);
            this.btnSave.Size = new System.Drawing.Size(160, 35);
            this.btnSave.Text = "Save to BSON...";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            this.btnLoad.Location = new System.Drawing.Point(570, 240);
            this.btnLoad.Size = new System.Drawing.Size(160, 35);
            this.btnLoad.Text = "Load from BSON...";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);

            this.labelDetails.AutoSize = true;
            this.labelDetails.Location = new System.Drawing.Point(390, 295);
            this.labelDetails.Text = "Details:";

            this.txtDetails.Location = new System.Drawing.Point(393, 315);
            this.txtDetails.Multiline = true;
            this.txtDetails.ReadOnly = true;
            this.txtDetails.Size = new System.Drawing.Size(337, 70);

            this.labelPluginReport.AutoSize = true;
            this.labelPluginReport.Location = new System.Drawing.Point(12, 430);
            this.labelPluginReport.Text = "Plugins:";

            this.lblPluginReport.Location = new System.Drawing.Point(12, 450);
            this.lblPluginReport.Multiline = true;
            this.lblPluginReport.ReadOnly = true;
            this.lblPluginReport.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.lblPluginReport.Size = new System.Drawing.Size(718, 90);

            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 555);
            this.Controls.Add(this.lblPluginReport);
            this.Controls.Add(this.labelPluginReport);
            this.Controls.Add(this.txtDetails);
            this.Controls.Add(this.labelDetails);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.labelType);
            this.Controls.Add(this.nudYear);
            this.Controls.Add(this.labelYear);
            this.Controls.Add(this.txtCreator);
            this.Controls.Add(this.labelCreator);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.listItems);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lab 6 - Patterns (Adapter, Singleton, Decorator)";
            ((System.ComponentModel.ISupportInitialize)(this.nudYear)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
    }
}
