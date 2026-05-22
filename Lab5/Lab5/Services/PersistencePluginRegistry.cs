using System;
using System.Collections.Generic;
using System.Linq;
using Lab5.Plugins;

namespace Lab5.Services
{
    /// <summary>
    /// Holds loaded persistence plugins and their enabled state for the save/load pipeline.
    /// </summary>
    public static class PersistencePluginRegistry
    {
        private static readonly List<RegisteredPersistencePlugin> entries =
            new List<RegisteredPersistencePlugin>();

        /// <summary>Raised when the plugin list or enabled flags change (refresh settings UI).</summary>
        public static event Action Changed;

        /// <summary>Read-only view of registered persistence plugins.</summary>
        public static IReadOnlyList<RegisteredPersistencePlugin> Entries
        {
            get { return entries.AsReadOnly(); }
        }

        /// <summary>
        /// Registers a plugin instance if its id is not already present.
        /// </summary>
        /// <returns>True when a new entry was added.</returns>
        public static bool Register(IPersistencePlugin plugin, string sourcePath)
        {
            if (plugin == null)
                throw new ArgumentNullException(nameof(plugin));

            if (string.IsNullOrWhiteSpace(plugin.PluginId))
                throw new ArgumentException("PluginId cannot be empty.");

            if (entries.Any(e => e.Plugin.PluginId.Equals(plugin.PluginId, StringComparison.OrdinalIgnoreCase)))
                return false;

            entries.Add(new RegisteredPersistencePlugin(plugin, sourcePath, enabled: true));
            OnChanged();
            return true;
        }

        /// <summary>Sets whether a plugin participates in the save/load pipeline.</summary>
        public static void SetEnabled(string pluginId, bool enabled)
        {
            RegisteredPersistencePlugin entry = FindEntry(pluginId);
            if (entry == null)
                return;

            entry.Enabled = enabled;
            OnChanged();
        }

        /// <summary>Returns enabled plugins in registration order.</summary>
        public static IList<IPersistencePlugin> GetEnabledPluginsInOrder()
        {
            return entries.Where(e => e.Enabled).Select(e => e.Plugin).ToList();
        }

        /// <summary>Returns enabled plugins in reverse order (for after-load processing).</summary>
        public static IList<IPersistencePlugin> GetEnabledPluginsReverseOrder()
        {
            return entries.Where(e => e.Enabled).Select(e => e.Plugin).Reverse().ToList();
        }

        /// <summary>Finds a registered plugin by id.</summary>
        public static RegisteredPersistencePlugin FindEntry(string pluginId)
        {
            return entries.FirstOrDefault(
                e => e.Plugin.PluginId.Equals(pluginId, StringComparison.OrdinalIgnoreCase));
        }

        private static void OnChanged()
        {
            Action handler = Changed;
            if (handler != null)
                handler();
        }
    }

    /// <summary>Wrapper that stores plugin instance, source DLL path, and enabled flag.</summary>
    public class RegisteredPersistencePlugin
    {
        public RegisteredPersistencePlugin(IPersistencePlugin plugin, string sourcePath, bool enabled)
        {
            Plugin = plugin;
            SourcePath = sourcePath;
            Enabled = enabled;
        }

        public IPersistencePlugin Plugin { get; private set; }
        public string SourcePath { get; private set; }
        public bool Enabled { get; set; }
    }
}
