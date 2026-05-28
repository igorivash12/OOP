using System.Collections.Generic;

namespace Lab6.Core.Plugins
{
    /// <summary>
    /// Ordered list of <see cref="IProcessingPlugin"/> applied in sequence on
    /// save and in reverse on load. The host keeps a single instance and asks
    /// it to wrap raw BSON bytes before writing them to disk.
    /// </summary>
    public class ProcessingPipeline
    {
        private readonly List<IProcessingPlugin> plugins = new List<IProcessingPlugin>();

        public IReadOnlyList<IProcessingPlugin> Plugins => plugins;

        public void Register(IProcessingPlugin plugin)
        {
            if (plugin != null) plugins.Add(plugin);
        }

        /// <summary>
        /// Applies every enabled plugin's save-side transform in registration
        /// order. Disabled plugins are skipped so users can turn parts of the
        /// pipeline off without recompiling.
        /// </summary>
        public byte[] RunOnSave(byte[] input)
        {
            var data = input;
            foreach (var p in plugins)
            {
                if (p.Enabled) data = p.ProcessOnSave(data);
            }
            return data;
        }

        /// <summary>
        /// Reverses the pipeline. Crucial that plugins are walked in reverse
        /// to undo transforms in last-in-first-out order.
        /// </summary>
        public byte[] RunOnLoad(byte[] input)
        {
            var data = input;
            for (int i = plugins.Count - 1; i >= 0; i--)
            {
                if (plugins[i].Enabled) data = plugins[i].ProcessOnLoad(data);
            }
            return data;
        }
    }
}
