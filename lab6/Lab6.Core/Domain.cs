using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Lab6.Core
{
    /// <summary>
    /// Base sports record that can be serialized to BSON. Designed to be extended
    /// by both built-in classes and dynamically loaded plugin assemblies.
    /// </summary>
    public abstract class MediaItem
    {
        /// <summary>Display name of the sports record.</summary>
        public string Title { get; set; }

        /// <summary>Year of the season / event.</summary>
        public int Year { get; set; }

        /// <summary>Coach or main responsible person.</summary>
        public string Creator { get; set; }

        /// <summary>Textual type name shown in the UI.</summary>
        public abstract string TypeName { get; }

        /// <summary>Additional info specific to the concrete type.</summary>
        public abstract string GetDetails();

        public override string ToString()
        {
            return $"{TypeName}: {Title} ({Year}) - {Creator}";
        }
    }

    public class Football : MediaItem
    {
        public string League { get; set; }
        public override string TypeName => "Football";
        public override string GetDetails() => $"League: {League}";
    }

    public class Basketball : MediaItem
    {
        public string League { get; set; }
        public override string TypeName => "Basketball";
        public override string GetDetails() => $"League: {League}";
    }

    public class Swimming : MediaItem
    {
        public int DistanceMeters { get; set; }
        public override string TypeName => "Swimming";
        public override string GetDetails() => $"Distance: {DistanceMeters} m";
    }

    public class Running : MediaItem
    {
        public double DistanceKm { get; set; }
        public override string TypeName => "Running";
        public override string GetDetails() => $"Distance: {DistanceKm:0.##} km";
    }

    public class Tennis : MediaItem
    {
        public string Surface { get; set; }
        public override string TypeName => "Tennis";
        public override string GetDetails() => $"Surface: {Surface}";
    }

    public class Cycling : MediaItem
    {
        public int DistanceKm { get; set; }
        public override string TypeName => "Cycling";
        public override string GetDetails() => $"Distance: {DistanceKm} km";
    }

    /// <summary>
    /// Descriptor that knows how to create a concrete sports record for the UI.
    /// Plugins push their own descriptors into the registry at startup.
    /// </summary>
    public sealed class MediaItemTypeDescriptor
    {
        public string Name { get; }
        private readonly Func<string, string, int, MediaItem> factory;

        public MediaItemTypeDescriptor(string name, Func<string, string, int, MediaItem> factory)
        {
            Name = name;
            this.factory = factory;
        }

        public MediaItem Create(string title, string creator, int year)
        {
            return factory(title, creator, year);
        }

        public override string ToString() => Name;
    }

    /// <summary>
    /// Registry of known sports record types. Built-ins are seeded eagerly;
    /// plugins are registered via <see cref="Register"/> after a successful load.
    /// </summary>
    public static class MediaItemTypeRegistry
    {
        private static readonly List<MediaItemTypeDescriptor> types = new List<MediaItemTypeDescriptor>
        {
            new MediaItemTypeDescriptor("Football", (t, c, y) => new Football { Title = t, Creator = c, Year = y, League = "Premier League" }),
            new MediaItemTypeDescriptor("Basketball", (t, c, y) => new Basketball { Title = t, Creator = c, Year = y, League = "NBA" }),
            new MediaItemTypeDescriptor("Swimming", (t, c, y) => new Swimming { Title = t, Creator = c, Year = y, DistanceMeters = 100 }),
            new MediaItemTypeDescriptor("Running", (t, c, y) => new Running { Title = t, Creator = c, Year = y, DistanceKm = 5.0 }),
            new MediaItemTypeDescriptor("Tennis", (t, c, y) => new Tennis { Title = t, Creator = c, Year = y, Surface = "Hard" }),
            new MediaItemTypeDescriptor("Cycling", (t, c, y) => new Cycling { Title = t, Creator = c, Year = y, DistanceKm = 40 })
        };

        public static IReadOnlyList<MediaItemTypeDescriptor> Types => types;

        /// <summary>Adds a new descriptor coming from a plugin.</summary>
        public static void Register(MediaItemTypeDescriptor descriptor)
        {
            if (descriptor == null) throw new ArgumentNullException(nameof(descriptor));
            types.Add(descriptor);
        }
    }

    /// <summary>
    /// Wrapper around BindingList that encapsulates BSON serialization logic.
    /// Plugin classes can be registered for polymorphic serialization through
    /// <see cref="RegisterPluginType"/>.
    /// </summary>
    public class MediaItemList
    {
        static MediaItemList()
        {
            // Register mappings only once per application domain.
            if (!BsonClassMap.IsClassMapRegistered(typeof(MediaItem)))
            {
                BsonClassMap.RegisterClassMap<MediaItem>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIsRootClass(true);
                });

                BsonClassMap.RegisterClassMap<Football>(cm => cm.AutoMap());
                BsonClassMap.RegisterClassMap<Basketball>(cm => cm.AutoMap());
                BsonClassMap.RegisterClassMap<Swimming>(cm => cm.AutoMap());
                BsonClassMap.RegisterClassMap<Running>(cm => cm.AutoMap());
                BsonClassMap.RegisterClassMap<Tennis>(cm => cm.AutoMap());
                BsonClassMap.RegisterClassMap<Cycling>(cm => cm.AutoMap());
            }
        }

        /// <summary>
        /// Registers a plugin-provided concrete type so BSON can polymorphically
        /// deserialize it. Safe to call multiple times.
        /// </summary>
        public static void RegisterPluginType(Type pluginType)
        {
            if (pluginType == null) throw new ArgumentNullException(nameof(pluginType));
            if (!BsonClassMap.IsClassMapRegistered(pluginType))
            {
                var map = new BsonClassMap(pluginType);
                map.AutoMap();
                BsonClassMap.RegisterClassMap(map);
            }
        }

        public BindingList<MediaItem> Items { get; } = new BindingList<MediaItem>();

        public void Add(MediaItem item) => Items.Add(item);
        public void Remove(MediaItem item) => Items.Remove(item);

        /// <summary>
        /// Serializes the current collection to a BSON byte array. The host
        /// can then push the bytes through the processing pipeline before
        /// writing them to a file, so plugins can compress, encrypt, etc.
        /// </summary>
        public byte[] SerializeToBson()
        {
            var array = new BsonArray();
            foreach (var item in Items)
                array.Add(item.ToBsonDocument());
            var document = new BsonDocument { { "items", array } };
            return document.ToBson();
        }

        /// <summary>
        /// Replaces current items with the contents of the given BSON byte
        /// array. The host is responsible for running the pipeline in reverse
        /// before calling this method.
        /// </summary>
        public void DeserializeFromBson(byte[] bytes)
        {
            var document = BsonSerializer.Deserialize<BsonDocument>(bytes);
            var array = document["items"].AsBsonArray;
            Items.Clear();
            foreach (var value in array)
            {
                var item = BsonSerializer.Deserialize<MediaItem>(value.AsBsonDocument);
                Items.Add(item);
            }
        }

        /// <summary>Convenience wrapper that writes raw BSON to a file.</summary>
        public void SaveToBson(string filePath) => File.WriteAllBytes(filePath, SerializeToBson());

        /// <summary>Convenience wrapper that reads raw BSON from a file.</summary>
        public void LoadFromBson(string filePath) => DeserializeFromBson(File.ReadAllBytes(filePath));
    }
}
