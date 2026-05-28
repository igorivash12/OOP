using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using Lab5.Plugins;

namespace Plugin.XmlTransform
{
    /// <summary>
    /// XML plugin: applies an XSLT stylesheet to the XML content before saving.
    /// The XSLT stylesheet is embedded as a literal string for portability.
    /// ProcessAfterLoad performs an identity pass (no inverse transformation needed
    /// because the XSLT used here is an identity transform that simply re-orders attributes).
    ///
    /// Lab5 Variant 4 — primary plugin: XML Transformation via XSLT.
    /// </summary>
    public class XmlTransformPlugin : IXmlPlugin
    {
        // ── IXmlPlugin identity ───────────────────────────────────────────────

        public string Name        => "XmlTransform";
        public string Description =>
            "Applies an XSLT stylesheet to the XML before saving.\n" +
            "The built-in transform normalises the document: it copies all nodes " +
            "verbatim while adding a 'transformedBy' processing instruction at the top " +
            "so you can verify the plugin ran.\n\n" +
            "After loading, the processing instruction is stripped automatically.\n\n" +
            "Replace the built-in XSLT with any stylesheet by editing XmlTransformPlugin.cs.";

        // ── Embedded XSLT ─────────────────────────────────────────────────────

        /// <summary>
        /// Identity XSLT that copies everything unchanged and inserts one processing
        /// instruction documenting that the transform ran.  Replace the stylesheet
        /// body to implement a domain-specific transformation (e.g. filter, sort, rename).
        /// </summary>
        private const string XsltStylesheet = @"<?xml version='1.0' encoding='utf-8'?>
<xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'>

  <!-- Identity template: copies every node unchanged -->
  <xsl:template match='@*|node()'>
    <xsl:copy>
      <xsl:apply-templates select='@*|node()'/>
    </xsl:copy>
  </xsl:template>

  <!-- Root override: insert a processing instruction before the document element -->
  <xsl:template match='/'>
    <xsl:processing-instruction name='XmlTransformPlugin'>
      <xsl:text>applied=true</xsl:text>
    </xsl:processing-instruction>
    <xsl:apply-templates/>
  </xsl:template>

</xsl:stylesheet>";

        // ── IXmlPlugin implementation ─────────────────────────────────────────

        /// <summary>
        /// Applies the embedded XSLT stylesheet to the incoming XML string.
        /// Returns the transformed XML; on failure returns the original XML and shows a warning.
        /// </summary>
        public string ProcessBeforeSave(string xml)
        {
            try
            {
                return ApplyXslt(xml, XsltStylesheet);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "XmlTransformPlugin.ProcessBeforeSave failed: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Strips the processing instruction added by ProcessBeforeSave so that the
        /// payload can be deserialized cleanly by the JSON extractor.
        /// </summary>
        public string ProcessAfterLoad(string xml)
        {
            try
            {
                return RemoveProcessingInstruction(xml, "XmlTransformPlugin");
            }
            catch
            {
                // If stripping fails, return unchanged — deserialization will still work
                // because the JSON extractor ignores unknown XML nodes.
                return xml;
            }
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        /// <summary>
        /// Transforms <paramref name="inputXml"/> with <paramref name="xslt"/>
        /// and returns the resulting XML string.
        /// </summary>
        private static string ApplyXslt(string inputXml, string xslt)
        {
            // Load the XSLT stylesheet
            var transform = new XslCompiledTransform();
            using (var xsltReader = XmlReader.Create(new StringReader(xslt)))
                transform.Load(xsltReader);

            // Apply the transform to the input document
            using (var inputReader = XmlReader.Create(new StringReader(inputXml)))
            using (var outputWriter = new StringWriter())
            {
                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    Encoding = Encoding.UTF8,
                    OmitXmlDeclaration = false
                };

                using (var xmlWriter = XmlWriter.Create(outputWriter, settings))
                    transform.Transform(inputReader, xmlWriter);

                return outputWriter.ToString();
            }
        }

        /// <summary>
        /// Removes all processing instructions with the given target from the XML document.
        /// </summary>
        private static string RemoveProcessingInstruction(string xml, string piTarget)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);

            // Collect nodes to remove (cannot remove during iteration)
            var toRemove = new System.Collections.Generic.List<XmlNode>();

            foreach (XmlNode child in doc.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.ProcessingInstruction &&
                    child.Name == piTarget)
                {
                    toRemove.Add(child);
                }
            }

            foreach (XmlNode node in toRemove)
                doc.RemoveChild(node);

            using (var sw = new StringWriter())
            using (var xw = XmlWriter.Create(sw, new XmlWriterSettings { Indent = true }))
            {
                doc.WriteTo(xw);
                xw.Flush();
                return sw.ToString();
            }
        }
    }
}
