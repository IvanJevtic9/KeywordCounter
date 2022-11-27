using KeyWordCounterApp.Models;
using System.Text;

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

        public async Task ExecuteJob()
        {
            ResultRetriver.Instance.InitializeJobResult(this);

            var tasks = new List<Task>();

            var dir = new DirectoryInfo(FullName);
            var files = dir.GetFiles();

            long currentChunkSize = 0;
            List<FileInfo> currentChunkFiles = new();
            foreach (var file in files)
            {
                currentChunkSize += file.Length;
                currentChunkFiles.Add(file);
                if (currentChunkSize >= Program.AppSettings.FileSizeLimit)
                {
                    tasks.Add(Task.Factory.StartNew(() => ScanFile(new List<FileInfo>(currentChunkFiles).ToArray())));

                    Thread.Sleep(10);
                    currentChunkFiles = new ();
                    currentChunkSize = 0;
                }
            }

            await Task.Factory.ContinueWhenAll(tasks.ToArray(), (tasks) =>
            {
                ResultRetriver.Instance.UpdateScanStatus(Name, ScanStatus.COMPLETED);
            });
        }

        private void ScanFile(FileInfo[] files)
        {
            int a = 0;
            int b = 0;
            int c = 0;

            if(files.Length > 0)
            {
                for (int index = 0; index < files.Length; index++)
                {
                    using var fs = files[index].OpenRead();
                    using var st = new StreamReader(fs, Encoding.UTF8);

                    string content = st.ReadToEnd();
                    var words = content.Split(' ');

                    for (int i = 0; i < words.Length; i++)
                    {
                        if (words[i] == Program.AppSettings.KeyWords[0]) ++a;
                        else if (words[i] == Program.AppSettings.KeyWords[1]) ++b;
                        else if (words[i] == Program.AppSettings.KeyWords[2]) ++c;
                    }
                }

                ResultRetriver.Instance.InsertResult(Name, Thread.CurrentThread.ManagedThreadId, new Result(a, b, c));
            }
        }
    }
}
