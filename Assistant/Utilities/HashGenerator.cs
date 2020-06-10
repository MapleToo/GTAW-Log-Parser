using System;
using System.Text;
using System.Windows;
using Assistant.Localization;
using System.Security.Cryptography;

namespace Assistant.Utilities
{
    public static class HashGenerator
    {
        /// <summary>
        /// Saves the MD5 hash of the given chat log to the
        /// settings and displays a warning message if the
        /// same chat log has been parsed {X} or more times
        /// </summary>
        /// <param name="log"></param>
        public static void SaveParsedHash(string log)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                string hash = GetMd5Hash(md5Hash, log);
                string lastAutoHash = Properties.Settings.Default.LastParsedAutoHash;

                Properties.Settings.Default.SameHashAutoCount = lastAutoHash == hash ? Properties.Settings.Default.SameHashAutoCount + 1 : 1;
                Properties.Settings.Default.LastParsedAutoHash = hash;
                Properties.Settings.Default.Save();

                if (Properties.Settings.Default.SameHashAutoCount >= Properties.Settings.Default.SameHashWarnAmount)
                    MessageBox.Show(string.Format(Strings.SameHashWarning, Properties.Settings.Default.SameHashWarnAmount), Strings.Warning, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Returns the MD5 hash of
        /// the given string parameter
        /// </summary>
        /// <param name="md5Hash"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        // ReSharper disable once SuggestBaseTypeForParameter
        private static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();

            // ReSharper disable once ForCanBeConvertedToForeach
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        /// <summary>
        /// Returns: true, of the given string parameter
        /// is equal to the given MD5 hash parameter
        ///         false, otherwise
        /// </summary>
        /// <param name="md5Hash"></param>
        /// <param name="input"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        public static bool IsSameMd5Hash(MD5 md5Hash, string input, string hash)
        {
            string hashOfInput = GetMd5Hash(md5Hash, input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(hashOfInput, hash) == 0;
        }
    }
}
