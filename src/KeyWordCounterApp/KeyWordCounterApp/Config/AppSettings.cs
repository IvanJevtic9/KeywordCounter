using Newtonsoft.Json;

namespace KeyWordCounterApp.Config
{
    public class AppSetings : IDisposable
    {
        private const string FileName = "appSettings.json";

        public string[] KeyWords { get; init; }
        public string DirPrefix { get; init; }
        public int CrawlerSleepTime { get; init; }
        public int FileSizeLimit { get; init; }
        public int HopCount { get; init; }
        public int UrlRefreshTime { get; init; }

        private AppSetings()
        {
            using (var streamReader = new StreamReader(FileName))
            {
                var content = streamReader.ReadToEnd();
                var settings = JsonConvert.DeserializeObject<AppSetings>(content);

                KeyWords = settings.KeyWords;
                DirPrefix = settings.DirPrefix;
                CrawlerSleepTime = settings.CrawlerSleepTime;
                FileSizeLimit = settings.FileSizeLimit;
                HopCount = settings.HopCount;
                UrlRefreshTime = settings.UrlRefreshTime;
            }
        }

        private static Lazy<AppSetings> _lazyAppSetings => new Lazy<AppSetings>(() => new AppSetings());

        public static AppSetings Instance => _lazyAppSetings.Value;

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
