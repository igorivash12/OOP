using System.Windows.Forms;

namespace Lab5.Plugins
{
    /// <summary>
    /// Contract for plugins that transform XML payload before file save and after file load.
    /// Variant 4: XML transformation (including optional XSLT).
    /// </summary>
    public interface IPersistencePlugin
    {
        /// <summary>Unique identifier used in settings and registry.</summary>
        string PluginId { get; }

        /// <summary>Human-readable name shown in the settings dialog.</summary>
        string DisplayName { get; }

        /// <summary>Short description of the transformation performed.</summary>
        string Description { get; }

        /// <summary>
        /// Transforms XML text immediately before writing to disk.
        /// Called in registration order for all enabled plugins.
        /// </summary>
        /// <param name="xmlContent">Current XML document text.</param>
        /// <returns>Transformed XML ready for the next plugin or file write.</returns>
        string ProcessBeforeSave(string xmlContent);

        /// <summary>
        /// Restores or adjusts XML text immediately after reading from disk.
        /// Called in reverse order for all enabled plugins.
        /// </summary>
        /// <param name="xmlContent">XML text read from file (possibly after prior plugins).</param>
        /// <returns>XML suitable for deserialization.</returns>
        string ProcessAfterLoad(string xmlContent);

        /// <summary>
        /// Optional UI extension: plugin-specific items under the Settings menu.
        /// Return null or empty when the plugin has no extra menu entries.
        /// </summary>
        ToolStripItem[] GetSettingsMenuItems();
    }
}
