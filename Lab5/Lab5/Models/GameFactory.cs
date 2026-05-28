using System;
using System.Collections.Generic;
using System.Linq;
using Lab5.Plugins;

namespace Lab5.Models
{
    /// <summary>
    /// Factory for creating hierarchy objects by type key.
    /// Built-in types are registered at startup; plugins register at runtime.
    /// </summary>
    public static class GameFactory
    {
        private static readonly Dictionary<string, Func<BaseEntity>> Creators =
            new Dictionary<string, Func<BaseEntity>>(StringComparer.OrdinalIgnoreCase);

        private static readonly Dictionary<string, string> DisplayNames =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private static bool builtInRegistered;

        static GameFactory()
        {
            RegisterBuiltInTypes();
        }

        /// <summary>Registers all entity types compiled into the main application.</summary>
        private static void RegisterBuiltInTypes()
        {
            if (builtInRegistered)
                return;

            Register("PCGame", "PC Game", () => new PCGame());
            Register("ConsoleGame", "Console Game", () => new ConsoleGame());
            Register("MobileGame", "Mobile Game", () => new MobileGame());
            Register("OnlineGame", "Online Game", () => new OnlineGame());
            Register("IndieGame", "Indie Game", () => new IndieGame());
            Register("VRGame", "VR Game", () => new VRGame());

            builtInRegistered = true;
        }

        /// <summary>Adds a creator delegate and optional display name for the UI ComboBox.</summary>
        private static void Register(string typeKey, string displayName, Func<BaseEntity> factory)
        {
            Creators[typeKey] = factory;
            DisplayNames[typeKey] = displayName;
        }

        /// <summary>
        /// Registers a type provided by a dynamically loaded game plugin module.
        /// </summary>
        public static void RegisterPlugin(IGamePlugin plugin)
        {
            if (plugin == null)
                throw new ArgumentNullException("plugin");

            string typeKey = plugin.TypeKey;
            if (string.IsNullOrWhiteSpace(typeKey))
                throw new ArgumentException("Plugin TypeKey cannot be empty.");

            Creators[typeKey] = plugin.CreateInstance;
            DisplayNames[typeKey] = string.IsNullOrWhiteSpace(plugin.DisplayName)
                ? typeKey
                : plugin.DisplayName;
        }

        /// <summary>Creates a new entity instance for the given type key.</summary>
        public static BaseEntity Create(string typeKey)
        {
            if (!Creators.ContainsKey(typeKey))
                throw new KeyNotFoundException("Unknown type: " + typeKey);

            return Creators[typeKey]();
        }

        /// <summary>Returns type keys sorted for filling the type ComboBox.</summary>
        public static IList<string> GetAvailableTypeKeys()
        {
            return Creators.Keys.OrderBy(k => k).ToList();
        }

        /// <summary>Returns display text for a type key (falls back to the key itself).</summary>
        public static string GetDisplayName(string typeKey)
        {
            string name;
            if (DisplayNames.TryGetValue(typeKey, out name))
                return name;
            return typeKey;
        }

        /// <summary>Resolves a display name or type key back to the internal type key.</summary>
        public static string ResolveTypeKey(string selectedText)
        {
            if (string.IsNullOrEmpty(selectedText))
                return selectedText;

            if (Creators.ContainsKey(selectedText))
                return selectedText;

            foreach (var pair in DisplayNames)
            {
                if (pair.Value.Equals(selectedText, StringComparison.OrdinalIgnoreCase))
                    return pair.Key;
            }

            return selectedText;
        }
    }
}
