using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Lab6.Core.Plugins
{
    /// <summary>
    /// Information about a single attempt to load a plugin. A plugin DLL may
    /// expose both type-providing and processing implementations; either or
    /// both can be populated on success.
    /// </summary>
    public class PluginLoadEntry
    {
        public string FileName { get; set; }
        public bool Loaded { get; set; }
        public string Message { get; set; }
        public IMediaItemPlugin TypePlugin { get; set; }
        public IProcessingPlugin ProcessingPlugin { get; set; }
    }

    /// <summary>
    /// Discovers plugin DLLs in a folder, verifies their signatures and
    /// instantiates every implementation of <see cref="IMediaItemPlugin"/>
    /// and <see cref="IProcessingPlugin"/> they expose. Unsigned or expired
    /// plugins are rejected and never loaded.
    /// </summary>
    public class PluginLoader
    {
        private readonly string publicKeyXml;

        public PluginLoader(string publicKeyXml)
        {
            this.publicKeyXml = publicKeyXml;
        }

        /// <summary>
        /// Scans the folder for *.dll plugin files and a matching
        /// *.manifest.xml side-car. Loads only those that verify.
        /// </summary>
        public List<PluginLoadEntry> LoadFromFolder(string folder)
        {
            var results = new List<PluginLoadEntry>();
            if (!Directory.Exists(folder)) return results;

            foreach (var dll in Directory.GetFiles(folder, "*.dll"))
                results.Add(LoadFile(dll));

            return results;
        }

        /// <summary>
        /// Verifies and loads a single plugin DLL. Exposed publicly so that the
        /// UI can offer an "Add plugin..." file dialog in addition to the
        /// automatic folder scan required by the task.
        /// </summary>
        public PluginLoadEntry LoadFile(string dll)
        {
            var entry = new PluginLoadEntry { FileName = Path.GetFileName(dll) };
            try
            {
                var manifestPath = dll + ".manifest.xml";
                if (!File.Exists(manifestPath))
                {
                    entry.Message = "Manifest file is missing.";
                    return entry;
                }

                var manifest = PluginManifest.Load(manifestPath);
                var verification = PluginSignature.Verify(dll, manifest, publicKeyXml);
                if (!verification.IsValid)
                {
                    entry.Message = "Rejected: " + verification.Reason;
                    return entry;
                }

                var assembly = Assembly.LoadFrom(dll);
                var types = assembly.GetTypes();

                // Filter by parameterless ctor: an assembly may legitimately
                // contain helper classes that implement the plugin interface
                // but require constructor arguments (e.g. the inner adapter
                // class that wraps a foreign plugin). Only the "entry" class
                // with a public no-arg ctor is meant to be loaded directly.
                var typePluginType = types.FirstOrDefault(t =>
                    typeof(IMediaItemPlugin).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface
                    && t.GetConstructor(Type.EmptyTypes) != null);
                var procPluginType = types.FirstOrDefault(t =>
                    typeof(IProcessingPlugin).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface
                    && t.GetConstructor(Type.EmptyTypes) != null);

                if (typePluginType == null && procPluginType == null)
                {
                    entry.Message = "No plugin implementation found.";
                    return entry;
                }

                if (typePluginType != null)
                    entry.TypePlugin = (IMediaItemPlugin)Activator.CreateInstance(typePluginType);
                if (procPluginType != null)
                    entry.ProcessingPlugin = (IProcessingPlugin)Activator.CreateInstance(procPluginType);

                entry.Loaded = true;
                var labels = new List<string>();
                if (entry.TypePlugin != null) labels.Add($"types: {entry.TypePlugin.Name}");
                if (entry.ProcessingPlugin != null) labels.Add($"processor: {entry.ProcessingPlugin.Name}");
                entry.Message = "Loaded (" + string.Join(", ", labels) + ").";
            }
            catch (Exception ex)
            {
                entry.Message = "Load error: " + ex.Message;
            }

            return entry;
        }
    }
}
