using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Lab5.Plugins;

namespace Plugin.XmlMetadata
{
    /// <summary>
    /// XML plugin: injects a &lt;Metadata&gt; element into the XML document before saving
    /// and removes it again after loading, keeping the payload clean for deserialization.
    ///
    /// The metadata element records:
    ///   - the UTC timestamp of the save operation
    ///   - the name of the machine that saved the file
    ///   - the Lab5 plugin version
    ///
    /// Lab5 Variant 4 — utility plugin.
    /// </summary>
    public class XmlMetadataPlugin : IXmlPlugin
    {
        // ── Constants ─────────────────────────────────────────────────────────
        private const string MetadataElementName = "Metadata";
        private const string PluginVersion        = "5.0";

        // ── IXmlPlugin identity ───────────────────────────────────────────────

        public string Name        => "XmlMetadata";
        public string Description =>
            "Injects a <Metadata> element into the XML root before saving.\n\n" +
            "The element contains:\n" +
            "  • savedAtUtc   — ISO-8601 timestamp of the save\n" +
            "  • machine      — name of the computer that saved the file\n" +
            "  • pluginVersion — version of this plugin\n\n" +
            "ProcessAfterLoad removes the <Metadata> element so that the JSON " +
            "extractor always receives clean content.";

        // ── IXmlPlugin implementation ─────────────────────────────────────────

        /// <summary>
        /// Adds a &lt;Metadata&gt; child element to the XML document root.
        /// </summary>
        public string ProcessBeforeSave(string xml)
        {
            XDocument doc = XDocument.Parse(xml);

            // Build the metadata element with service attributes
            var metadata = new XElement(MetadataElementName,
                new XAttribute("savedAtUtc",    DateTime.UtcNow.ToString("o")),
                new XAttribute("machine",       Environment.MachineName),
                new XAttribute("pluginVersion", PluginVersion));

            // Append as the first child of the document root for visibility
            doc.Root?.AddFirst(metadata);

            return SerializeDocument(doc);
        }

        /// <summary>
        /// Removes the &lt;Metadata&gt; element inserted by ProcessBeforeSave
        /// so that downstream deserialization receives unmodified data.
        /// </summary>
        public string ProcessAfterLoad(string xml)
        {
            try
            {
                XDocument doc = XDocument.Parse(xml);

                // Remove all Metadata elements from the root (handle duplicates safely)
                doc.Root?.Elements(MetadataElementName).ToList()
                         .ForEach(el => el.Remove());

                return SerializeDocument(doc);
            }
            catch
            {
                // Return unchanged if parsing fails; the JSON extractor will handle it.
                return xml;
            }
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        /// <summary>
        /// Serializes an XDocument to an indented UTF-8 string without a BOM.
        /// </summary>
        private static string SerializeDocument(XDocument doc)
        {
            var settings = new XmlWriterSettings
            {
                Indent             = true,
                IndentChars        = "  ",
                Encoding           = Encoding.UTF8,
                OmitXmlDeclaration = false
            };

            using (var ms = new MemoryStream())
            {
                using (var xw = XmlWriter.Create(ms, settings))
                    doc.Save(xw);

                return Encoding.UTF8.GetString(ms.ToArray()).TrimStart('\uFEFF');
            }
        }
    }
}
