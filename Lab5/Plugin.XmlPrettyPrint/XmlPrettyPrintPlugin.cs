using System.IO;
using System.Windows.Forms;
using System.Xml;
using Lab5.Plugins;

namespace Plugin.XmlPrettyPrint
{
    /// <summary>
    /// Formats XML with indentation before save; compacts XML after load for stable parsing.
    /// </summary>
    public class XmlPrettyPrintPlugin : IPersistencePlugin
    {
        public string PluginId
        {
            get { return "XmlPrettyPrint"; }
        }

        public string DisplayName
        {
            get { return "XML pretty-print"; }
        }

        public string Description
        {
            get { return "Indents XML on save; normalizes whitespace on load."; }
        }

        /// <summary>Applies human-readable indentation to the XML document.</summary>
        public string ProcessBeforeSave(string xmlContent)
        {
            var document = new XmlDocument();
            document.LoadXml(xmlContent);

            using (var writer = new StringWriter())
            {
                var xmlWriter = new XmlTextWriter(writer)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 2
                };
                document.Save(xmlWriter);
                return writer.ToString();
            }
        }

        /// <summary>Removes cosmetic formatting so the host can deserialize reliably.</summary>
        public string ProcessAfterLoad(string xmlContent)
        {
            var document = new XmlDocument();
            document.LoadXml(xmlContent);
            return document.OuterXml;
        }

        /// <summary>No extra menu entries for this plugin.</summary>
        public ToolStripItem[] GetSettingsMenuItems()
        {
            return null;
        }
    }
}
