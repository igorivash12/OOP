using System.Xml;

namespace Lab5.Plugins
{
    /// <summary>
    /// Contract for XML-processing plugins loaded dynamically at runtime.
    /// Each plugin transforms XML content either before saving or after loading.
    /// Lab5 variant: XML Transformation (XSLT and utility plugins).
    /// </summary>
    public interface IXmlPlugin
    {
        /// <summary>Unique internal identifier for the plugin (e.g. "XmlTransform").</summary>
        string Name { get; }

        /// <summary>Human-readable description shown in the plugin manager UI.</summary>
        string Description { get; }

        /// <summary>
        /// Transforms the XML document before it is serialized to disk.
        /// Modify the document in place or return a modified copy.
        /// </summary>
        /// <param name="xml">Full XML content string to process.</param>
        /// <returns>Processed XML content string.</returns>
        string ProcessBeforeSave(string xml);

        /// <summary>
        /// Transforms the XML document after it is loaded from disk.
        /// Used for reversible transformations (e.g. stripping metadata added by ProcessBeforeSave).
        /// Return the original xml unchanged if no post-load processing is required.
        /// </summary>
        /// <param name="xml">Full XML content string to process.</param>
        /// <returns>Processed XML content string.</returns>
        string ProcessAfterLoad(string xml);
    }
}
