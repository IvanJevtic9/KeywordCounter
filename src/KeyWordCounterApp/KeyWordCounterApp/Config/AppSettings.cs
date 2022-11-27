using System.Text.Json;

namespace KeyWordCounterApp.Config
{
    public class AppSettings
    {
        private const string FileName = "appSettings.json";

        public string[] KeyWords { get; set; }
        public string DirPrefix { get; set; }
        public int CrawlerSleepTime { get; set; }
        public int FileSizeLimit { get; set; }
        public int HopCount { get; set; }
        public int UrlRefreshTime { get; set; }

        public void Load()
        {
            using (var streamReader = new StreamReader(FileName))
            {
                var content = streamReader.ReadToEnd();
                var settings = JsonSerializer.Deserialize<AppSettings>(content);

                KeyWords = settings.KeyWords;
                DirPrefix = settings.DirPrefix;
                CrawlerSleepTime = settings.CrawlerSleepTime;
                FileSizeLimit = settings.FileSizeLimit;
                HopCount = settings.HopCount;
                UrlRefreshTime = settings.UrlRefreshTime;
            }
        }
    }
}
