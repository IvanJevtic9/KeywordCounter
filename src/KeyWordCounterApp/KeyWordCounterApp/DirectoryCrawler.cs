namespace KeyWordCounterApp
{
    public class DirectoryCrawler : IDisposable
    {
        private static Lazy<DirectoryCrawler> _directoryCrawler = new Lazy<DirectoryCrawler>(() => new DirectoryCrawler());

        private static HashSet<string> _directories;
        private static HashSet<string> _searchedDirectories;

        private DirectoryCrawler()
        {
            _directories = new HashSet<string>();
            _searchedDirectories = new HashSet<string>();
        }

        public static DirectoryCrawler Instance => _directoryCrawler.Value;

        public static void Search(CancellationToken cancellationToken)
        {
            while (true)
            {

            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
