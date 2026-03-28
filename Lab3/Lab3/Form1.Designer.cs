namespace Lab3
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        public System.Windows.Forms.TextBox txtName;
        public System.Windows.Forms.TextBox txtPrice;
        public System.Windows.Forms.TextBox txtGenre;
        public System.Windows.Forms.TextBox txtExtra1;
        public System.Windows.Forms.TextBox txtExtra2;

        public System.Windows.Forms.Label lblExtra1;
        public System.Windows.Forms.Label lblExtra2;

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
        private System.Windows.Forms.ListBox listBox1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.txtGenre = new System.Windows.Forms.TextBox();
            this.txtExtra1 = new System.Windows.Forms.TextBox();
            this.txtExtra2 = new System.Windows.Forms.TextBox();

            this.lblName = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblGenre = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.lblExtra1 = new System.Windows.Forms.Label();
            this.lblExtra2 = new System.Windows.Forms.Label();

            this.cmbType = new System.Windows.Forms.ComboBox();

            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();

            this.listBox1 = new System.Windows.Forms.ListBox();

            this.SuspendLayout();

            // Labels
            lblName.Text = "Name:";
            lblName.Location = new System.Drawing.Point(20, 10);

            lblPrice.Text = "Price:";
            lblPrice.Location = new System.Drawing.Point(20, 40);

            lblGenre.Text = "Genre:";
            lblGenre.Location = new System.Drawing.Point(20, 70);

            lblType.Text = "Type:";
            lblType.Location = new System.Drawing.Point(20, 100);

            lblExtra1.Text = "Field1:";
            lblExtra1.Location = new System.Drawing.Point(20, 130);

            lblExtra2.Text = "Field2:";
            lblExtra2.Location = new System.Drawing.Point(20, 160);

            // TextBoxes
            txtName.Location = new System.Drawing.Point(120, 10);
            txtPrice.Location = new System.Drawing.Point(120, 40);
            txtGenre.Location = new System.Drawing.Point(120, 70);
            txtExtra1.Location = new System.Drawing.Point(120, 130);
            txtExtra2.Location = new System.Drawing.Point(120, 160);

            // ComboBox
            cmbType.Location = new System.Drawing.Point(120, 100);
            cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbType.Items.AddRange(new object[]
            {
                "PCGame","ConsoleGame","MobileGame","OnlineGame","IndieGame","VRGame"
            });
            cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);

            // Buttons
            btnAdd.Text = "Add";
            btnAdd.Location = new System.Drawing.Point(300, 10);
            btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            btnUpdate.Text = "Update";
            btnUpdate.Location = new System.Drawing.Point(300, 40);
            btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);

            btnDelete.Text = "Delete";
            btnDelete.Location = new System.Drawing.Point(300, 70);
            btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            btnSave.Text = "Save";
            btnSave.Location = new System.Drawing.Point(300, 100);
            btnSave.Click += new System.EventHandler(this.btnSave_Click);

            btnLoad.Text = "Load";
            btnLoad.Location = new System.Drawing.Point(300, 130);
            btnLoad.Click += new System.EventHandler(this.btnLoad_Click);

            // ListBox
            listBox1.Location = new System.Drawing.Point(20, 200);
            listBox1.Size = new System.Drawing.Size(350, 150);
            listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);

            // Form
            this.ClientSize = new System.Drawing.Size(400, 370);
            this.Text = "Game Library";

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

            this.Controls.Add(listBox1);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}