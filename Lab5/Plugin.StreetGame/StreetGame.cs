using Lab5.Models;

namespace Plugin.StreetGame
{
    public class StreetGame : Game
    {
        public StreetGame()
        {
            // Default values; can be customized by the plugin consumer
            Price = 0.0;
            Genre = "Street";
        }
    }
}
