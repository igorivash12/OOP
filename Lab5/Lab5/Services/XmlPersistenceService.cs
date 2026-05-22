using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Lab5.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lab5.Services
{
    /// <summary>
    /// Serializes entity collections to XML (via JSON bridge) and runs the persistence plugin pipeline.
    /// </summary>
    public static class XmlPersistenceService
    {
        /// <summary>Default data file name (XML with optional plugin transforms).</summary>
        public const string DefaultFileName = "games.xml";

        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Newtonsoft.Json.Formatting.None
        };

        private static readonly JsonSerializer EntitySerializer = JsonSerializer.Create(JsonSettings);

        /// <summary>Wrapper type so JSON-to-XML conversion has a single root element.</summary>
        private class EntityListWrapper
        {
            public List<BaseEntity> Items { get; set; }
        }

        /// <summary>Resolves a file name to an absolute path next to the executable.</summary>
        public static string ResolveDataFilePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                path = DefaultFileName;

            if (Path.IsPathRooted(path))
                return path;

            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
        }

        /// <summary>
        /// Converts entities to XML, applies persistence plugins, and writes the file.
        /// </summary>
        public static void Save(string path, List<BaseEntity> items)
        {
            string fullPath = ResolveDataFilePath(path);
            string xml = EntitiesToXml(items);
            xml = PersistencePipeline.ProcessBeforeSave(xml);
            File.WriteAllText(fullPath, xml);
        }

        /// <summary>
        /// Reads XML from disk, applies persistence plugins, and rebuilds the entity list.
        /// </summary>
        public static List<BaseEntity> Load(string path)
        {
            string fullPath = ResolveDataFilePath(path);

            if (!File.Exists(fullPath))
                return new List<BaseEntity>();

            string xml = File.ReadAllText(fullPath);
            xml = PersistencePipeline.ProcessAfterLoad(xml);
            return XmlToEntities(xml);
        }

        /// <summary>Returns true when the default data file exists beside the executable.</summary>
        public static bool DefaultDataFileExists()
        {
            return File.Exists(ResolveDataFilePath(DefaultFileName));
        }

        /// <summary>Builds canonical XML from the in-memory entity list (before plugin transforms).</summary>
        public static string EntitiesToXml(List<BaseEntity> items)
        {
            var wrapper = new EntityListWrapper { Items = items ?? new List<BaseEntity>() };
            string json = JsonConvert.SerializeObject(wrapper, JsonSettings);
            XmlDocument document = JsonConvert.DeserializeXmlNode(json, "GameLibrary");
            return FormatXml(document.OuterXml);
        }

        /// <summary>
        /// Parses XML (after plugin reverse transforms) back into entities.
        /// Handles GameLibrary/Library root and Items/values array layout from JSON.NET XML conversion.
        /// </summary>
        public static List<BaseEntity> XmlToEntities(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
                return new List<BaseEntity>();

            var document = new XmlDocument();
            document.LoadXml(xml);
            string json = JsonConvert.SerializeXmlNode(document);
            JObject root = JObject.Parse(json);

            // Root may be GameLibrary (canonical) or Library (if rename plugin was used on save)
            JToken container = root["GameLibrary"] ?? root["Library"];
            if (container == null)
                return new List<BaseEntity>();

            return ParseItemsFromContainer(container);
        }

        /// <summary>Extracts entity instances from the Items node produced by SerializeXmlNode.</summary>
        private static List<BaseEntity> ParseItemsFromContainer(JToken container)
        {
            var result = new List<BaseEntity>();
            JToken itemsNode = container["Items"];

            if (itemsNode == null)
                return result;

            // List serialized to XML uses "values" elements; JSON list uses "$values"
            JToken values = itemsNode["$values"] ?? itemsNode["values"];
            if (values == null)
                return result;

            if (values is JArray array)
            {
                foreach (JToken entry in array)
                    result.Add(DeserializeEntity(entry));
            }
            else
            {
                result.Add(DeserializeEntity(values));
            }

            return result;
        }

        /// <summary>
        /// Deserializes one entity token after normalizing XML-to-JSON artifacts
        /// (e.g. Platform: { "#text": "Windows", "@xmlns": "" } → "Windows").
        /// </summary>
        private static BaseEntity DeserializeEntity(JToken token)
        {
            JObject normalized = NormalizeEntityObject(token as JObject ?? new JObject());
            return normalized.ToObject<BaseEntity>(EntitySerializer);
        }

        /// <summary>Builds a clean JSON object suitable for Newtonsoft entity deserialization.</summary>
        private static JObject NormalizeEntityObject(JObject source)
        {
            var result = new JObject();

            foreach (JProperty property in source.Properties())
            {
                string name = property.Name;

                if (name == "@json:type" || name == "$type")
                {
                    result["$type"] = NormalizeScalar(property.Value);
                    continue;
                }

                // Skip XML namespace declarations on the entity element
                if (name.StartsWith("@", StringComparison.Ordinal) || name == "xmlns")
                    continue;

                result[name] = NormalizeToken(property.Value);
            }

            return result;
        }

        /// <summary>Recursively unwraps XML element JSON representation to scalars or nested objects.</summary>
        private static JToken NormalizeToken(JToken token)
        {
            if (token == null || token.Type == JTokenType.Null)
                return JValue.CreateNull();

            switch (token.Type)
            {
                case JTokenType.Object:
                    return NormalizeObjectToken((JObject)token);
                case JTokenType.Array:
                    return new JArray(token.Select(NormalizeToken));
                default:
                    return token;
            }
        }

        /// <summary>Unwraps objects produced by SerializeXmlNode for simple XML elements.</summary>
        private static JToken NormalizeObjectToken(JObject obj)
        {
            JToken textNode = obj["#text"];
            if (textNode != null && obj.Properties().All(p => IsXmlMetadataProperty(p.Name) || p.Name == "#text"))
                return NormalizeScalar(textNode);

            var result = new JObject();
            foreach (JProperty property in obj.Properties())
            {
                if (IsXmlMetadataProperty(property.Name))
                    continue;

                result[property.Name] = NormalizeToken(property.Value);
            }

            return result;
        }

        /// <summary>Returns true for XML attribute names injected by SerializeXmlNode.</summary>
        private static bool IsXmlMetadataProperty(string name)
        {
            return name.StartsWith("@", StringComparison.Ordinal) || name == "xmlns";
        }

        /// <summary>Converts text nodes to string/number/bool when possible.</summary>
        private static JToken NormalizeScalar(JToken token)
        {
            if (token == null || token.Type == JTokenType.Null)
                return JValue.CreateNull();

            if (token.Type == JTokenType.String)
            {
                string text = token.Value<string>();
                if (string.IsNullOrEmpty(text))
                    return new JValue(string.Empty);

                int intValue;
                if (int.TryParse(text, out intValue))
                    return new JValue(intValue);

                long longValue;
                if (long.TryParse(text, out longValue))
                    return new JValue(longValue);

                double doubleValue;
                if (double.TryParse(text, System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out doubleValue))
                    return new JValue(doubleValue);

                bool boolValue;
                if (bool.TryParse(text, out boolValue))
                    return new JValue(boolValue);

                return new JValue(text);
            }

            return token;
        }

        /// <summary>Normalizes XML formatting for stable downstream transforms.</summary>
        private static string FormatXml(string xml)
        {
            var document = new XmlDocument();
            document.LoadXml(xml);
            using (var writer = new StringWriter())
            using (var xmlWriter = new XmlTextWriter(writer))
            {
                xmlWriter.Formatting = System.Xml.Formatting.None;
                document.WriteTo(xmlWriter);
                return writer.ToString();
            }
        }
    }
}
