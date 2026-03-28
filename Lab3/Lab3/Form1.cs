using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Lab3.Models;
using Lab3.Services;

namespace Lab3
{
    public partial class Form1 : Form
    {
        private List<BaseEntity> items = new List<BaseEntity>();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cmbType.SelectedItem == null)
            {
                MessageBox.Show("Select product type!");
                return;
            }

            try
            {
                BaseEntity item = CreateFromInput();
                items.Add(item);
                UpdateList();
            }
            catch
            {
                MessageBox.Show("Invalid input!");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                MessageBox.Show("Select item to update!");
                return;
            }

            try
            {
                int index = listBox1.SelectedIndex;
                items[index] = CreateFromInput();
                UpdateList();
            }
            catch
            {
                MessageBox.Show("Invalid input!");
            }
        }

        private BaseEntity CreateFromInput()
        {
            string type = cmbType.SelectedItem.ToString();

            switch (type)
            {
                case "LuxuryPerfume":
                    return new LuxuryPerfume()
                    {
                        Name = txtName.Text,
                        Price = double.Parse(txtPrice.Text),
                        Volume = int.Parse(txtExtra.Text),
                        Brand = txtBrand.Text
                    };

                case "BudgetPerfume":
                    return new BudgetPerfume()
                    {
                        Name = txtName.Text,
                        Price = double.Parse(txtPrice.Text),
                        Volume = int.Parse(txtExtra.Text)
                    };

                case "Cosmetic":
                    return new Cosmetic()
                    {
                        Name = txtName.Text,
                        Price = double.Parse(txtPrice.Text),
                        Type = txtExtra.Text
                    };

                default:
                    throw new Exception();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
                return;

            var item = items[listBox1.SelectedIndex];

            txtName.Text = item.Name;

            if (item is LuxuryPerfume lp)
            {
                cmbType.SelectedItem = "LuxuryPerfume";
                txtPrice.Text = lp.Price.ToString();
                txtExtra.Text = lp.Volume.ToString();
                txtBrand.Text = lp.Brand;
            }
            else if (item is BudgetPerfume bp)
            {
                cmbType.SelectedItem = "BudgetPerfume";
                txtPrice.Text = bp.Price.ToString();
                txtExtra.Text = bp.Volume.ToString();
                txtBrand.Text = "";
            }
            else if (item is Cosmetic c)
            {
                cmbType.SelectedItem = "Cosmetic";
                txtPrice.Text = c.Price.ToString();
                txtExtra.Text = c.Type;
                txtBrand.Text = "";
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                items.RemoveAt(listBox1.SelectedIndex);
                UpdateList();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK)
                JsonSerializerService.Save("data.json", items);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
                items = JsonSerializerService.Load("data.json");
            UpdateList();
        }

        private void UpdateList()
        {
            listBox1.Items.Clear();

            foreach (var item in items)
            {
                listBox1.Items.Add(item.Name);
            }
        }
    }
}