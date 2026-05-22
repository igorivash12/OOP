using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Lab4.Models;

namespace Lab4.Services
{
    /// <summary>
    /// Persists the entity list as JSON with type metadata (supports plugin types after load).
    /// </summary>
    public static class JsonSerializerService
    {
        /// <summary>Newtonsoft settings: store CLR type names for polymorphic deserialization.</summary>
        private static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Newtonsoft.Json.Formatting.Indented
        };

        /// <summary>Writes all items to the given file path.</summary>
        public static void Save(string path, List<BaseEntity> items)
        {
            string json = JsonConvert.SerializeObject(items, settings);
            File.WriteAllText(path, json);
        }

        /// <summary>
        /// Reads items from disk. Plugin assemblies must be loaded before calling
        /// so that plugin entity types can be instantiated during deserialization.
        /// </summary>
        public static List<BaseEntity> Load(string path)
        {
            if (!File.Exists(path))
                return new List<BaseEntity>();

            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<List<BaseEntity>>(json, settings);
        }
    }
}
