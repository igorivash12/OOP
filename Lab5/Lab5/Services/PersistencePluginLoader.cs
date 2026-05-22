using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Lab5.Plugins;

namespace Lab5.Services
{
    /// <summary>
    /// Discovers and loads IPersistencePlugin implementations from the Plugins folder or via UI.
    /// </summary>
    public static class PersistencePluginLoader
    {
        private static readonly HashSet<string> LoadedAssemblyPaths =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Loads all persistence plugins from the Plugins directory next to the executable.
        /// </summary>
        public static void Initialize()
        {
            string pluginsDirectory = PluginLoader.GetPluginsDirectory();
            PluginLoader.EnsurePluginsDirectoryExists(pluginsDirectory);
            LoadAllFromDirectory(pluginsDirectory, showSummary: false);
        }

        /// <summary>
        /// Opens a file dialog so the user can pick a plugin DLL to load manually.
        /// </summary>
        /// <returns>Number of new persistence plugins registered from the chosen file.</returns>
        public static int LoadFromFileDialog()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "Select persistence plugin assembly";
                dialog.Filter = "Plugin assemblies (*.dll)|*.dll|All files (*.*)|*.*";
                dialog.InitialDirectory = PluginLoader.GetPluginsDirectory();

                if (dialog.ShowDialog() != DialogResult.OK)
                    return 0;

                return LoadAssemblyFile(dialog.FileName, showErrors: true);
            }
        }

        /// <summary>Reloads persistence plugins from every DLL in the Plugins folder.</summary>
        public static int ReloadAll()
        {
            string pluginsDirectory = PluginLoader.GetPluginsDirectory();
            PluginLoader.EnsurePluginsDirectoryExists(pluginsDirectory);
            return LoadAllFromDirectory(pluginsDirectory, showSummary: true);
        }

        /// <summary>Loads each suitable DLL from the given directory.</summary>
        private static int LoadAllFromDirectory(string pluginsDirectory, bool showSummary)
        {
            int total = 0;

            foreach (string dllPath in Directory.GetFiles(pluginsDirectory, "*.dll"))
            {
                if (PluginLoader.ShouldSkipHostOrDependency(dllPath))
                    continue;

                total += LoadAssemblyFile(dllPath, showErrors: false);
            }

            if (showSummary)
            {
                MessageBox.Show(
                    string.Format("Persistence plugins reload complete. Registered {0} plugin(s).", total),
                    "Persistence plugins",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            return total;
        }

        /// <summary>Loads one assembly and registers all IPersistencePlugin types found inside.</summary>
        private static int LoadAssemblyFile(string dllPath, bool showErrors)
        {
            if (LoadedAssemblyPaths.Contains(dllPath))
                return 0;

            try
            {
                Assembly assembly = Assembly.LoadFrom(dllPath);
                int count = 0;

                foreach (IPersistencePlugin plugin in FindPersistencePluginsInAssembly(assembly))
                {
                    if (PersistencePluginRegistry.Register(plugin, dllPath))
                        count++;
                }

                LoadedAssemblyPaths.Add(dllPath);
                return count;
            }
            catch (Exception ex)
            {
                if (showErrors)
                {
                    MessageBox.Show(
                        "Failed to load persistence plugin: " + Path.GetFileName(dllPath)
                        + Environment.NewLine + ex.Message,
                        "Persistence plugin error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

                return 0;
            }
        }

        /// <summary>Instantiates every concrete IPersistencePlugin type in the assembly.</summary>
        private static IEnumerable<IPersistencePlugin> FindPersistencePluginsInAssembly(Assembly assembly)
        {
            Type pluginInterface = typeof(IPersistencePlugin);
            var result = new List<IPersistencePlugin>();

            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsAbstract || type.IsInterface)
                    continue;

                if (!pluginInterface.IsAssignableFrom(type))
                    continue;

                var instance = Activator.CreateInstance(type) as IPersistencePlugin;
                if (instance != null)
                    result.Add(instance);
            }

            return result;
        }
    }
}
