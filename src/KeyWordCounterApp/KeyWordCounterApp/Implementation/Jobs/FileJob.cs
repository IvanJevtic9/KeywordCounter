using KeyWordCounterApp.Models;

namespace KeyWordCounterApp.Implementation.Jobs
{
    public class FileJob : IScanningJob
    {
        public string FullName { get; init; }
        public string Name { get; init; }

        public FileJob(string dirPath)
        {
            var dir = new DirectoryInfo(dirPath);

            FullName = dirPath;
            Name = dir.Name;
        }

        public ScanType GetJobType() => ScanType.FILE;

        public string GetQuery()
        {
            return $"get file|{Name}";
        }

        public void ExecuteJob()
        {
            // Register Job in Result retriver - TODO

            var dir = new DirectoryInfo(FullName);

            var files = dir.GetFiles();

            long currentChunkSize = 0;
            List<FileInfo> currentChunkFiles = new ();
            foreach (var file in files)
            {
                currentChunkSize += file.Length;
                currentChunkFiles.Add(file);
                if (currentChunkSize >= Program.AppSettings.FileSizeLimit)
                {
                    // start execute thread
                    currentChunkFiles.Clear();
                    currentChunkSize = 0;
                }
            }
        }
    }
}
