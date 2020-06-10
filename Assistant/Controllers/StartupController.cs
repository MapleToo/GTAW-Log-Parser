using System;
using System.IO;
using System.Linq;
using System.Windows;
using IWshRuntimeLibrary;
using System.Collections.Generic;

namespace Assistant.Controllers
{
    public static class StartupController
    {
        private static readonly string StartUpDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.Startup)}\\";
        private const string ShortcutName = "gtaw-parser.lnk";

        /// <summary>
        /// Verifies the continuity between the (non) existent shortcut
        /// in the startup directory and the Start With Windows option
        /// </summary>
        public static void InitializeShortcut()
        {
            if (IsAddedToStartup())
            {
                if (!Properties.Settings.Default.BackupChatLogAutomatically)
                {
                    Properties.Settings.Default.StartWithWindows = false;
                    Properties.Settings.Default.Save();

                    TryRemovingFromStartup();
                }
                else if (!Properties.Settings.Default.StartWithWindows)
                {
                    TryRemovingFromStartup();
                }

                if (IsAddedToStartup())
                    CheckIfLegitimate();
            }
            else
            {
                if (Properties.Settings.Default.StartWithWindows)
                {
                    TryAddingToStartup();
                }
            }
        }

        /// <summary>
        /// Adds a shortcut to the startup directory if the
        /// Start With Windows option is enabled and
        /// removes the shortcut if the option is disabled
        /// </summary>
        /// <param name="toggle"></param>
        public static void ToggleStartup(bool toggle)
        {
            if (toggle)
                TryAddingToStartup();
            else
                TryRemovingFromStartup();
        }

        /// <summary>
        /// Deletes all but one shortcut of the
        /// application from the startup directory
        /// </summary>
        private static void CheckIfLegitimate()
        {
            try
            {
                bool legit = true;
                List<FileInfo> parserShortcuts = GetParserShortcuts();

                if (parserShortcuts.Count <= 0) return;
                foreach (FileInfo file in parserShortcuts)
                {
                    if (legit)
                    {
                        WshShell wshShell = new WshShell();

                        if (!(wshShell.CreateShortcut(file.FullName) is IWshShortcut shortcut)) continue;
                        if (shortcut.TargetPath != AppController.ExecutablePath)
                            shortcut.TargetPath = AppController.ExecutablePath;
                        if (!shortcut.Arguments.ToLower()
                            .Contains($"{AppController.ParameterPrefix}minimized"))
                            shortcut.Arguments = $"{AppController.ParameterPrefix}minimized";
                        if (shortcut.WorkingDirectory != AppController.StartupPath)
                            shortcut.WorkingDirectory = AppController.StartupPath;

                        shortcut.Save();
                        legit = false;
                    }
                    else
                    {
                        file.Delete();
                    }
                }
            }
            catch
            {
                // Silent exception
            }
        }

        /// <summary>
        /// Adds a shortcut of the application
        /// to the startup directory
        /// </summary>
        /// <param name="showError"></param>
        private static void TryAddingToStartup(bool showError = true)
        {
            try
            {
                if (IsAddedToStartup())
                    return;

                WshShell wshShell = new WshShell();
                if (!(wshShell.CreateShortcut(StartUpDirectory + ShortcutName) is IWshShortcut shortcut)) return;
                shortcut.TargetPath = AppController.ExecutablePath;
                shortcut.Arguments = $"{AppController.ParameterPrefix}minimized";
                shortcut.WorkingDirectory = AppController.StartupPath;
                shortcut.Save();
            }
            catch
            {
                if (showError)
                    MessageBox.Show(Localization.Strings.AutoStartEnableError, Localization.Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);

                Properties.Settings.Default.StartWithWindows = false;
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// Deletes all shortcuts of the application
        /// from the startup directory
        /// /// </summary>
        /// <param name="showError"></param>
        private static void TryRemovingFromStartup(bool showError = true)
        {
            try
            {
                if (!IsAddedToStartup())
                    return;

                List<FileInfo> parserShortcuts = GetParserShortcuts();

                if (parserShortcuts.Count <= 0) return;
                foreach (FileInfo file in parserShortcuts)
                {
                    file.Delete();
                }
            }
            catch
            {
                if (showError)
                    MessageBox.Show(Localization.Strings.AutoStartDisableError, Localization.Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);

                if (IsAddedToStartup())
                {
                    Properties.Settings.Default.StartWithWindows = true;
                    Properties.Settings.Default.Save();
                }
            }
        }

        /// <summary>
        /// Returns: true, if there is a shortcut of the
        ///                application in the startup directory
        ///         false, otherwise
        /// </summary>
        /// <returns></returns>
        public static bool IsAddedToStartup()
        {
            return GetParserShortcuts().Count > 0;
        }

        /// <summary>
        /// Returns the number of shortcuts of the application
        /// present in the startup directory
        /// </summary>
        /// <returns></returns>
        private static List<FileInfo> GetParserShortcuts()
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(StartUpDirectory);
                FileInfo[] allShortcuts = directory.GetFiles("*.lnk");

                return allShortcuts.Where(file => file.Name.ToLower().Contains(ShortcutName.ToLower())).ToList();
            }
            catch
            {
                return new List<FileInfo>();
            }
        }
    }
}
