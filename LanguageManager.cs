using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace CountAndSortWinFormsAppNetFr4
{
    public static class LanguageManager
    {
        private static Dictionary<string, string> languageCodes = new Dictionary<string, string>()
    {
        {"English", "en"},
        {"Slovensky", "sk"},
        {"Česky", "cs"},
        {"Deutsch", "de"},
        {"Polski", "pl"},
        {"Magyar", "hu"},
        {"Українська", "uk"}
    };

        public static void ChangeLanguage(string language)
        {
            if (languageCodes.ContainsKey(language))
            {
                Thread.CurrentThread.CurrentUICulture =
                    new CultureInfo(languageCodes[language]);
                Thread.CurrentThread.CurrentCulture =
                    new CultureInfo(languageCodes[language]);
            }
        }
    }
}
