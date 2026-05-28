using System.Collections.Generic;

namespace Lab6.Core.Plugins
{
    /// <summary>
    /// Contract every type-providing plugin assembly must implement. The host
    /// scans loaded assemblies for non-abstract classes implementing this
    /// interface, instantiates them and asks for descriptors.
    /// </summary>
    public interface IMediaItemPlugin
    {
        /// <summary>Human readable plugin name shown in the host UI.</summary>
        string Name { get; }

        /// <summary>Optional plugin version, used purely for diagnostics.</summary>
        string Version { get; }

        /// <summary>
        /// New descriptors that the plugin contributes to the host registry.
        /// </summary>
        IEnumerable<MediaItemTypeDescriptor> GetTypeDescriptors();

        /// <summary>
        /// Concrete <see cref="MediaItem"/> subclasses introduced by the plugin.
        /// The host uses this list to wire BSON polymorphic serialization.
        /// </summary>
        IEnumerable<System.Type> GetItemTypes();
    }
}
