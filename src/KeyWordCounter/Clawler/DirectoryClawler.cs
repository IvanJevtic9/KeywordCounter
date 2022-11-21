using KeyWordCounter.Jobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace KeyWordCounter.Clawler
{
    public class DirectoryClawler : IClawler
    {
        private Dictionary<string, DateTime> fileMap = new Dictionary<string, DateTime>();
        private Queue<DirectoryInfo> dirs = new Queue<DirectoryInfo>();
        private Queue<DirectoryInfo> searchedDirs = new Queue<DirectoryInfo>();
        private IQueue jobsQueue;

        public DirectoryClawler(IQueue jobsQueue)
        {
            this.jobsQueue = jobsQueue;
        }

        private void CorpusSearch(DirectoryInfo root)
        {
            var dirs = root.GetDirectories();

            foreach(var dir in dirs)
            {
                CorpusSearch(dir);
            }

            if (root.Name.StartsWith(ApplicationSettings.Instance.DirPrefix))
            {
                var files = root.GetFiles();

                bool newJob = false;

                foreach(var file in files)
                {
                    if (fileMap.ContainsKey(file.FullName))
                    {
                        if(fileMap[file.FullName] != file.LastWriteTime)
                        {
                            newJob = true;
                            fileMap[file.FullName] = file.LastWriteTime;
                        }
                    }
                    else
                    {
                        newJob = true;
                        fileMap.Add(file.FullName, file.LastWriteTime);
                    }
                }

                if (newJob) jobsQueue.EnqueueJob(new FileJob(root));
            }
        }

        public void AddDir(string path)
        {
            if (!Directory.Exists(path))
            {
                // do logging - Invalid directory path
                return;
            }

            bool isNew = true;
            var directory = new DirectoryInfo(path);

            foreach (var dir in dirs)
            {
                if (dir.Equals(directory))
                {
                    isNew = false;
                    break;
                }
            }

            if (isNew) dirs.Enqueue(directory);
        }

        public void Run()
        {
            while (true)
            {
                // break (looking for cancellation token) - do logging 
                if (dirs.Count == 0)
                {
                    Thread.Sleep(ApplicationSettings.Instance.ClawlerSleepTime);

                    while (searchedDirs.Count > 0) dirs.Enqueue(searchedDirs.Dequeue());
                }
                else
                {
                    DirectoryInfo directory;

                    dirs.TryDequeue(out directory);

                    CorpusSearch(directory);

                    searchedDirs.Enqueue(directory);
                }
            }
        }
    }
}
