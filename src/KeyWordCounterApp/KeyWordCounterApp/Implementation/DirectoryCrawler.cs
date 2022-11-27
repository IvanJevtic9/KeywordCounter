using KeyWordCounterApp.Implementation.Jobs;

namespace KeyWordCounterApp.Implementation
{
    public class DirectoryCrawler : IDisposable
    {
        private static Lazy<DirectoryCrawler> _directoryCrawler = new Lazy<DirectoryCrawler>(() => new DirectoryCrawler());

        private Mutex _dirMutex = new Mutex();

        private static HashSet<string> _directories;
        private static HashSet<string> _searchedDirectories;

        private DirectoryCrawler()
        {
            _directories = new HashSet<string>();
            _searchedDirectories = new HashSet<string>();
        }

        public static DirectoryCrawler Instance => _directoryCrawler.Value;

        public bool AddDir(string path, out string errorMessage)
        {
            errorMessage = null;

            if (!Directory.Exists(path))
            {
                errorMessage = "Invalid path.";
                return false;
            }

            bool success;
            var haveLock = _dirMutex.WaitOne();
            try
            {
                success = _directories.Add(path);
                
            }
            finally
            {
                if (haveLock) _dirMutex.ReleaseMutex();
            }

            if (!success)
            {
                errorMessage = "Path already added";
            }

            return success;
        }

        public void Search(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                var haveLock = _dirMutex.WaitOne();                    
                foreach (var directoryPath in _directories)
                {
                    if (haveLock) _dirMutex.ReleaseMutex();
                    
                    BFSDirectorySearch(new DirectoryInfo(directoryPath));
                    
                    haveLock = _dirMutex.WaitOne();
                }

                if (haveLock)
                {
                    _dirMutex.ReleaseMutex();
                }

                _searchedDirectories.Clear();
                Thread.Sleep(Program.AppSettings.CrawlerSleepTime);
            }
        }

        public void Dispose()
        {
            if (Instance != null)
            {
                CLI.Instance.ConsoleLog($"{nameof(DirectoryCrawler)} shutted down.", consoleColor: ConsoleColor.Green);

                _directoryCrawler = null;
                _directories = null;
                _searchedDirectories = null;

                GC.SuppressFinalize(this);

                Thread.Sleep(500);
            }
        }

        private void BFSDirectorySearch(DirectoryInfo root)
        {
            if (_searchedDirectories.Contains(root.FullName))
            {
                return;
            }

            _searchedDirectories.Add(root.FullName);

            if (root.Name.StartsWith(Program.AppSettings.DirPrefix))
            {
                JobDispatcher.Instance.CreateAJob(new FileJob(root.FullName));
            }

            var dirs = root.GetDirectories();

            for(int i = 0; i < dirs.Length; i++)
            {
                BFSDirectorySearch(dirs[i]);
            }
        }
    }
}
