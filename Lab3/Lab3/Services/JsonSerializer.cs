using Lab3.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Lab3.Services
{
    public static class JsonSerializerService
    {
        private static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Newtonsoft.Json.Formatting.Indented
        };

        // Save to JSON file
        public static void Save(string path, List<BaseEntity> items)
        {
            string json = JsonConvert.SerializeObject(items, settings);
            File.WriteAllText(path, json);
        }

        // Load from JSON file
        public static List<BaseEntity> Load(string path)
        {
            if (!File.Exists(path))
                return new List<BaseEntity>();

            string json = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<List<BaseEntity>>(json, settings);
        }
    }
}