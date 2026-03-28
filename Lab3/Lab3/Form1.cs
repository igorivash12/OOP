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

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = cmbType.SelectedItem.ToString();

            switch (type)
            {
                case "PCGame":
                    lblExtra1.Text = "Platform:";
                    lblExtra2.Text = "RAM (GB):";
                    break;

                case "ConsoleGame":
                    lblExtra1.Text = "Console:";
                    lblExtra2.Text = "Max Players:";
                    break;

                case "MobileGame":
                    lblExtra1.Text = "OS:";
                    lblExtra2.Text = "Has Ads:";
                    break;

                case "OnlineGame":
                    lblExtra1.Text = "Servers:";
                    lblExtra2.Text = "Subscription:";
                    break;

                case "IndieGame":
                    lblExtra1.Text = "Studio:";
                    lblExtra2.Text = "Team Size:";
                    break;

                case "VRGame":
                    lblExtra1.Text = "VR Device:";
                    lblExtra2.Text = "Controllers:";
                    break;
            }
        }

        private BaseEntity CreateFromInput()
        {
            string type = cmbType.SelectedItem.ToString();

            switch (type)
            {
                case "PCGame":
                    return new PCGame
                    {
                        Name = txtName.Text,
                        Price = double.Parse(txtPrice.Text),
                        Genre = txtGenre.Text,
                        Platform = txtExtra1.Text,
                        RequiredRAM = int.Parse(txtExtra2.Text)
                    };

                case "ConsoleGame":
                    return new ConsoleGame
                    {
                        Name = txtName.Text,
                        Price = double.Parse(txtPrice.Text),
                        Genre = txtGenre.Text,
                        Console = txtExtra1.Text,
                        MaxPlayers = int.Parse(txtExtra2.Text)
                    };

                case "MobileGame":
                    return new MobileGame
                    {
                        Name = txtName.Text,
                        Price = double.Parse(txtPrice.Text),
                        Genre = txtGenre.Text,
                        OS = txtExtra1.Text,
                        HasAds = bool.Parse(txtExtra2.Text)
                    };

                case "OnlineGame":
                    return new OnlineGame
                    {
                        Name = txtName.Text,
                        Price = double.Parse(txtPrice.Text),
                        Genre = txtGenre.Text,
                        ServerCount = int.Parse(txtExtra1.Text),
                        Subscription = bool.Parse(txtExtra2.Text)
                    };

                case "IndieGame":
                    return new IndieGame
                    {
                        Name = txtName.Text,
                        Price = double.Parse(txtPrice.Text),
                        Genre = txtGenre.Text,
                        Studio = txtExtra1.Text,
                        TeamSize = int.Parse(txtExtra2.Text)
                    };

                case "VRGame":
                    return new VRGame
                    {
                        Name = txtName.Text,
                        Price = double.Parse(txtPrice.Text),
                        Genre = txtGenre.Text,
                        VRDevice = txtExtra1.Text,
                        RequiresControllers = bool.Parse(txtExtra2.Text)
                    };

                default:
                    throw new Exception();
            }
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
                items[listBox1.SelectedIndex] = CreateFromInput();
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
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK)
                JsonSerializerService.Save("games.json", items);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
                items = JsonSerializerService.Load("games.json");
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

        // ✅ ВАЖНЫЙ МЕТОД (исправляет твою ошибку)
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
                return;

            var item = items[listBox1.SelectedIndex];

            txtName.Text = item.Name;

            if (item is PCGame pc)
            {
                cmbType.SelectedItem = "PCGame";
                txtPrice.Text = pc.Price.ToString();
                txtGenre.Text = pc.Genre;
                txtExtra1.Text = pc.Platform;
                txtExtra2.Text = pc.RequiredRAM.ToString();
            }
            else if (item is ConsoleGame cg)
            {
                cmbType.SelectedItem = "ConsoleGame";
                txtPrice.Text = cg.Price.ToString();
                txtGenre.Text = cg.Genre;
                txtExtra1.Text = cg.Console;
                txtExtra2.Text = cg.MaxPlayers.ToString();
            }
            else if (item is MobileGame mg)
            {
                cmbType.SelectedItem = "MobileGame";
                txtPrice.Text = mg.Price.ToString();
                txtGenre.Text = mg.Genre;
                txtExtra1.Text = mg.OS;
                txtExtra2.Text = mg.HasAds.ToString();
            }
            else if (item is OnlineGame og)
            {
                cmbType.SelectedItem = "OnlineGame";
                txtPrice.Text = og.Price.ToString();
                txtGenre.Text = og.Genre;
                txtExtra1.Text = og.ServerCount.ToString();
                txtExtra2.Text = og.Subscription.ToString();
            }
            else if (item is IndieGame ig)
            {
                cmbType.SelectedItem = "IndieGame";
                txtPrice.Text = ig.Price.ToString();
                txtGenre.Text = ig.Genre;
                txtExtra1.Text = ig.Studio;
                txtExtra2.Text = ig.TeamSize.ToString();
            }
            else if (item is VRGame vr)
            {
                cmbType.SelectedItem = "VRGame";
                txtPrice.Text = vr.Price.ToString();
                txtGenre.Text = vr.Genre;
                txtExtra1.Text = vr.VRDevice;
                txtExtra2.Text = vr.RequiresControllers.ToString();
            }
        }
    }
}