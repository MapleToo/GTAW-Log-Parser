using System.Linq;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;

namespace Parser.Controllers
{
    public static class LocalizationController
    {
        private static string currentLanguage = string.Empty;
        public enum Language {English, Spanish}

        // Link enum values to language codes
        private static readonly Dictionary<Language, string> Languages = new Dictionary<Language, string>
        {
            { Language.English, "en-US" },
            { Language.Spanish, "es-ES" }
        };

        /// <summary>
        /// Changes the current thread's UI culture to the one in @currentLanguage 
        /// if it is not empty, otherwise grabs it from the settings. 
        /// Optionally saves @currentLanguage to the settings
        /// </summary>
        /// <param name="save"></param>
        public static void InitializeLocale(bool save = false)
        {
            if (string.IsNullOrWhiteSpace(currentLanguage))
                currentLanguage = Properties.Settings.Default.LanguageCode;

            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(currentLanguage);

            if (!save) return;
            Properties.Settings.Default.LanguageCode = currentLanguage;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Returns the @currentLanguage
        /// </summary>
        /// <returns></returns>
        public static string GetLanguage()
        {
            return currentLanguage;
        }

        /// <summary>
        /// Sets the @currentLanguage to a given language
        /// Defaults to English if the language has no key
        /// in the @Languages dictionary
        /// </summary>
        /// <param name="language"></param>
        /// <param name="save"></param>
        public static void SetLanguage(Language language, bool save = true)
        {
            if (!Languages.ContainsKey(language))
                language = Language.English;

            currentLanguage = Languages[language];
            InitializeLocale(save);
        }

        /// <summary>
        /// Returns a string representation of the current
        /// language found in the @Languages dictionary
        /// based on a given language code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetLanguageFromCode(string code)
        {
            return Languages.FirstOrDefault(x => x.Value == code).Key.ToString();
        }

        /// <summary>
        /// Returns the language code corresponding
        /// to the given language if it is found in
        /// the @Languages dictionary. Defaults to English
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        public static string GetCodeFromLanguage(Language language)
        {
            if (!Languages.ContainsKey(language))
                language = Language.English;

            return Languages[language];
        }
    }
}
