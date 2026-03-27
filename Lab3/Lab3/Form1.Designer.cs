namespace Lab3
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblExtra;
        private System.Windows.Forms.Label lblBrand;
        private System.Windows.Forms.Label lblType;

        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.TextBox txtExtra;
        private System.Windows.Forms.TextBox txtBrand;

        private System.Windows.Forms.ComboBox cmbType;

        private System.Windows.Forms.Button btnAdd;
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
            this.lblExtra = new System.Windows.Forms.Label();
            this.lblBrand = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();

            this.txtName = new System.Windows.Forms.TextBox();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.txtExtra = new System.Windows.Forms.TextBox();
            this.txtBrand = new System.Windows.Forms.TextBox();

            this.cmbType = new System.Windows.Forms.ComboBox();

            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();

            this.listBox1 = new System.Windows.Forms.ListBox();

            this.SuspendLayout();

            // Labels
            this.lblName.Text = "Name:";
            this.lblName.Location = new System.Drawing.Point(20, 10);

            this.lblPrice.Text = "Price:";
            this.lblPrice.Location = new System.Drawing.Point(20, 40);

            this.lblExtra.Text = "Volume / Type:";
            this.lblExtra.Location = new System.Drawing.Point(20, 70);

            this.lblBrand.Text = "Brand:";
            this.lblBrand.Location = new System.Drawing.Point(20, 100);

            this.lblType.Text = "Product Type:";
            this.lblType.Location = new System.Drawing.Point(20, 130);

            // TextBoxes
            this.txtName.Location = new System.Drawing.Point(140, 10);
            this.txtPrice.Location = new System.Drawing.Point(140, 40);
            this.txtExtra.Location = new System.Drawing.Point(140, 70);
            this.txtBrand.Location = new System.Drawing.Point(140, 100);

            // ComboBox
            this.cmbType.Location = new System.Drawing.Point(140, 130);
            this.cmbType.Size = new System.Drawing.Size(150, 23);
            this.cmbType.Items.AddRange(new object[] {
                "LuxuryPerfume",
                "BudgetPerfume",
                "Cosmetic"
            });

            // Buttons
            this.btnAdd.Location = new System.Drawing.Point(320, 10);
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            this.btnDelete.Location = new System.Drawing.Point(320, 40);
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            this.btnSave.Location = new System.Drawing.Point(320, 70);
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            this.btnLoad.Location = new System.Drawing.Point(320, 100);
            this.btnLoad.Text = "Load";
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);

            // ListBox
            this.listBox1.Location = new System.Drawing.Point(20, 170);
            this.listBox1.Size = new System.Drawing.Size(350, 150);

            // Form
            this.ClientSize = new System.Drawing.Size(420, 350);

            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.lblExtra);
            this.Controls.Add(this.lblBrand);
            this.Controls.Add(this.lblType);

            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.txtExtra);
            this.Controls.Add(this.txtBrand);

            this.Controls.Add(this.cmbType);

            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLoad);

            this.Controls.Add(this.listBox1);

            this.Text = "Serialization App";

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}