using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Lab4.Models;
using Lab4.Services;

namespace Lab4
{
    /// <summary>
    /// Main window: CRUD for game hierarchy objects, JSON persistence, plugin-driven types.
    /// </summary>
    public partial class Form1 : Form
    {
        private List<BaseEntity> items = new List<BaseEntity>();

        public Form1()
        {
            InitializeComponent();
            RefreshTypeComboBox();
        }

        /// <summary>Updates labels for the two dynamic extra fields (plugin-specific).</summary>
        public void SetLabels(string l1, string l2)
        {
            lblExtra1.Text = l1;
            lblExtra2.Text = l2;
        }

        /// <summary>
        /// Fills the type ComboBox from GameFactory (built-in + loaded plugins).
        /// </summary>
        public void RefreshTypeComboBox()
        {
            string previous = cmbType.SelectedItem as string;

            cmbType.Items.Clear();
            foreach (string typeKey in GameFactory.GetAvailableTypeKeys())
                cmbType.Items.Add(typeKey);

            if (!string.IsNullOrEmpty(previous) && cmbType.Items.Contains(previous))
                cmbType.SelectedItem = previous;
            else if (cmbType.Items.Count > 0)
                cmbType.SelectedIndex = 0;
        }

        /// <summary>Creates an entity via factory and reads field values from the form.</summary>
        private BaseEntity CreateFromInput()
        {
            string typeKey = GameFactory.ResolveTypeKey(cmbType.SelectedItem.ToString());
            var obj = GameFactory.Create(typeKey);
            obj.FillFromForm(this);
            return obj;
        }

        /// <summary>Adds a new item built from the current form input.</summary>
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

        /// <summary>Replaces the selected list item with data from the form.</summary>
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

        /// <summary>Removes the currently selected item from the collection.</summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                items.RemoveAt(listBox1.SelectedIndex);
                UpdateList();
            }
        }

        /// <summary>Serializes the collection to games.json.</summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            JsonSerializerService.Save("games.json", items);
            MessageBox.Show("Saved");
        }

        /// <summary>Deserializes items from games.json (plugin assemblies must already be loaded).</summary>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            items = JsonSerializerService.Load("games.json");
            UpdateList();
            MessageBox.Show("Loaded");
        }

        /// <summary>Scans the Plugins folder again and refreshes the type ComboBox.</summary>
        private void btnReloadPlugins_Click(object sender, EventArgs e)
        {
            PluginLoader.ReloadAll();
            RefreshTypeComboBox();
        }

        /// <summary>Redraws the list box with entity names.</summary>
        private void UpdateList()
        {
            listBox1.Items.Clear();
            foreach (var item in items)
                listBox1.Items.Add(item.Name);
        }

        /// <summary>When selection changes, loads entity data into the form.</summary>
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
                return;

            var item = items[listBox1.SelectedIndex];

            cmbType.SelectedItem = item.GetTypeName();
            item.FillForm(this);
        }

        /// <summary>When type changes, updates dynamic field labels for that type.</summary>
        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbType.SelectedItem == null)
                return;

            string typeKey = GameFactory.ResolveTypeKey(cmbType.SelectedItem.ToString());
            var obj = GameFactory.Create(typeKey);
            obj.FillForm(this);
        }
    }
}
