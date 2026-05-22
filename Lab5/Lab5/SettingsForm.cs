using System;
using System.Drawing;
using System.Windows.Forms;
using Lab5.Services;

namespace Lab5
{
    /// <summary>
    /// Settings dialog: enable/disable XML persistence plugins loaded from the Plugins folder.
    /// </summary>
    public partial class SettingsForm : Form
    {
        private readonly FlowLayoutPanel pluginPanel;

        public SettingsForm()
        {
            Text = "Settings — XML persistence plugins";
            Size = new Size(520, 400);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            var description = new Label
            {
                Text = "Enable plugins to transform XML before save and after load (variant 4).",
                AutoSize = false,
                Size = new Size(480, 40),
                Location = new Point(12, 12)
            };

            pluginPanel = new FlowLayoutPanel
            {
                Location = new Point(12, 58),
                Size = new Size(480, 240),
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };

            var btnReload = new Button
            {
                Text = "Reload from Plugins folder",
                Location = new Point(12, 308),
                Size = new Size(200, 28)
            };
            btnReload.Click += BtnReload_Click;

            var btnLoadFile = new Button
            {
                Text = "Load plugin from file...",
                Location = new Point(220, 308),
                Size = new Size(180, 28)
            };
            btnLoadFile.Click += BtnLoadFile_Click;

            var btnClose = new Button
            {
                Text = "Close",
                Location = new Point(410, 308),
                Size = new Size(82, 28),
                DialogResult = DialogResult.OK
            };

            Controls.Add(description);
            Controls.Add(pluginPanel);
            Controls.Add(btnReload);
            Controls.Add(btnLoadFile);
            Controls.Add(btnClose);
            AcceptButton = btnClose;

            PersistencePluginRegistry.Changed += OnRegistryChanged;
            RefreshPluginList();
        }

        /// <summary>Rebuilds checkboxes for each registered persistence plugin.</summary>
        private void RefreshPluginList()
        {
            pluginPanel.Controls.Clear();

            if (PersistencePluginRegistry.Entries.Count == 0)
            {
                pluginPanel.Controls.Add(new Label
                {
                    Text = "No persistence plugins loaded. Place DLLs in the Plugins folder or use \"Load plugin from file\".",
                    AutoSize = true,
                    MaximumSize = new Size(460, 0)
                });
                return;
            }

            foreach (RegisteredPersistencePlugin entry in PersistencePluginRegistry.Entries)
            {
                var row = new Panel { Size = new Size(460, 52), Margin = new Padding(0, 0, 0, 4) };

                var checkBox = new CheckBox
                {
                    Text = entry.Plugin.DisplayName,
                    Checked = entry.Enabled,
                    AutoSize = true,
                    Location = new Point(0, 0),
                    Tag = entry.Plugin.PluginId
                };
                checkBox.CheckedChanged += PluginCheckBox_CheckedChanged;

                var info = new Label
                {
                    Text = entry.Plugin.Description + "  [" + entry.Plugin.PluginId + "]",
                    AutoSize = false,
                    Size = new Size(440, 28),
                    Location = new Point(24, 22),
                    ForeColor = Color.DimGray
                };

                row.Controls.Add(checkBox);
                row.Controls.Add(info);
                pluginPanel.Controls.Add(row);
            }
        }

        /// <summary>Updates enabled flag when the user toggles a plugin checkbox.</summary>
        private void PluginCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox == null || checkBox.Tag == null)
                return;

            PersistencePluginRegistry.SetEnabled(checkBox.Tag.ToString(), checkBox.Checked);
        }

        /// <summary>Rescans the Plugins directory for persistence plugin DLLs.</summary>
        private void BtnReload_Click(object sender, EventArgs e)
        {
            PersistencePluginLoader.ReloadAll();
            RefreshPluginList();
        }

        /// <summary>Loads a single persistence plugin chosen via OpenFileDialog.</summary>
        private void BtnLoadFile_Click(object sender, EventArgs e)
        {
            PersistencePluginLoader.LoadFromFileDialog();
            RefreshPluginList();
        }

        /// <summary>Registry change handler — keeps UI in sync when plugins are added elsewhere.</summary>
        private void OnRegistryChanged()
        {
            if (IsDisposed)
                return;

            RefreshPluginList();
        }

        /// <summary>Unsubscribes from registry events when the dialog closes.</summary>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            PersistencePluginRegistry.Changed -= OnRegistryChanged;
            base.OnFormClosed(e);
        }
    }
}
