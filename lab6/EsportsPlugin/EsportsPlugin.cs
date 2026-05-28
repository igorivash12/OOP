using System;
using System.Collections.Generic;
using Lab6.Core;
using Lab6.Core.Plugins;

namespace EsportsPlugin
{
    /// <summary>Competitive video game record.</summary>
    public class Esports : MediaItem
    {
        /// <summary>Game title, e.g. CS, Dota 2.</summary>
        public string Game { get; set; }

        /// <summary>Tournament prize pool in USD.</summary>
        public int PrizePoolUsd { get; set; }

        public override string TypeName => "Esports";
        public override string GetDetails() => $"Game: {Game}, Prize: ${PrizePoolUsd}";
    }

    /// <summary>Mixed martial arts record.</summary>
    public class MMA : MediaItem
    {
        /// <summary>Weight class of the fight (lbs).</summary>
        public int WeightClassLbs { get; set; }

        /// <summary>Number of rounds scheduled.</summary>
        public int Rounds { get; set; }

        public override string TypeName => "MMA";
        public override string GetDetails() => $"Weight: {WeightClassLbs} lbs, Rounds: {Rounds}";
    }

    /// <summary>Plugin entry point exposing two new sport types.</summary>
    public class EsportsPluginEntry : IMediaItemPlugin
    {
        public string Name => "Esports & MMA";
        public string Version => "1.0.0";

        public IEnumerable<MediaItemTypeDescriptor> GetTypeDescriptors()
        {
            yield return new MediaItemTypeDescriptor("Esports", (t, c, y) => new Esports
            {
                Title = t, Creator = c, Year = y, Game = "CS2", PrizePoolUsd = 1000000
            });
            yield return new MediaItemTypeDescriptor("MMA", (t, c, y) => new MMA
            {
                Title = t, Creator = c, Year = y, WeightClassLbs = 170, Rounds = 3
            });
        }

        public IEnumerable<Type> GetItemTypes()
        {
            yield return typeof(Esports);
            yield return typeof(MMA);
        }
    }
}
