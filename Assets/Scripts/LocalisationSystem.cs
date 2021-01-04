using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class LocalizationSystem
    {
        public enum Language
        {
            English,
            French,
            Japanese
        }

        public static string[] LanguageSuffix = new string[] { "en", "fr", "jp" };

        public static Language language = Language.English;

        private static Dictionary<int, string> localizedText = null;

        public static void Init()
        {
            CSVLoader csvLoader = new CSVLoader();
            csvLoader.loadCSV();

            localizedText = csvLoader.getDictionaryValues("localisation_" + LanguageSuffix[(int)language]);
        }

        public static string getLocalisedValue(string key)
        {
            if (localizedText == null)
            {
                Init();
            }

            string value;
            if (localizedText.TryGetValue(key.GetHashCode(), out value) == false)
            {
                value = $"##{key}##";
            }

            return value;
        }
    }
}
