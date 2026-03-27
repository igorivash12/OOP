using Lab3.Models;
using Lab3.Services;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Linq;

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
                BaseEntity item = null;
                string type = cmbType.SelectedItem.ToString();

                switch (type)
                {
                    case "LuxuryPerfume":
                        item = new LuxuryPerfume()
                        {
                            Name = txtName.Text,
                            Price = double.Parse(txtPrice.Text),
                            Volume = int.Parse(txtExtra.Text),
                            Brand = txtBrand.Text
                        };
                        break;

                    case "BudgetPerfume":
                        item = new BudgetPerfume()
                        {
                            Name = txtName.Text,
                            Price = double.Parse(txtPrice.Text),
                            Volume = int.Parse(txtExtra.Text)
                        };
                        break;

                    case "Cosmetic":
                        item = new Cosmetic()
                        {
                            Name = txtName.Text,
                            Price = double.Parse(txtPrice.Text),
                            Type = txtExtra.Text
                        };
                        break;
                }

                items.Add(item);
                UpdateList();
            }
            catch
            {
                MessageBox.Show("Invalid input!");
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
            JsonSerializerService.Save("data.json", items);
            MessageBox.Show("Saved!");
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            items = JsonSerializerService.Load("data.json");
            UpdateList();
            MessageBox.Show("Loaded!");
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