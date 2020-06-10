using System;
using System.Linq;
using System.Threading;
using Parser.Controllers;
using Parser.Localization;
using System.Windows.Forms;

namespace Parser
{
    internal static class Program
    {
        private static bool isRestarted;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // Get the command line arguments and check
            // if the current session is a restart
            string[] args = Environment.GetCommandLineArgs();
            if (args.Any(arg => arg == $"{ProgramController.ParameterPrefix}restart"))
                isRestarted = true;

            // Make sure only one instance is running
            // if the application is not currently restarting
            Mutex mutex = new Mutex(true, "GTAWParserMini", out bool isUnique);
            if (!isUnique && !isRestarted)
            {
                MessageBox.Show(Strings.OtherInstanceRunning, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Initialize the controllers and
            // display the main user form
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (Properties.Settings.Default.FirstStart)
                Properties.Settings.Default.Upgrade();

            LocalizationController.InitializeLocale();
            ProgramController.InitializeServerIp();
            Application.Run(new UI.Main());

            // Don't let the garbage
            // collector touch the Mutex
            GC.KeepAlive(mutex);
        }
    }
}
