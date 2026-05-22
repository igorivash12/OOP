using System;
using System.Windows.Forms;
using Lab5.Services;

namespace Lab5
{
    /// <summary>
    /// Application entry point. Loads game and persistence plugins before the main form starts.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Main entry. Optional first argument: plugin DLL name (e.g. Plugin.BoardGame.dll).
        /// Without arguments, all DLLs from the Plugins folder are loaded automatically.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Game-type plugins (extend entity hierarchy) — Lab 4 functionality
            PluginLoader.Initialize(args);

            // XML persistence plugins (transform data before save / after load) — Lab 5, variant 4
            PersistencePluginLoader.Initialize();

            Application.Run(new Form1());
        }
    }
}
