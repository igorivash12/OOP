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

        // Sets labels for dynamic fields
        public void SetLabels(string l1, string l2)
        {
            lblExtra1.Text = l1;
            lblExtra2.Text = l2;
        }

        // Creates object using factory and fills it from form
        private BaseEntity CreateFromInput()
        {
            var obj = GameFactory.Create(cmbType.SelectedItem.ToString());
            obj.FillFromForm(this);
            return obj;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                items.Add(CreateFromInput());
                UpdateList();
            }
            catch
            {
                MessageBox.Show("Invalid input");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                MessageBox.Show("Select item");
                return;
            }

            try
            {
                items[listBox1.SelectedIndex] = CreateFromInput();
                UpdateList();
            }
            catch
            {
                MessageBox.Show("Invalid input");
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
            JsonSerializerService.Save("games.json", items);
            MessageBox.Show("Saved");
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            items = JsonSerializerService.Load("games.json");
            UpdateList();
            MessageBox.Show("Loaded");
        }

        // Updates list display
        private void UpdateList()
        {
            listBox1.Items.Clear();
            foreach (var item in items)
                listBox1.Items.Add(item.Name);
        }

        // Handles selection change in list
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
                return;

            var item = items[listBox1.SelectedIndex];

            cmbType.SelectedItem = item.GetTypeName();
            item.FillForm(this);
        }

        // Handles type change in ComboBox
        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbType.SelectedItem == null)
                return;

            var obj = GameFactory.Create(cmbType.SelectedItem.ToString());

            // Only update labels (fields may be empty)
            obj.FillForm(this);
        }
    }
}