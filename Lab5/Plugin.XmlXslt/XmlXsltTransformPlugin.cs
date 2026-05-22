using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using Lab5.Plugins;

namespace Plugin.XmlXslt
{
    /// <summary>
    /// Applies a user-selected or default XSLT stylesheet before save; inverse stylesheet on load.
    /// </summary>
    public class XmlXsltTransformPlugin : IPersistencePlugin
    {
        /// <summary>Path to custom save XSLT chosen in Settings (optional).</summary>
        private static string customSaveXsltPath;

        /// <summary>Path to custom load XSLT chosen in Settings (optional).</summary>
        private static string customLoadXsltPath;

        /// <summary>Default save transform: adds persisted=""true"" on the root element.</summary>
        private const string DefaultSaveXslt = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xsl:stylesheet version=""1.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"">
  <xsl:template match=""GameLibrary"">
    <GameLibrary persisted=""true"">
      <xsl:apply-templates select=""@*|node()""/>
    </GameLibrary>
  </xsl:template>
  <xsl:template match=""@*|node()"">
    <xsl:copy><xsl:apply-templates select=""@*|node()""/></xsl:copy>
  </xsl:template>
</xsl:stylesheet>";

        /// <summary>Default load transform: removes the persisted attribute from the root.</summary>
        private const string DefaultLoadXslt = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xsl:stylesheet version=""1.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"">
  <xsl:template match=""GameLibrary"">
    <GameLibrary>
      <xsl:apply-templates select=""@*|node()""/>
    </GameLibrary>
  </xsl:template>
  <xsl:template match=""@persisted""/>
  <xsl:template match=""@*|node()"">
    <xsl:copy><xsl:apply-templates select=""@*|node()""/></xsl:copy>
  </xsl:template>
</xsl:stylesheet>";

        public string PluginId
        {
            get { return "XmlXsltTransform"; }
        }

        public string DisplayName
        {
            get { return "Custom XSLT transform"; }
        }

        public string Description
        {
            get { return "XSLT before save / reverse XSLT after load (custom or built-in)."; }
        }

        /// <summary>Runs save XSLT (custom file or built-in default).</summary>
        public string ProcessBeforeSave(string xmlContent)
        {
            bool useFile = !string.IsNullOrEmpty(customSaveXsltPath) && File.Exists(customSaveXsltPath);
            string xslt = useFile ? customSaveXsltPath : DefaultSaveXslt;
            return ApplyXslt(xmlContent, xslt, useFile);
        }

        /// <summary>Runs load XSLT (custom file or built-in default).</summary>
        public string ProcessAfterLoad(string xmlContent)
        {
            bool useFile = !string.IsNullOrEmpty(customLoadXsltPath) && File.Exists(customLoadXsltPath);
            string xslt = useFile ? customLoadXsltPath : DefaultLoadXslt;
            return ApplyXslt(xmlContent, xslt, useFile);
        }

        /// <summary>Menu entries to pick save/load XSLT files from disk.</summary>
        public ToolStripItem[] GetSettingsMenuItems()
        {
            var pickSave = new ToolStripMenuItem("Choose save XSLT file...");
            pickSave.Click += PickSaveXslt_Click;

            var pickLoad = new ToolStripMenuItem("Choose load XSLT file...");
            pickLoad.Click += PickLoadXslt_Click;

            var reset = new ToolStripMenuItem("Reset to built-in XSLT");
            reset.Click += ResetXslt_Click;

            return new ToolStripItem[] { pickSave, pickLoad, reset };
        }

        /// <summary>Opens file dialog for the save-phase XSLT stylesheet.</summary>
        private void PickSaveXslt_Click(object sender, System.EventArgs e)
        {
            string path = BrowseXsltFile();
            if (path != null)
                customSaveXsltPath = path;
        }

        /// <summary>Opens file dialog for the load-phase XSLT stylesheet.</summary>
        private void PickLoadXslt_Click(object sender, System.EventArgs e)
        {
            string path = BrowseXsltFile();
            if (path != null)
                customLoadXsltPath = path;
        }

        /// <summary>Clears custom paths so built-in transforms are used again.</summary>
        private void ResetXslt_Click(object sender, System.EventArgs e)
        {
            customSaveXsltPath = null;
            customLoadXsltPath = null;
            MessageBox.Show("Built-in XSLT stylesheets will be used.", "XSLT plugin");
        }

        /// <summary>Shows OpenFileDialog filtered to XSLT files.</summary>
        private static string BrowseXsltFile()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "Select XSLT stylesheet";
                dialog.Filter = "XSLT files (*.xsl;*.xslt)|*.xsl;*.xslt|All files (*.*)|*.*";
                return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : null;
            }
        }

        /// <summary>Applies XSLT from a file path or inline markup string.</summary>
        private static string ApplyXslt(string xmlContent, string xsltSource, bool fromFile)
        {
            var transform = new XslCompiledTransform();

            if (fromFile)
                transform.Load(xsltSource);
            else
            {
                using (var reader = XmlReader.Create(new StringReader(xsltSource)))
                    transform.Load(reader);
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
