using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using Lab5.Models;
using Lab5.Plugins;

namespace Lab5.Services
{
    /// <summary>
    /// Persists the entity list as JSON wrapped in an XML envelope.
    /// Before saving, the content is passed through all enabled IXmlPlugin.ProcessBeforeSave.
    /// After loading, the content is passed through all enabled IXmlPlugin.ProcessAfterLoad.
    /// This fulfils Lab5 Variant 4: XML Transformation.
    /// </summary>
    public static class JsonSerializerService
    {
        // ── Newtonsoft settings: preserve CLR type names for polymorphic deserialization ──
        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Newtonsoft.Json.Formatting.Indented
        };

        // ── XML element/attribute names ────────────────────────────────────────
        private const string RootElement    = "GameLibrary";
        private const string DataElement    = "Data";
        private const string SavedAtAttr    = "savedAt";
        private const string VersionAttr    = "version";
        private const string AppVersion     = "5.0";

        // ── Public API ─────────────────────────────────────────────────────────

        /// <summary>
        /// Serializes the item list to JSON, wraps it in an XML envelope,
        /// runs the XML through the enabled plugin pipeline, then writes to disk.
        /// </summary>
        public static void Save(string path, List<BaseEntity> items)
        {
            // Step 1: serialize items to JSON
            string json = JsonConvert.SerializeObject(items, JsonSettings);

            // Step 2: wrap JSON inside an XML document
            string xml = WrapJsonInXml(json);

            // Step 3: run all enabled XML plugins (ProcessBeforeSave)
            xml = XmlPluginManager.RunBeforeSave(xml);

            // Step 4: write processed XML to disk
            File.WriteAllText(path, xml, Encoding.UTF8);
        }

        /// <summary>
        /// Reads the XML file from disk, runs it through the enabled plugin pipeline
        /// (ProcessAfterLoad), then extracts and deserializes the JSON payload.
        /// </summary>
        public static List<BaseEntity> Load(string path)
        {
            if (!File.Exists(path))
                return new List<BaseEntity>();

            // Step 1: read raw XML from disk
            string xml = File.ReadAllText(path, Encoding.UTF8);

            // Step 2: run all enabled XML plugins (ProcessAfterLoad) in reverse order
            xml = XmlPluginManager.RunAfterLoad(xml);

            // Step 3: extract the JSON payload from the XML envelope
            string json = ExtractJsonFromXml(xml);
            if (string.IsNullOrWhiteSpace(json))
                return new List<BaseEntity>();

            // Step 4: deserialize the JSON back to entity list
            return JsonConvert.DeserializeObject<List<BaseEntity>>(json, JsonSettings)
                   ?? new List<BaseEntity>();
        }

        // ── XML envelope helpers ───────────────────────────────────────────────

        /// <summary>
        /// Creates a well-formed XML document that wraps the JSON payload in a CDATA section.
        /// </summary>
        private static string WrapJsonInXml(string json)
        {
            var doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement(RootElement,
                    new XAttribute(VersionAttr, AppVersion),
                    new XAttribute(SavedAtAttr, DateTime.UtcNow.ToString("o")),
                    new XElement(DataElement,
                        new XCData(json))));

            return doc.ToString(SaveOptions.None);
        }

        /// <summary>
        /// Extracts the JSON string from the XML document's Data/CDATA node.
        /// Falls back to treating the entire string as raw JSON if parsing fails
        /// (supports files saved without the XML wrapper).
        /// </summary>
        private static string ExtractJsonFromXml(string xml)
        {
            try
            {
                var doc = XDocument.Parse(xml);
                XElement dataEl = doc.Root?.Element(DataElement);

                if (dataEl != null)
                    return dataEl.Value; // XDocument automatically decodes CDATA content
            }
            catch
            {
                // File may be old-format plain JSON — fall through and return as-is
            }

            // Fallback: treat input as raw JSON (backward-compat with Lab4 files)
            return xml;
        }
    }
}
