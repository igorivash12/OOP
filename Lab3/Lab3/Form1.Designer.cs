namespace Lab3
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblGenre;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblExtra1;
        private System.Windows.Forms.Label lblExtra2;

        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.TextBox txtGenre;
        private System.Windows.Forms.TextBox txtExtra1;
        private System.Windows.Forms.TextBox txtExtra2;

        private System.Windows.Forms.ComboBox cmbType;

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoad;

        private System.Windows.Forms.ListBox listBox1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblName = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblGenre = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.lblExtra1 = new System.Windows.Forms.Label();
            this.lblExtra2 = new System.Windows.Forms.Label();

            this.txtName = new System.Windows.Forms.TextBox();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.txtGenre = new System.Windows.Forms.TextBox();
            this.txtExtra1 = new System.Windows.Forms.TextBox();
            this.txtExtra2 = new System.Windows.Forms.TextBox();

            this.cmbType = new System.Windows.Forms.ComboBox();

            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();

            this.listBox1 = new System.Windows.Forms.ListBox();

            this.SuspendLayout();

            // ===== Labels =====
            this.lblName.AutoSize = true;
            this.lblName.Text = "Name:";
            this.lblName.Location = new System.Drawing.Point(20, 10);

            this.lblPrice.AutoSize = true;
            this.lblPrice.Text = "Price:";
            this.lblPrice.Location = new System.Drawing.Point(20, 40);

            this.lblGenre.AutoSize = true;
            this.lblGenre.Text = "Genre:";
            this.lblGenre.Location = new System.Drawing.Point(20, 70);

            this.lblType.AutoSize = true;
            this.lblType.Text = "Game Type:";
            this.lblType.Location = new System.Drawing.Point(20, 100);

            this.lblExtra1.AutoSize = true;
            this.lblExtra1.Text = "Extra1:";
            this.lblExtra1.Location = new System.Drawing.Point(20, 130);

            this.lblExtra2.AutoSize = true;
            this.lblExtra2.Text = "Extra2:";
            this.lblExtra2.Location = new System.Drawing.Point(20, 160);

            // ===== TextBoxes =====
            this.txtName.Location = new System.Drawing.Point(140, 10);
            this.txtName.Size = new System.Drawing.Size(150, 23);

            this.txtPrice.Location = new System.Drawing.Point(140, 40);
            this.txtPrice.Size = new System.Drawing.Size(150, 23);

            this.txtGenre.Location = new System.Drawing.Point(140, 70);
            this.txtGenre.Size = new System.Drawing.Size(150, 23);

            this.txtExtra1.Location = new System.Drawing.Point(140, 130);
            this.txtExtra1.Size = new System.Drawing.Size(150, 23);

            this.txtExtra2.Location = new System.Drawing.Point(140, 160);
            this.txtExtra2.Size = new System.Drawing.Size(150, 23);

            // ===== ComboBox =====
            this.cmbType.Location = new System.Drawing.Point(140, 100);
            this.cmbType.Size = new System.Drawing.Size(150, 23);
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.Items.AddRange(new object[] {
                "PCGame",
                "ConsoleGame",
                "MobileGame",
                "OnlineGame",
                "IndieGame",
                "VRGame"
            });
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);

            // ===== Buttons =====
            this.btnAdd.Location = new System.Drawing.Point(320, 10);
            this.btnAdd.Size = new System.Drawing.Size(90, 25);
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            this.btnUpdate.Location = new System.Drawing.Point(320, 40);
            this.btnUpdate.Size = new System.Drawing.Size(90, 25);
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);

            this.btnDelete.Location = new System.Drawing.Point(320, 70);
            this.btnDelete.Size = new System.Drawing.Size(90, 25);
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            this.btnSave.Location = new System.Drawing.Point(320, 100);
            this.btnSave.Size = new System.Drawing.Size(90, 25);
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            this.btnLoad.Location = new System.Drawing.Point(320, 130);
            this.btnLoad.Size = new System.Drawing.Size(90, 25);
            this.btnLoad.Text = "Load";
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);

            // ===== ListBox =====
            this.listBox1.Location = new System.Drawing.Point(20, 200);
            this.listBox1.Size = new System.Drawing.Size(390, 150);
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);

            // ===== Form =====
            this.ClientSize = new System.Drawing.Size(450, 370);
            this.Text = "Lab3 - Game Library";

            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.lblGenre);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.lblExtra1);
            this.Controls.Add(this.lblExtra2);

            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.txtGenre);
            this.Controls.Add(this.txtExtra1);
            this.Controls.Add(this.txtExtra2);

            this.Controls.Add(this.cmbType);

            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLoad);

            this.Controls.Add(this.listBox1);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}