using Lab5.Models;
using Lab5.Plugins;

namespace Plugin.BoardGame
{
    /// <summary>
    /// Entry point for the board-game plugin module loaded by PluginLoader.
    /// </summary>
    public class BoardGamePlugin : IGamePlugin
    {
        public string TypeKey     => "BoardGame";
        public string DisplayName => "Board Game (plugin)";

        /// <summary>Factory method invoked by the host application.</summary>
        public BaseEntity CreateInstance()
        {
            return new BoardGame();
        }
    }
}
