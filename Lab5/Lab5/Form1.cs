using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Lab5.Models;
using Lab5.Plugins;
using Lab5.Services;

namespace Lab5
{
    /// <summary>
    /// Main window: CRUD for game hierarchy, XML persistence with plugin transforms, game-type plugins.
    /// </summary>
    public partial class Form1 : Form
    {
        private List<BaseEntity> items = new List<BaseEntity>();
        private MenuStrip mainMenu;

        public Form1()
        {
            InitializeComponent();
            BuildSettingsMenu();
            OffsetControlsBelowMenu();
            RefreshTypeComboBox();
            RefreshPersistencePluginMenuItems();
            PersistencePluginRegistry.Changed += OnPersistencePluginsChanged;
        }

        /// <summary>Updates labels for the two dynamic extra fields (plugin-specific).</summary>
        public void SetLabels(string l1, string l2)
        {
            lblExtra1.Text = l1;
            lblExtra2.Text = l2;
        }

        /// <summary>
        /// Fills the type ComboBox from GameFactory (built-in + loaded game plugins).
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

        /// <summary>Moves form controls down so they do not overlap the menu strip.</summary>
        private void OffsetControlsBelowMenu()
        {
            int offset = mainMenu.PreferredSize.Height;

            foreach (Control control in Controls)
            {
                if (control == mainMenu)
                    continue;

                control.Top += offset;
            }

            ClientSize = new System.Drawing.Size(ClientSize.Width, ClientSize.Height + offset);
        }

        /// <summary>Creates the Settings menu with persistence-plugin actions.</summary>
        private void BuildSettingsMenu()
        {
            mainMenu = new MenuStrip();

            var settingsMenu = new ToolStripMenuItem("Settings");
            var openSettings = new ToolStripMenuItem("Persistence plugins...");
            openSettings.Click += OpenSettings_Click;
            settingsMenu.DropDownItems.Add(openSettings);

            settingsMenu.DropDownItems.Add(new ToolStripSeparator());

            var loadGamePlugin = new ToolStripMenuItem("Load game plugin from file...");
            loadGamePlugin.Click += LoadGamePlugin_Click;
            settingsMenu.DropDownItems.Add(loadGamePlugin);

            var loadPersistencePlugin = new ToolStripMenuItem("Load persistence plugin from file...");
            loadPersistencePlugin.Click += LoadPersistencePlugin_Click;
            settingsMenu.DropDownItems.Add(loadPersistencePlugin);

            settingsMenu.DropDownItems.Add(new ToolStripSeparator());

            var pluginActionsRoot = new ToolStripMenuItem("Plugin actions");
            pluginActionsRoot.Name = "pluginActionsRoot";
            settingsMenu.DropDownItems.Add(pluginActionsRoot);

            mainMenu.Items.Add(settingsMenu);
            MainMenuStrip = mainMenu;
            Controls.Add(mainMenu);
        }

        /// <summary>Adds per-plugin menu items reported by loaded persistence plugins.</summary>
        private void RefreshPersistencePluginMenuItems()
        {
            ToolStripMenuItem root = FindPluginActionsMenu();
            if (root == null)
                return;

            root.DropDownItems.Clear();

            if (PersistencePluginRegistry.Entries.Count == 0)
            {
                var empty = new ToolStripMenuItem("(no persistence plugins loaded)");
                empty.Enabled = false;
                root.DropDownItems.Add(empty);
                return;
            }

            foreach (RegisteredPersistencePlugin entry in PersistencePluginRegistry.Entries)
            {
                ToolStripItem[] items = entry.Plugin.GetSettingsMenuItems();
                if (items == null || items.Length == 0)
                    continue;

                var group = new ToolStripMenuItem(entry.Plugin.DisplayName);
                foreach (ToolStripItem item in items)
                    group.DropDownItems.Add(item);

                root.DropDownItems.Add(group);
            }

            if (root.DropDownItems.Count == 0)
            {
                var none = new ToolStripMenuItem("(selected plugins have no extra actions)");
                none.Enabled = false;
                root.DropDownItems.Add(none);
            }
        }

        /// <summary>Locates the dynamic Plugin actions submenu under Settings.</summary>
        private ToolStripMenuItem FindPluginActionsMenu()
        {
            if (mainMenu == null)
                return null;

            foreach (ToolStripItem top in mainMenu.Items)
            {
                var settings = top as ToolStripMenuItem;
                if (settings == null || settings.Text != "Settings")
                    continue;

                foreach (ToolStripItem child in settings.DropDownItems)
                {
                    if (child.Name == "pluginActionsRoot")
                        return child as ToolStripMenuItem;
                }
            }

            return null;
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

        /// <summary>Serializes to XML, runs persistence plugins, writes games.xml next to the executable.</summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string path = XmlPersistenceService.ResolveDataFilePath(XmlPersistenceService.DefaultFileName);
                XmlPersistenceService.Save(XmlPersistenceService.DefaultFileName, items);
                MessageBox.Show(
                    string.Format("Saved {0} item(s) to:\n{1}", items.Count, path),
                    "Save",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Reads games.xml, runs reverse persistence plugins, deserializes entities.</summary>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                string path = XmlPersistenceService.ResolveDataFilePath(XmlPersistenceService.DefaultFileName);

                if (!XmlPersistenceService.DefaultDataFileExists())
                {
                    MessageBox.Show(
                        "Data file not found:\n" + path + "\n\nSave data first.",
                        "Load",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                items = XmlPersistenceService.Load(XmlPersistenceService.DefaultFileName);
                UpdateList();

                MessageBox.Show(
                    string.Format("Loaded {0} item(s) from:\n{1}", items.Count, path),
                    "Load",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Rescans Plugins folder for game-type modules and refreshes the type list.</summary>
        private void btnReloadPlugins_Click(object sender, EventArgs e)
        {
            PluginLoader.ReloadAll();
            RefreshTypeComboBox();
        }

        /// <summary>Opens the settings dialog for XML persistence plugins.</summary>
        private void OpenSettings_Click(object sender, EventArgs e)
        {
            using (var form = new SettingsForm())
                form.ShowDialog(this);
        }

        /// <summary>Loads a game plugin DLL selected by the user.</summary>
        private void LoadGamePlugin_Click(object sender, EventArgs e)
        {
            int count = PluginLoader.LoadFromFileDialog();
            if (count > 0)
                RefreshTypeComboBox();
        }

        /// <summary>Loads a persistence plugin DLL selected by the user.</summary>
        private void LoadPersistencePlugin_Click(object sender, EventArgs e)
        {
            PersistencePluginLoader.LoadFromFileDialog();
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

        /// <summary>Keeps plugin-specific Settings menu entries up to date.</summary>
        private void OnPersistencePluginsChanged()
        {
            RefreshPersistencePluginMenuItems();
        }
    }
}
