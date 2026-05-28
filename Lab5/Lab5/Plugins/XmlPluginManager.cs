using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Lab5.Plugins
{
    /// <summary>
    /// Central registry and runtime manager for IXmlPlugin instances.
    /// Handles discovery via reflection, enable/disable state, ordered pipeline execution,
    /// and dynamic DLL loading from the Plugins folder or via OpenFileDialog.
    /// </summary>
    public static class XmlPluginManager
    {
        // ── Internal state ────────────────────────────────────────────────────

        /// <summary>All discovered XML plugins, keyed by Name.</summary>
        private static readonly Dictionary<string, IXmlPlugin> Plugins =
            new Dictionary<string, IXmlPlugin>(StringComparer.OrdinalIgnoreCase);

        /// <summary>Tracks which plugins are currently enabled by the user.</summary>
        private static readonly HashSet<string> EnabledPlugins =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>Tracks assemblies already loaded to avoid double-registration.</summary>
        private static readonly HashSet<string> LoadedPaths =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // ── Public discovery API ──────────────────────────────────────────────

        /// <summary>
        /// Scans the Plugins directory for DLLs implementing IXmlPlugin and registers them.
        /// Safe to call multiple times — already-loaded paths are skipped.
        /// </summary>
        /// <param name="pluginsDirectory">Absolute path to the Plugins folder.</param>
        public static void LoadFromDirectory(string pluginsDirectory)
        {
            if (!Directory.Exists(pluginsDirectory))
                return;

            foreach (string dll in Directory.GetFiles(pluginsDirectory, "*.dll"))
                LoadAssembly(dll);
        }

        /// <summary>
        /// Opens an OpenFileDialog allowing the user to select a plugin DLL manually.
        /// Loads and registers all IXmlPlugin types found inside the selected file.
        /// </summary>
        /// <returns>Number of new XML plugins registered from the selected file.</returns>
        public static int LoadFromDialog()
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Title = "Load XML Plugin DLL";
                dlg.Filter = "DLL files (*.dll)|*.dll|All files (*.*)|*.*";
                dlg.Multiselect = false;

                if (dlg.ShowDialog() != DialogResult.OK)
                    return 0;

                return LoadAssembly(dlg.FileName);
            }
        }

        // ── Pipeline execution ────────────────────────────────────────────────

        /// <summary>
        /// Passes the XML string through every enabled plugin's ProcessBeforeSave in registration order.
        /// </summary>
        /// <param name="xml">Original XML content.</param>
        /// <returns>Transformed XML content after all active plugins have processed it.</returns>
        public static string RunBeforeSave(string xml)
        {
            foreach (IXmlPlugin plugin in GetEnabledPlugins())
            {
                try
                {
                    xml = plugin.ProcessBeforeSave(xml);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        string.Format("Plugin '{0}' failed during BeforeSave:\n{1}", plugin.Name, ex.Message),
                        "Plugin Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            return xml;
        }

        /// <summary>
        /// Passes the XML string through every enabled plugin's ProcessAfterLoad in reverse order
        /// so that reversible transformations are correctly unwound.
        /// </summary>
        /// <param name="xml">XML content read from disk.</param>
        /// <returns>Transformed XML content after all active plugins have processed it.</returns>
        public static string RunAfterLoad(string xml)
        {
            foreach (IXmlPlugin plugin in GetEnabledPlugins().Reverse())
            {
                try
                {
                    xml = plugin.ProcessAfterLoad(xml);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        string.Format("Plugin '{0}' failed during AfterLoad:\n{1}", plugin.Name, ex.Message),
                        "Plugin Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            return xml;
        }

        // ── Enable / disable ──────────────────────────────────────────────────

        /// <summary>Enables a plugin by name so that it participates in the pipeline.</summary>
        public static void Enable(string name)
        {
            if (Plugins.ContainsKey(name))
                EnabledPlugins.Add(name);
        }

        /// <summary>Disables a plugin by name so that it is skipped during pipeline execution.</summary>
        public static void Disable(string name)
        {
            EnabledPlugins.Remove(name);
        }

        /// <summary>Returns true when the specified plugin is currently enabled.</summary>
        public static bool IsEnabled(string name)
        {
            return EnabledPlugins.Contains(name);
        }

        // ── Registry queries ──────────────────────────────────────────────────

        /// <summary>Returns a snapshot of all registered plugins in registration order.</summary>
        public static IReadOnlyList<IXmlPlugin> GetAllPlugins()
        {
            return Plugins.Values.ToList();
        }

        /// <summary>Returns a snapshot of currently enabled plugins in registration order.</summary>
        public static IReadOnlyList<IXmlPlugin> GetEnabledPlugins()
        {
            return Plugins.Values
                .Where(p => EnabledPlugins.Contains(p.Name))
                .ToList();
        }

        // ── Internal helpers ──────────────────────────────────────────────────

        /// <summary>
        /// Loads the assembly at the given path and registers all IXmlPlugin implementations found.
        /// Skips assemblies that have already been loaded.
        /// </summary>
        /// <returns>Count of new plugin types registered.</returns>
        private static int LoadAssembly(string dllPath)
        {
            if (LoadedPaths.Contains(dllPath))
                return 0;

            try
            {
                Assembly assembly = Assembly.LoadFrom(dllPath);
                int count = RegisterPluginsFromAssembly(assembly);
                LoadedPaths.Add(dllPath);
                return count;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format("Failed to load XML plugin DLL '{0}':\n{1}",
                        Path.GetFileName(dllPath), ex.Message),
                    "Plugin Load Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return 0;
            }
        }

        /// <summary>
        /// Instantiates and registers every concrete IXmlPlugin type in the given assembly.
        /// </summary>
        private static int RegisterPluginsFromAssembly(Assembly assembly)
        {
            Type xmlPluginInterface = typeof(IXmlPlugin);
            int count = 0;

            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsAbstract || type.IsInterface)
                    continue;

                if (!xmlPluginInterface.IsAssignableFrom(type))
                    continue;

                try
                {
                    var instance = (IXmlPlugin)Activator.CreateInstance(type);

                    if (!Plugins.ContainsKey(instance.Name))
                    {
                        Plugins[instance.Name] = instance;
                        count++;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        string.Format("Could not instantiate XML plugin type '{0}':\n{1}",
                            type.FullName, ex.Message),
                        "Plugin Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }

            return count;
        }
    }
}
