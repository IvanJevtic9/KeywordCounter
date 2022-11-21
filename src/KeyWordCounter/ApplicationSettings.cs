using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KeyWordCounter
{
    public class ApplicationSettings
    {
        private const string settingsPath = @"D:\source\Git\KeywordCounter\src\KeyWordCounter\appSettings.json";
        public List<string> KeyWords { get; }
        public string DirPrefix { get; }
        public int ClawlerSleepTime { get; }
        public int FileSizeLimit { get; }
        public int HopCount { get; }
        public int UrlRefreshTime { get; }

        private ApplicationSettings(string settingsPath)
        {
            using (StreamReader sr = new StreamReader(settingsPath))
            {
                string settings = sr.ReadToEnd();

                var appSettings = JObject.Parse(settings);

                DirPrefix = (string)appSettings["dirPrefix"];
                ClawlerSleepTime = (int)appSettings["clawlerSleepTime"];
                FileSizeLimit = (int)appSettings["fileSizeLimit"];
                HopCount = (int)appSettings["hopCount"];
                UrlRefreshTime = (int)appSettings["urlRefreshTime"];
                KeyWords = new List<string>();

                Array keyWords = appSettings["keyWords"].ToArray();
                for (int i = 0; i < keyWords.Length; ++i)
                {
                    KeyWords.Add(keyWords.GetValue(i).ToString());
                }
            }
        }

        private static Lazy<ApplicationSettings> instance = new Lazy<ApplicationSettings>(() => new ApplicationSettings(settingsPath));
        public static ApplicationSettings Instance => instance.Value;
    }
}
