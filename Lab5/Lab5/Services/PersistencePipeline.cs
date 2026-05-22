using System.Collections.Generic;
using Lab5.Plugins;

namespace Lab5.Services
{
    /// <summary>
    /// Applies enabled persistence plugins to XML text before save and after load.
    /// </summary>
    public static class PersistencePipeline
    {
        /// <summary>
        /// Runs all enabled plugins in registration order on XML produced from entities.
        /// </summary>
        public static string ProcessBeforeSave(string xmlContent)
        {
            IList<IPersistencePlugin> plugins = PersistencePluginRegistry.GetEnabledPluginsInOrder();

            foreach (IPersistencePlugin plugin in plugins)
                xmlContent = plugin.ProcessBeforeSave(xmlContent);

            return xmlContent;
        }

        /// <summary>
        /// Runs all enabled plugins in reverse order to restore XML before deserialization.
        /// </summary>
        public static string ProcessAfterLoad(string xmlContent)
        {
            IList<IPersistencePlugin> plugins = PersistencePluginRegistry.GetEnabledPluginsReverseOrder();

            foreach (IPersistencePlugin plugin in plugins)
                xmlContent = plugin.ProcessAfterLoad(xmlContent);

            return xmlContent;
        }
    }
}
