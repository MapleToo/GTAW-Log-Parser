using System;
using MahApps.Metro;
using System.Windows;
using Microsoft.Win32;
using System.Management;
using System.Windows.Media;
using Assistant.Properties;
using System.Security.Principal;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Assistant.Controllers
{
    public static class StyleController
    {
        private const string DefaultLightStyle = "Amber";
        private const string DefaultDarkStyle = "Cyan";

        public static bool DarkMode
        {
            get => Settings.Default.DarkMode;
            set
            {
                Settings.Default.DarkMode = value;
                Settings.Default.Save();
            }
        }

        public static string Style
        {
            get => Settings.Default.Theme;
            set
            {
                Settings.Default.Theme = value;
                Settings.Default.Save();
            }
        }

        public static readonly List<string> ValidStyles = new List<string>
        {
            "Default",
            "Red",
            "Green",
            "Blue",
            "Purple",
            "Orange",
            "Lime",
            "Emerald",
            "Teal",
            "Cyan",
            "Cobalt",
            "Indigo",
            "Violet",
            "Pink",
            "Magenta",
            "Crimson",
            "Amber",
            "Yellow",
            "Brown",
            "Olive",
            "Steel",
            "Mauve",
            "Taupe",
            "Sienna"
        };

        private static ManagementEventWatcher appModeWatcher;
        private static ManagementEventWatcher systemAccentWatcher;

        /// <summary>
        /// Event:
        /// Changes the app mode to light or dark
        /// depending on the registry value
        /// </summary>
        private static void AppModeChanged()
        {
            if (!Settings.Default.FollowSystemMode) return;
            DarkMode = GetAppMode();
            UpdateTheme();
        }

        /// <summary>
        /// Event:
        /// Changes the app accent color
        /// depending on the registry value
        /// </summary>
        private static void SystemAccentChanged()
        {
            if (!Settings.Default.FollowSystemColor) return;
            Style = "Steel";
            UpdateTheme();

            Style = "Windows";
            UpdateTheme();
        }

        /// <summary>
        /// Checks whether or not the application can follow the system
        /// app mode and system accent color through the registry and
        /// starts the value event watchers, if possible
        /// </summary>
        [SuppressMessage("ReSharper", "InvertIf")]
        public static void InitializeFollowEligibility()
        {
            try
            {
                var keyValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", null);
                if (keyValue != null)
                {
                    AppController.CanFollowSystemMode = true;

                    WqlEventQuery appModeQuery = new WqlEventQuery("SELECT * FROM RegistryValueChangeEvent WHERE " +
                    "Hive='HKEY_USERS' " +
                     @"AND KeyPath='" + WindowsIdentity.GetCurrent().User?.Value + @"\\Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize' " +
                     @"AND ValueName='AppsUseLightTheme'");
                    appModeWatcher = new ManagementEventWatcher(appModeQuery);
                    appModeWatcher.EventArrived += (s, args) => AppModeChanged();
                    appModeWatcher.Start();
                }
            }
            catch
            {
                AppController.CanFollowSystemMode = false;
            }

            try
            {
                var keyValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "ColorizationColor", null);
                if (keyValue != null && AppController.CanFollowSystemMode)
                {
                    AppController.CanFollowSystemColor = true;

                    WqlEventQuery systemAccentQuery = new WqlEventQuery("SELECT * FROM RegistryValueChangeEvent WHERE " +
                    "Hive='HKEY_USERS' " +
                     @"AND KeyPath='" + WindowsIdentity.GetCurrent().User?.Value + @"\\Software\\Microsoft\\Windows\\DWM' " +
                     @"AND ValueName='ColorizationColor'");
                    systemAccentWatcher = new ManagementEventWatcher(systemAccentQuery);
                    systemAccentWatcher.EventArrived += (s, args) => SystemAccentChanged();
                    systemAccentWatcher.Start();
                }
            }
            catch
            {
                AppController.CanFollowSystemColor = false;
            }
        }

        /// <summary>
        /// Stops the app mode and system accent color registry watchers
        /// </summary>
        public static void StopWatchers()
        {
            appModeWatcher?.Stop();
            systemAccentWatcher?.Stop();
        }

        /// <summary>
        /// Returns the ideal text color for a given background color
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private static Color GetIdealTextColor(Color color)
        {
            const int nThreshold = 105;
            int bgDelta = Convert.ToInt32(color.R * 0.299 + color.G * 0.587 + color.B * 0.114);
            Color foreColor = 255 - bgDelta < nThreshold ? Colors.Black : Colors.White;
            return foreColor;
        }

        /// <summary>
        /// Returns a SolidColorBrush of the given color and opacity
        /// </summary>
        /// <param name="color"></param>
        /// <param name="opacity"></param>
        /// <returns></returns>
        private static SolidColorBrush GetSolidColorBrush(Color color, double opacity = 1d)
        {
            SolidColorBrush brush = new SolidColorBrush(color) { Opacity = opacity };
            brush.Freeze();
            return brush;
        }

        /// <summary>
        /// Returns a Metro Accent corresponding
        /// to the current system accent color
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0028:Simplify collection initialization", Justification = "<Pending>")]
        private static Accent GetSystemAccent()
        {
            try
            {
                Color color = SystemParameters.WindowGlassColor;
                var keyValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "ColorizationColor", null);
                if (keyValue != null)
                {
                    byte[] bytes = BitConverter.GetBytes((uint)(int)keyValue);
                    color = Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
                }

                // ReSharper disable once UseObjectOrCollectionInitializer
                ResourceDictionary resourceDictionary = new ResourceDictionary();
                resourceDictionary.Add("HighlightColor", color);
                resourceDictionary.Add("AccentBaseColor", color);
                resourceDictionary.Add("AccentColor", Color.FromArgb(204, color.R, color.G, color.B));
                resourceDictionary.Add("AccentColor2", Color.FromArgb(153, color.R, color.G, color.B));
                resourceDictionary.Add("AccentColor3", Color.FromArgb(102, color.R, color.G, color.B));
                resourceDictionary.Add("AccentColor4", Color.FromArgb(51, color.R, color.G, color.B));

                resourceDictionary.Add("HighlightBrush", GetSolidColorBrush((Color)resourceDictionary["HighlightColor"]));
                resourceDictionary.Add("AccentBaseColorBrush", GetSolidColorBrush((Color)resourceDictionary["AccentBaseColor"]));
                resourceDictionary.Add("AccentColorBrush", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));
                resourceDictionary.Add("AccentColorBrush2", GetSolidColorBrush((Color)resourceDictionary["AccentColor2"]));
                resourceDictionary.Add("AccentColorBrush3", GetSolidColorBrush((Color)resourceDictionary["AccentColor3"]));
                resourceDictionary.Add("AccentColorBrush4", GetSolidColorBrush((Color)resourceDictionary["AccentColor4"]));

                resourceDictionary.Add("WindowTitleColorBrush", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));

                resourceDictionary.Add("ProgressBrush", new LinearGradientBrush(
                    new GradientStopCollection(new[]
                    {
                    new GradientStop((Color)resourceDictionary["HighlightColor"], 0),
                    new GradientStop((Color)resourceDictionary["AccentColor3"], 1)
                    }),
                    new Point(1.002, 0.5), new Point(0.001, 0.5)));

                resourceDictionary.Add("CheckmarkFill", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));
                resourceDictionary.Add("RightArrowFill", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));

                resourceDictionary.Add("IdealForegroundColor", GetIdealTextColor(color));
                resourceDictionary.Add("IdealForegroundColorBrush", GetSolidColorBrush((Color)resourceDictionary["IdealForegroundColor"]));
                resourceDictionary.Add("IdealForegroundDisabledBrush", GetSolidColorBrush((Color)resourceDictionary["IdealForegroundColor"], 0.4));
                resourceDictionary.Add("AccentSelectedColorBrush", GetSolidColorBrush((Color)resourceDictionary["IdealForegroundColor"]));

                resourceDictionary.Add("MetroDataGrid.HighlightBrush", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));
                resourceDictionary.Add("MetroDataGrid.HighlightTextBrush", GetSolidColorBrush((Color)resourceDictionary["IdealForegroundColor"]));
                resourceDictionary.Add("MetroDataGrid.MouseOverHighlightBrush", GetSolidColorBrush((Color)resourceDictionary["AccentColor3"]));
                resourceDictionary.Add("MetroDataGrid.FocusBorderBrush", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));
                resourceDictionary.Add("MetroDataGrid.InactiveSelectionHighlightBrush", GetSolidColorBrush((Color)resourceDictionary["AccentColor2"]));
                resourceDictionary.Add("MetroDataGrid.InactiveSelectionHighlightTextBrush", GetSolidColorBrush((Color)resourceDictionary["IdealForegroundColor"]));

                resourceDictionary.Add("MahApps.Metro.Brushes.ToggleSwitchButton.OnSwitchBrush.Win10", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));
                resourceDictionary.Add("MahApps.Metro.Brushes.ToggleSwitchButton.OnSwitchMouseOverBrush.Win10", GetSolidColorBrush((Color)resourceDictionary["AccentColor2"]));
                resourceDictionary.Add("MahApps.Metro.Brushes.ToggleSwitchButton.ThumbIndicatorCheckedBrush.Win10", GetSolidColorBrush((Color)resourceDictionary["IdealForegroundColor"]));

                return ThemeManager.GetAccent(resourceDictionary);
            }
            catch
            {
                AppController.CanFollowSystemColor = false;
                ValidStyles.Remove("Windows");

                Settings.Default.FollowSystemColor = false;
                Settings.Default.Save();

                return ThemeManager.GetAccent(DarkMode ? DefaultDarkStyle : DefaultLightStyle);
            }
        }

        /// <summary>
        /// Returns: true, if the system is using dark mode
        ///         false, if the system is using light mode
        ///                or the system does not have an app mode
        /// </summary>
        /// <returns></returns>
        public static bool GetAppMode()
        {
            bool darkMode = false;
            try
            {
                var keyValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", null);
                if (keyValue != null && (uint)(int)keyValue == 0)
                    darkMode = true;
            }
            catch
            {
                AppController.CanFollowSystemMode = false;

                Settings.Default.FollowSystemMode = false;
                Settings.Default.Save();
            }

            return darkMode;
        }

        /// <summary>
        /// Applies the current app mode and theme
        /// </summary>
        public static void UpdateTheme()
        {
            if (!ValidStyles.Contains(Style))
                Style = "Default";

            Application.Current.Dispatcher?.Invoke(() =>
            {
                ThemeManager.ChangeAppStyle(Application.Current,
                                        Style == "Windows" ? GetSystemAccent() : ThemeManager.GetAccent(Style == "Default" ? DarkMode ? DefaultDarkStyle : DefaultLightStyle : Style),
                                        ThemeManager.GetAppTheme(DarkMode ? "BaseDark" : "BaseLight"));
            });
        }
    }
}
