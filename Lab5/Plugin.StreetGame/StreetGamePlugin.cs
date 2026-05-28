using Lab5.Models;
using Lab5.Plugins;

namespace Plugin.StreetGame
{
    public class StreetGamePlugin : IGamePlugin
    {
        public string TypeKey => "StreetGame";
        public string DisplayName => "Street Game";
        public BaseEntity CreateInstance()
        {
            return new StreetGame();
        }
    }
}
