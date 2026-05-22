using System;
using System.Windows.Forms;
using Lab4.Services;

namespace Lab4
{
    /// <summary>
    /// Application entry point. Loads plugins before starting the main form.
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

            // Dynamic module load — no code changes required when adding new plugin DLLs
            PluginLoader.Initialize(args);

            Application.Run(new Form1());
        }
    }
}
