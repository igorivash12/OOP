using Lab5.Models;

namespace Lab5.Plugins
{
    /// <summary>
    /// Contract for dynamically loaded modules that extend the game hierarchy.
    /// Each plugin registers one entity type and its factory method.
    /// Carried forward from Lab4 — all game-type plugins implement this interface.
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
