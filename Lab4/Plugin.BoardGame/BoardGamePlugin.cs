using Lab4.Models;
using Lab4.Plugins;

namespace Plugin.BoardGame
{
    /// <summary>
    /// Entry point for the board-game plugin module loaded by PluginLoader.
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
