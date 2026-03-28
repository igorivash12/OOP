using System;
using System.Collections.Generic;

namespace Lab3.Models
{
    public static class GameFactory
    {
        private static Dictionary<string, Func<BaseEntity>> map =
            new Dictionary<string, Func<BaseEntity>>()
        {
            { "PCGame", () => new PCGame() },
            { "ConsoleGame", () => new ConsoleGame() },
            { "MobileGame", () => new MobileGame() },
            { "OnlineGame", () => new OnlineGame() },
            { "IndieGame", () => new IndieGame() },
            { "VRGame", () => new VRGame() }
        };

        public static BaseEntity Create(string type)
        {
            return map[type]();
        }
    }
}