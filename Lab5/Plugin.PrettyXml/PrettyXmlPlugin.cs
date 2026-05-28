using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Lab5.Plugins;

namespace Plugin.PrettyXml
{
    /// <summary>
    /// XML plugin: formats (pretty-prints) the XML document before saving.
    /// Produces consistently indented, human-readable XML output.
    /// ProcessAfterLoad normalises the whitespace in the same way so that
    /// the document is always in canonical pretty form when read back.
    ///
    /// Lab5 Variant 4 — utility plugin.
    /// </summary>
    public class PrettyXmlPlugin : IXmlPlugin
    {
        // ── IXmlPlugin identity ───────────────────────────────────────────────

        public string Name        => "PrettyXml";
        public string Description =>
            "Pretty-prints the XML document (consistent 2-space indentation, UTF-8 header).\n\n" +
            "ProcessBeforeSave: reformats XML with indentation before writing to disk.\n" +
            "ProcessAfterLoad:  normalises whitespace on load (no-op for documents " +
            "already produced by this plugin, useful for externally modified files).";

        // ── IXmlPlugin implementation ─────────────────────────────────────────

        /// <summary>
        /// Re-serializes the XML with indentation applied.
        /// </summary>
        public string ProcessBeforeSave(string xml)
        {
            return Reformat(xml);
        }

        /// <summary>
        /// Applies the same reformatting on load so that any externally produced
        /// XML file is normalised before the JSON extractor processes it.
        /// This is safe because reformatting is idempotent.
        /// </summary>
        public string ProcessAfterLoad(string xml)
        {
            try
            {
                return Reformat(xml);
            }
            catch
            {
                // If reformatting fails (e.g. the content is not well-formed XML),
                // return the original so downstream processing can still attempt deserialization.
                return xml;
            }
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        /// <summary>
        /// Parses <paramref name="xml"/> and writes it back with 2-space indentation
        /// and an explicit UTF-8 XML declaration.
        /// </summary>
        private static string Reformat(string xml)
        {
            // Load into XDocument — it handles CDATA sections transparently
            XDocument doc = XDocument.Parse(xml, LoadOptions.PreserveWhitespace);

            var settings = new XmlWriterSettings
            {
                Indent             = true,
                IndentChars        = "  ",      // 2-space indent
                NewLineChars       = "\r\n",
                NewLineHandling    = NewLineHandling.Replace,
                Encoding           = Encoding.UTF8,
                OmitXmlDeclaration = false
            };

            using (var ms = new MemoryStream())
            {
                using (var xw = XmlWriter.Create(ms, settings))
                    doc.Save(xw);

                // MemoryStream contains UTF-8 bytes including the BOM from XmlWriter;
                // trim the BOM so the result is clean text.
                return Encoding.UTF8.GetString(ms.ToArray()).TrimStart('\uFEFF');
            }
        }
    }
}
