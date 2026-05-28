using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Lab5.Plugins;

namespace Lab5
{
    /// <summary>
    /// Modal dialog that lists all registered IXmlPlugin instances and lets the user
    /// enable/disable them individually, view their descriptions, and load additional DLLs.
    /// </summary>
    public partial class FormPluginManager : Form
    {
        public FormPluginManager()
        {
            InitializeComponent();
            PopulateList();
        }

        // ── List population ───────────────────────────────────────────────────

        /// <summary>
        /// Fills the checked list box with all known XML plugins,
        /// checking each one that is currently enabled.
        /// </summary>
        private void PopulateList()
        {
            clbPlugins.Items.Clear();

            foreach (IXmlPlugin plugin in XmlPluginManager.GetAllPlugins())
            {
                bool enabled = XmlPluginManager.IsEnabled(plugin.Name);
                clbPlugins.Items.Add(plugin.Name, enabled);
            }

            UpdateDescription();
        }

        // ── Event handlers ────────────────────────────────────────────────────

        /// <summary>
        /// When the user checks or unchecks a plugin, enable or disable it in the manager.
        /// </summary>
        private void clbPlugins_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string name = clbPlugins.Items[e.Index].ToString();

            if (e.NewValue == CheckState.Checked)
                XmlPluginManager.Enable(name);
            else
                XmlPluginManager.Disable(name);
        }

        /// <summary>Updates the description text area when the selected plugin changes.</summary>
        private void clbPlugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDescription();
        }

        /// <summary>Refreshes the description panel from the currently selected plugin.</summary>
        private void UpdateDescription()
        {
            if (clbPlugins.SelectedIndex < 0)
            {
                txtDescription.Text = string.Empty;
                return;
            }

            string name = clbPlugins.Items[clbPlugins.SelectedIndex].ToString();
            IXmlPlugin plugin = FindPlugin(name);

            if (plugin != null)
                txtDescription.Text = string.Format(
                    "Name: {0}\r\n\r\nDescription:\r\n{1}",
                    plugin.Name,
                    plugin.Description);
        }

        /// <summary>Loads an XML plugin DLL via OpenFileDialog and refreshes the list.</summary>
        private void btnLoadDll_Click(object sender, EventArgs e)
        {
            int count = XmlPluginManager.LoadFromDialog();
            if (count > 0)
            {
                MessageBox.Show(
                    string.Format("Loaded {0} XML plugin(s).", count),
                    "Load DLL",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                PopulateList();
            }
        }

        /// <summary>Enables all listed plugins at once.</summary>
        private void btnEnableAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbPlugins.Items.Count; i++)
            {
                clbPlugins.SetItemChecked(i, true);
                XmlPluginManager.Enable(clbPlugins.Items[i].ToString());
            }
        }

        /// <summary>Disables all listed plugins at once.</summary>
        private void btnDisableAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbPlugins.Items.Count; i++)
            {
                clbPlugins.SetItemChecked(i, false);
                XmlPluginManager.Disable(clbPlugins.Items[i].ToString());
            }
        }

        /// <summary>Closes the dialog.</summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        /// <summary>Finds a plugin by name from the manager's registry.</summary>
        private static IXmlPlugin FindPlugin(string name)
        {
            foreach (IXmlPlugin p in XmlPluginManager.GetAllPlugins())
            {
                if (string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase))
                    return p;
            }
            return null;
        }
    }
}
