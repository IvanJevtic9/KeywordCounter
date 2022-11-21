namespace KeyWordCounter.Jobs
{
    public class JobDispatcher
    {
        private readonly IQueue queue;

        public JobDispatcher(IQueue queue)
        {
            this.queue = queue;
        }

        public void Run()
        {
            // TODO cancellation token
            while (true)
            {
                try
                {
                    // TODO Mutex lock if queue is empty - blocking thread if queue is empty
                    
                    var job = queue.DequeueJob();
                    if(job != null) job.Initiate();
                }
                finally
                {
                    // TODO mutex sync
                }
            }
        }
    }
}
