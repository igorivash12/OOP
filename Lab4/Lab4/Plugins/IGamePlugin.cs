using Lab4.Models;

namespace Lab4.Plugins
{
    /// <summary>
    /// Contract for dynamically loaded modules that extend the game hierarchy.
    /// Each plugin registers one entity type and its factory method.
    /// </summary>
    public interface IGamePlugin
    {
        /// <summary>Unique type key used in ComboBox and JSON (e.g. "BoardGame").</summary>
        string TypeKey { get; }

        /// <summary>Human-readable name shown in the type ComboBox.</summary>
        string DisplayName { get; }

        /// <summary>Creates a new instance of the plugin entity class.</summary>
        BaseEntity CreateInstance();
    }
}
