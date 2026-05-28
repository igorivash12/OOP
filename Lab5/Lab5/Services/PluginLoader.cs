using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Lab5.Models;
using Lab5.Plugins;

namespace Lab5.Services
{
    /// <summary>
    /// Discovers and loads plugin assemblies from the Plugins folder or a user-selected path.
    /// Handles both IGamePlugin (game-type registration) and IXmlPlugin (XML processing pipeline).
    /// </summary>
    public static class PluginLoader
    {
        private static readonly HashSet<string> LoadedAssemblyPaths =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>Folder name relative to the executable directory.</summary>
        public const string PluginsFolderName = "Plugins";

        // ── Startup ───────────────────────────────────────────────────────────

        /// <summary>
        /// Initializes plugin loading. Call once at application startup before using GameFactory.
        /// Also loads IXmlPlugin implementations into XmlPluginManager.
        /// </summary>
        /// <param name="args">Optional command-line arguments; first arg may name a specific plugin DLL.</param>
        public static void Initialize(string[] args)
        {
            string pluginsDirectory = GetPluginsDirectory();
            EnsureDirectoryExists(pluginsDirectory);

            if (args != null && args.Length > 0 && !string.IsNullOrWhiteSpace(args[0]))
            {
                LoadPluginFromArgument(args[0].Trim(), pluginsDirectory);
            }
            else
            {
                LoadAllPluginsFromDirectory(pluginsDirectory, showSummary: false);
            }

            // Separately register XML plugins from the same directory
            XmlPluginManager.LoadFromDirectory(pluginsDirectory);
        }

        /// <summary>Returns the absolute path to the Plugins directory next to the executable.</summary>
        public static string GetPluginsDirectory()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PluginsFolderName);
        }

        /// <summary>
        /// Reloads every plugin DLL from the Plugins folder (used by the UI reload button).
        /// Returns total number of IGamePlugin types registered during this reload.
        /// </summary>
        public static int ReloadAll()
        {
            string pluginsDirectory = GetPluginsDirectory();
            EnsureDirectoryExists(pluginsDirectory);

            int count = LoadAllPluginsFromDirectory(pluginsDirectory, showSummary: false);

            // Refresh XML plugins too
            XmlPluginManager.LoadFromDirectory(pluginsDirectory);

            MessageBox.Show(
                string.Format("Reload complete. Registered {0} game plugin type(s).", count),
                "Plugins",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            return count;
        }

        // ── Internal helpers ──────────────────────────────────────────────────

        /// <summary>Creates the Plugins directory if it does not exist yet.</summary>
        private static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        /// <summary>Loads a single plugin specified on the command line (file name or full path).</summary>
        private static void LoadPluginFromArgument(string argument, string pluginsDirectory)
        {
            string dllPath = Path.IsPathRooted(argument)
                ? argument
                : Path.Combine(pluginsDirectory, argument);

            if (!dllPath.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                dllPath += ".dll";

            if (!File.Exists(dllPath))
            {
                MessageBox.Show(
                    "Plugin file not found: " + dllPath,
                    "Plugin load",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            LoadAssemblyFile(dllPath);
        }

        /// <summary>Loads every suitable DLL file from the given directory.</summary>
        private static int LoadAllPluginsFromDirectory(string pluginsDirectory, bool showSummary)
        {
            int totalTypes = 0;

            foreach (string dllPath in Directory.GetFiles(pluginsDirectory, "*.dll"))
            {
                if (ShouldSkipFile(dllPath))
                    continue;

                totalTypes += LoadAssemblyFile(dllPath);
            }

            return totalTypes;
        }

        /// <summary>Skips host and third-party libraries that are not plugin modules.</summary>
        private static bool ShouldSkipFile(string dllPath)
        {
            string fileName = Path.GetFileName(dllPath);

            if (fileName.Equals("Lab5.exe", StringComparison.OrdinalIgnoreCase) ||
                fileName.Equals("Lab5.dll", StringComparison.OrdinalIgnoreCase))
                return true;

            if (fileName.StartsWith("Newtonsoft.", StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        /// <summary>Loads one assembly and registers all IGamePlugin implementations found inside.</summary>
        private static int LoadAssemblyFile(string dllPath)
        {
            if (LoadedAssemblyPaths.Contains(dllPath))
                return 0;

            try
            {
                Assembly assembly = Assembly.LoadFrom(dllPath);
                IEnumerable<IGamePlugin> plugins = FindGamePluginsInAssembly(assembly);

                int count = 0;
                foreach (IGamePlugin plugin in plugins)
                {
                    GameFactory.RegisterPlugin(plugin);
                    count++;
                }

                LoadedAssemblyPaths.Add(dllPath);
                return count;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to load plugin: " + Path.GetFileName(dllPath) + Environment.NewLine + ex.Message,
                    "Plugin load error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return 0;
            }
        }

        /// <summary>Instantiates every concrete type in the assembly that implements IGamePlugin.</summary>
        private static IEnumerable<IGamePlugin> FindGamePluginsInAssembly(Assembly assembly)
        {
            Type pluginInterface = typeof(IGamePlugin);
            var result = new List<IGamePlugin>();

            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsAbstract || type.IsInterface)
                    continue;

                if (!pluginInterface.IsAssignableFrom(type))
                    continue;

                var instance = Activator.CreateInstance(type) as IGamePlugin;
                if (instance != null)
                    result.Add(instance);
            }

            return result;
        }
    }
}
