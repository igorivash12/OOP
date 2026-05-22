using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using Lab5.Plugins;

namespace Plugin.XmlRootRename
{
    /// <summary>
    /// Renames the root element GameLibrary to Library on save and restores it on load using XSLT.
    /// </summary>
    public class XmlRootRenamePlugin : IPersistencePlugin
    {
        /// <summary>XSLT: renames GameLibrary root to Library for storage.</summary>
        private const string SaveXslt = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xsl:stylesheet version=""1.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"">
  <xsl:template match=""GameLibrary"">
    <Library><xsl:apply-templates select=""@*|node()""/></Library>
  </xsl:template>
  <xsl:template match=""@*|node()"">
    <xsl:copy><xsl:apply-templates select=""@*|node()""/></xsl:copy>
  </xsl:template>
</xsl:stylesheet>";

        /// <summary>XSLT: restores Library root back to GameLibrary after load.</summary>
        private const string LoadXslt = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xsl:stylesheet version=""1.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"">
  <xsl:template match=""Library"">
    <GameLibrary><xsl:apply-templates select=""@*|node()""/></GameLibrary>
  </xsl:template>
  <xsl:template match=""@*|node()"">
    <xsl:copy><xsl:apply-templates select=""@*|node()""/></xsl:copy>
  </xsl:template>
</xsl:stylesheet>";

        public string PluginId
        {
            get { return "XmlRootRename"; }
        }

        public string DisplayName
        {
            get { return "XML root rename (XSLT)"; }
        }

        public string Description
        {
            get { return "Renames &lt;GameLibrary&gt; to &lt;Library&gt; in the file via XSLT."; }
        }

        /// <summary>Transforms root element name before writing the file.</summary>
        public string ProcessBeforeSave(string xmlContent)
        {
            return ApplyXslt(xmlContent, SaveXslt);
        }

        /// <summary>Restores the original root element name after reading the file.</summary>
        public string ProcessAfterLoad(string xmlContent)
        {
            return ApplyXslt(xmlContent, LoadXslt);
        }

        /// <summary>No configurable menu items — XSLT is embedded.</summary>
        public ToolStripItem[] GetSettingsMenuItems()
        {
            return null;
        }

        /// <summary>Runs an XSLT stylesheet loaded from a string.</summary>
        private static string ApplyXslt(string xmlContent, string xsltMarkup)
        {
            var transform = new XslCompiledTransform();
            using (var xsltReader = new StringReader(xsltMarkup))
            using (var xmlReader = XmlReader.Create(xsltReader))
            {
                transform.Load(xmlReader);
            }

            var input = new XmlDocument();
            input.LoadXml(xmlContent);

            using (var writer = new StringWriter())
            {
                transform.Transform(input, null, writer);
                return writer.ToString();
            }
        }
    }
}
