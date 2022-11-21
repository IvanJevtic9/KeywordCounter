using System.Collections.Concurrent;
using System.Collections.Generic;

namespace KeyWordCounter.Jobs
{
    public class JobsQueue : IQueue
    {
        private Queue<IScanningJob> jobsQueue = new Queue<IScanningJob>();
        public JobsQueue()
        {}

        public bool IsEmpty()
        {
            return !(jobsQueue.Count > 0);
        }

        public IScanningJob DequeueJob()
        {
            IScanningJob job;

            jobsQueue.TryDequeue(out job);

            return job;
        }

        public void EnqueueJob(IScanningJob job)
        {
            //check for limit 
            jobsQueue.Enqueue(job);
        }
    }
}
