using System;
using System.Windows.Forms;
using Lab5.Services;

namespace Lab5
{
    /// <summary>
    /// Application entry point for Lab5.
    /// Loads game-type plugins and XML processing plugins before starting the main form.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Main entry. Optional first argument: plugin DLL name (e.g. Plugin.BoardGame.dll).
        /// Without arguments, all DLLs from the Plugins folder are loaded.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Load game-type plugins (IGamePlugin) and XML pipeline plugins (IXmlPlugin)
            PluginLoader.Initialize(args);

            Application.Run(new Form1());
        }
    }
}
