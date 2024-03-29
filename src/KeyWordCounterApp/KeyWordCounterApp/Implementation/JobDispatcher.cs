﻿using KeyWordCounterApp.Implementation.Jobs;
using System.Collections.Concurrent;

namespace KeyWordCounterApp.Implementation
{
    public class JobDispatcher : IDisposable
    {
        private bool sleepMode = true;
        private static Lazy<JobDispatcher> _instance = new Lazy<JobDispatcher>(() => new JobDispatcher());

        private ConcurrentQueue<IScanningJob> _jobQueue;
        private ConcurrentDictionary<string, DateTime> _executeHistory;

        public JobDispatcher()
        {
            _jobQueue = new();
            _executeHistory = new();
        }

        public static JobDispatcher Instance => _instance.Value;

        public void CreateAJob(IScanningJob job)
        {
            _jobQueue.Enqueue(job);
            CLI.Instance.LogToFile($"Created a new job of type {job.GetJobType()} - {job.Name}", nameof(JobDispatcher));

            if (sleepMode)
            {
                Task.Factory.StartNew(() => Run(Program.Source.Token));
            }
        }

        public async void Run(CancellationToken cancellationToken)
        {
            sleepMode = false;

            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                var success = _jobQueue.TryDequeue(out IScanningJob job);

                if (!success)
                {
                    sleepMode = true;
                    break;
                }

                success = _executeHistory.TryGetValue(job.Name, out DateTime lastExecuteTime);
                if (success)
                {
                    lastExecuteTime = lastExecuteTime.AddMinutes(10);
                    if (DateTime.Compare(DateTime.Now, lastExecuteTime) < 0)
                    {
                        continue;
                    }
                }

                _executeHistory.AddOrUpdate(job.Name, DateTime.Now, (key, value) => DateTime.Now);
                await job.ExecuteJob();
            }
        }

        public void Dispose()
        {
            if (Instance != null)
            {
                CLI.Instance.ConsoleLog($"{nameof(JobDispatcher)} shutted down.", nameof(JobDispatcher), consoleColor: ConsoleColor.Green);
                _instance = null;

                GC.SuppressFinalize(this);
                Thread.Sleep(500);
            }
        }
    }
}
