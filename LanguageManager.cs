using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace CountAndSortWinFormsAppNetFr4
{
    public static class LanguageManager
    {
        private static readonly Dictionary<string, CultureInfo> LanguageMappings = new Dictionary<string, CultureInfo>
    {
        {"English", new CultureInfo("en")},
        {"Slovensky", new CultureInfo("sk")},
        {"Česky", new CultureInfo("cs")},
        {"Deutsch", new CultureInfo("de")},
        {"Polski", new CultureInfo("pl")},
        {"Magyar", new CultureInfo("hu")},
        {"Українська", new CultureInfo("uk")}
    };

        public static void ChangeLanguage(string language)
        {
            if (LanguageMappings.ContainsKey(language))
            {
                Thread.CurrentThread.CurrentUICulture = LanguageMappings[language];
                Thread.CurrentThread.CurrentCulture = LanguageMappings[language];
            }
        }
    }
}
