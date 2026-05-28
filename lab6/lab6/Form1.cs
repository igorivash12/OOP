using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Lab6.Core;
using Lab6.Core.Patterns;
using Lab6.Core.Plugins;

namespace lab6
{
    public partial class Form1 : Form
    {
        private readonly MediaItemList mediaList = new MediaItemList();
        // Singleton pattern: the pipeline lives inside the global PluginManager
        // so that any subsystem (UI dialogs, future schedulers, tests) shares
        // the same plugin state without having to thread a reference around.
        private ProcessingPipeline Pipeline => PluginManager.Instance.Pipeline;
        private PluginLoader loader;

        public Form1()
        {
            InitializeComponent();
            InitializePluginLoader();
            LoadPluginsFromDefaultFolder();
            InitializeDomainBindings();
        }

        /// <summary>Reads the trusted public key once and constructs the loader.</summary>
        private void InitializePluginLoader()
        {
            var exeDir = Path.GetDirectoryName(Application.ExecutablePath);
            var keyPath = Path.Combine(exeDir, "trusted_public_key.xml");
            if (!File.Exists(keyPath))
            {
                MessageBox.Show(this,
                    "Trusted public key not found next to the executable.\nPlugins will not be loaded.",
                    "Plugin loader", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            loader = new PluginLoader(File.ReadAllText(keyPath));
        }

        /// <summary>
        /// Auto-discovers signed plugins from the Plugins folder next to the
        /// executable. Both type-providing and processing plugins are picked
        /// up by the same scan.
        /// </summary>
        private void LoadPluginsFromDefaultFolder()
        {
            if (loader == null) return;
            var exeDir = Path.GetDirectoryName(Application.ExecutablePath);
            var pluginsDir = Path.Combine(exeDir, "Plugins");
            var report = new StringBuilder();

            foreach (var entry in loader.LoadFromFolder(pluginsDir))
            {
                report.AppendLine($"{entry.FileName}: {entry.Message}");
                RegisterEntry(entry);
            }

            if (report.Length > 0) lblPluginReport.Text = report.ToString().TrimEnd();
        }

        /// <summary>Wires a verified plugin into the registry / pipeline.</summary>
        private void RegisterEntry(PluginLoadEntry entry)
        {
            if (!entry.Loaded) return;

            if (entry.TypePlugin != null)
            {
                foreach (var d in entry.TypePlugin.GetTypeDescriptors())
                    MediaItemTypeRegistry.Register(d);
                foreach (var t in entry.TypePlugin.GetItemTypes())
                    MediaItemList.RegisterPluginType(t);
            }

            if (entry.ProcessingPlugin != null)
                PluginManager.Instance.RegisterProcessingPlugin(entry.ProcessingPlugin);
        }

        private void InitializeDomainBindings()
        {
            listItems.DataSource = mediaList.Items;
            cmbType.DataSource = MediaItemTypeRegistry.Types.ToList();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cmbType.SelectedItem is MediaItemTypeDescriptor descriptor)
            {
                var item = descriptor.Create(txtTitle.Text, txtCreator.Text, (int)nudYear.Value);
                mediaList.Add(item);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listItems.SelectedItem is MediaItem item) { mediaList.Remove(item); ClearEditor(); }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (listItems.SelectedItem is MediaItem item)
            {
                item.Title = txtTitle.Text;
                item.Creator = txtCreator.Text;
                item.Year = (int)nudYear.Value;
                mediaList.Items.ResetItem(listItems.SelectedIndex);
                UpdateDetails(item);
            }
        }

        /// <summary>
        /// Serializes the list, runs it through every enabled processing
        /// plugin, and writes the resulting bytes to a file.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "BSON files (*.bson)|*.bson|All files (*.*)|*.*";
                dialog.Title = "Save media library";
                dialog.DefaultExt = "bson";
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    var bytes = mediaList.SerializeToBson();
                    bytes = Pipeline.RunOnSave(bytes);
                    File.WriteAllBytes(dialog.FileName, bytes);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Save failed: " + ex.Message, "Save", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>Reads bytes, reverses the pipeline, deserializes BSON.</summary>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "BSON files (*.bson)|*.bson|All files (*.*)|*.*";
                dialog.Title = "Load media library";
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    var bytes = File.ReadAllBytes(dialog.FileName);
                    bytes = Pipeline.RunOnLoad(bytes);
                    mediaList.DeserializeFromBson(bytes);
                    ClearEditor();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Load failed: " + ex.Message, "Load", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Opens a tabbed settings dialog that lets the user toggle and
        /// configure every loaded processing plugin.
        /// </summary>
        private void miPluginSettings_Click(object sender, EventArgs e)
        {
            using (var dlg = new Form
            {
                Text = "Plugin settings",
                Width = 480,
                Height = 360,
                StartPosition = FormStartPosition.CenterParent
            })
            {
                var tabs = new TabControl { Dock = DockStyle.Fill };
                foreach (var plugin in Pipeline.Plugins)
                {
                    var page = new TabPage(plugin.Name);
                    var ctrl = plugin.BuildSettingsControl();
                    if (ctrl != null) { ctrl.Dock = DockStyle.Fill; page.Controls.Add(ctrl); }
                    else page.Controls.Add(new Label { Text = plugin.Description, Dock = DockStyle.Fill });
                    tabs.TabPages.Add(page);
                }
                if (tabs.TabPages.Count == 0)
                    tabs.TabPages.Add(new TabPage("No plugins") { Controls = { new Label { Text = "No processing plugins loaded.", Dock = DockStyle.Fill } } });
                dlg.Controls.Add(tabs);
                dlg.ShowDialog(this);
            }
        }

        /// <summary>Lets the user pick a single plugin DLL and load it manually.</summary>
        private void miLoadPlugin_Click(object sender, EventArgs e)
        {
            if (loader == null) return;
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Plugin DLLs (*.dll)|*.dll";
                dialog.Title = "Select a signed plugin DLL";
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                var entry = loader.LoadFile(dialog.FileName);
                MessageBox.Show(this, entry.Message, "Plugin", MessageBoxButtons.OK,
                    entry.Loaded ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                RegisterEntry(entry);
                cmbType.DataSource = MediaItemTypeRegistry.Types.ToList();
            }
        }

        /// <summary>
        /// Shows the log produced by the Decorator-wrapped plugins. Each save
        /// and load adds one line per active plugin, so the user can confirm
        /// the pipeline order and inspect compression / encryption ratios.
        /// </summary>
        private void miViewLog_Click(object sender, EventArgs e)
        {
            var text = string.Join("\r\n", PluginManager.Instance.Log);
            if (string.IsNullOrEmpty(text)) text = "(no operations logged yet)";
            using (var dlg = new Form { Text = "Processing log", Width = 600, Height = 400, StartPosition = FormStartPosition.CenterParent })
            {
                dlg.Controls.Add(new TextBox
                {
                    Dock = DockStyle.Fill,
                    Multiline = true,
                    ReadOnly = true,
                    ScrollBars = ScrollBars.Vertical,
                    Text = text
                });
                dlg.ShowDialog(this);
            }
        }

        private void listItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listItems.SelectedItem is MediaItem item)
            {
                txtTitle.Text = item.Title;
                txtCreator.Text = item.Creator;
                nudYear.Value = item.Year >= nudYear.Minimum && item.Year <= nudYear.Maximum
                    ? item.Year : nudYear.Minimum;
                UpdateDetails(item);
            }
            else ClearEditor();
        }

        private void ClearEditor()
        {
            txtTitle.Text = string.Empty;
            txtCreator.Text = string.Empty;
            nudYear.Value = nudYear.Minimum;
            txtDetails.Text = string.Empty;
        }

        private void UpdateDetails(MediaItem item)
        {
            txtDetails.Text = $"{item.TypeName}\r\n{item.GetDetails()}";
        }
    }
}
