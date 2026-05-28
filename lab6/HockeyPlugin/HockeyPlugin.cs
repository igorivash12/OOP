using System;
using System.Collections.Generic;
using Lab6.Core;
using Lab6.Core.Plugins;

namespace HockeyPlugin
{
    /// <summary>Hockey-specific sports record added by the plugin.</summary>
    public class Hockey : MediaItem
    {
        /// <summary>Ice league this team or game belongs to.</summary>
        public string League { get; set; }

        /// <summary>Total goals counted for this record.</summary>
        public int Goals { get; set; }

        public override string TypeName => "Hockey";

        public override string GetDetails()
        {
            return $"League: {League}, Goals: {Goals}";
        }
    }

    /// <summary>
    /// Plugin entry point; the host discovers this type by interface and
    /// instantiates it to enumerate new sport descriptors and types.
    /// </summary>
    public class HockeyPluginEntry : IMediaItemPlugin
    {
        public string Name => "Hockey";
        public string Version => "1.0.0";

        public IEnumerable<MediaItemTypeDescriptor> GetTypeDescriptors()
        {
            yield return new MediaItemTypeDescriptor("Hockey", (title, creator, year) => new Hockey
            {
                Title = title,
                Creator = creator,
                Year = year,
                League = "NHL",
                Goals = 0
            });
        }

        public IEnumerable<Type> GetItemTypes()
        {
            yield return typeof(Hockey);
        }
    }
}
