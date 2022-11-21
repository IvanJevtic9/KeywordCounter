namespace KeyWordCounter.Jobs
{
    public interface IQueue
    {
        bool IsEmpty();
        void EnqueueJob(IScanningJob job);
        IScanningJob DequeueJob();
    }
}
