using System.Windows.Forms;

namespace Lab6.Core.Plugins
{
    /// <summary>
    /// Contract for plugins that mutate the serialized byte stream on its way
    /// to and from disk. Plugins are stacked into a pipeline; the host calls
    /// <see cref="ProcessOnSave"/> in registration order and
    /// <see cref="ProcessOnLoad"/> in reverse, so a Compressв†’Encrypt chain on
    /// save becomes Decryptв†’Decompress on load automatically.
    /// </summary>
    public interface IProcessingPlugin
    {
        /// <summary>Display name shown in the settings menu.</summary>
        string Name { get; }

        /// <summary>Short description of what the plugin does.</summary>
        string Description { get; }

        /// <summary>
        /// Whether the user has enabled this plugin via the settings menu.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>Transforms bytes about to be written to a file.</summary>
        byte[] ProcessOnSave(byte[] input);

        /// <summary>Reverses <see cref="ProcessOnSave"/> on bytes read back.</summary>
        byte[] ProcessOnLoad(byte[] input);

        /// <summary>
        /// Builds a small UI control that lets the user tweak the plugin's
        /// parameters (encryption key, compression level, etc.). May return
        /// null if the plugin has no configurable options.
        /// </summary>
        Control BuildSettingsControl();
    }
}
