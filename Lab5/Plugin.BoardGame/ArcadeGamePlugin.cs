using Lab5.Models;
using Lab5.Plugins;

namespace Plugin.BoardGame
{
    /// <summary>
    /// Registers the ArcadeGame type with the host application plugin loader.
    /// </summary>
    public class ArcadeGamePlugin : IGamePlugin
    {
        public string TypeKey
        {
            get { return "ArcadeGame"; }
        }

        public string DisplayName
        {
            get { return "Arcade Game (plugin)"; }
        }

        /// <summary>Factory method invoked by the host application.</summary>
        public BaseEntity CreateInstance()
        {
            return new ArcadeGame();
        }
    }
}
