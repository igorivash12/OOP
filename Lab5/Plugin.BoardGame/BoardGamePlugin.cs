using Lab5.Models;
using Lab5.Plugins;

namespace Plugin.BoardGame
{
    /// <summary>
    /// Registers the BoardGame type with the host application plugin loader.
    /// </summary>
    public class BoardGamePlugin : IGamePlugin
    {
        public string TypeKey
        {
            get { return "BoardGame"; }
        }

        public string DisplayName
        {
            get { return "Board Game (plugin)"; }
        }

        /// <summary>Factory method invoked by the host application.</summary>
        public BaseEntity CreateInstance()
        {
            return new BoardGame();
        }
    }
}
