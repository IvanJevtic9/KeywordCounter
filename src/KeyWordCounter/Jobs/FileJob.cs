using KeyWordCounter.Retriver;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KeyWordCounter.Jobs
{
    public class FileJob : IScanningJob
    {
        private int numberOfTasks = 0;
        private ManualResetEvent signal = new ManualResetEvent(false);

        private Mutex corpusMutex = new Mutex();
        private Dictionary<string, int> corpusResult = (new Dictionary<string, int>()).Initialize();

        public DirectoryInfo Corpus { get; }
        public FileJob(DirectoryInfo dir)
        {
            Corpus = dir;
        }

        public string GetQuery()
        {
            throw new NotImplementedException();
        }
        public ScanType GetJobType()
        {
            return ScanType.FILE;
        }

        public Task<Dictionary<string, int>> Initiate()
        {
            Console.WriteLine($"Starting file scan for file|{Corpus.Name}");

            var task = new Task<Dictionary<string, int>>(() => Scan());
            task.Start();

            return task;
        }

        private Dictionary<string, int> Scan()
        {
            ResultRetriver.Instance.SignalForCorpusSearching(Corpus.Name);

            /*-----------------------------------------------------------*/ //D:\source\Git\KeywordCounter\src\KeyWordCounter\corpus_A\
            var files = Corpus.GetFiles();

            long size = 0;
            List<string> filePaths = new List<string>();

            var threadBag = new List<List<string>>();

            for (int i = 0; i < files.Length; i++)
            {
                if (size + files[i].Length <= ApplicationSettings.Instance.FileSizeLimit)
                {
                    size += files[i].Length;
                    filePaths.Add(files[i].FullName);
                }
                else
                {
                    threadBag.Add(new List<string>(filePaths));
                    size = files[i].Length;
                    filePaths.Clear();
                    filePaths.Add(files[i].FullName);
                }

            }

            if (filePaths.Count > 0) threadBag.Add(filePaths);

            numberOfTasks = threadBag.Count;

            foreach (var item in threadBag)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(Consume), item);
            }

            signal.WaitOne();

            ResultRetriver.Instance.AddCorpusResult(Corpus.Name, corpusResult);

            return corpusResult;
        }

        private void Consume(Object obj)
        {
            var files = (List<string>)obj;
            var localResult = (new Dictionary<string, int>()).Initialize();
            var keys = ApplicationSettings.Instance.KeyWords;

            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} Consuming");

            for (int i = 0; i < files.Count; i++)
            {
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} scanning {files[i]}");
                using (StreamReader sr = new StreamReader(files[i]))
                {
                    string text = sr.ReadToEnd();

                    foreach (var word in text.Split(new string[3] { " ", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        foreach (var key in keys)
                        {
                            if (key == word) localResult[key]++;
                        }
                    }
                }
            }

            bool haveLock = corpusMutex.WaitOne();
            try
            {
                foreach (var key in localResult.Keys)
                {
                    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} - {key} adding {localResult[key]} on {corpusResult[key]}");
                    corpusResult[key] += localResult[key];
                }
            }
            finally
            {
                if (haveLock) corpusMutex.ReleaseMutex();
            }

            if (Interlocked.Decrement(ref numberOfTasks) == 0) signal.Set();
        }
    }
}
