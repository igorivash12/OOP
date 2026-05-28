using System.Collections.Generic;
using Lab6.Core.Plugins;

namespace Lab6.Core.Patterns
{
    /// <summary>
    /// Singleton facade over the plugin subsystem. The host code, the
    /// settings dialog and any future tooling all reach for
    /// <see cref="PluginManager.Instance"/> instead of passing pipeline and
    /// registry references around.
    ///
    /// Pattern justification: the plugin set is process-wide state (loaded
    /// assemblies cannot be unloaded without an AppDomain shuffle), so
    /// creating a second instance would silently lie about the system. A
    /// Singleton enforces the single shared view and gives us a single
    /// place to add cross-cutting concerns like logging.
    ///
    /// Implementation uses the standard double-check lazy idiom; the
    /// <see cref="Lazy{T}"/> form would also work but the explicit lock
    /// makes the lifecycle obvious for educational purposes.
    /// </summary>
    public sealed class PluginManager
    {
        private static readonly object gate = new object();
        private static PluginManager instance;

        public ProcessingPipeline Pipeline { get; } = new ProcessingPipeline();
        public List<string> Log { get; } = new List<string>();

        private PluginManager() { }

        public static PluginManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (gate)
                    {
                        if (instance == null) instance = new PluginManager();
                    }
                }
                return instance;
            }
        }

        /// <summary>Wraps a plugin in the Decorator and adds it to the pipeline.</summary>
        public void RegisterProcessingPlugin(IProcessingPlugin plugin)
        {
            // Decorator pattern: every registered plugin is invisibly wrapped
            // so the pipeline always works with the same instrumented type.
            var wrapped = new LoggingProcessingPlugin(plugin, Log.Add);
            Pipeline.Register(wrapped);
        }
    }
}
