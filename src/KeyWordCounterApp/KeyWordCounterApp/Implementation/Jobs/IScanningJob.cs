using KeyWordCounterApp.Models;

namespace KeyWordCounterApp.Implementation.Jobs
{
    public interface IScanningJob
    {
        public string Name { get; init; }

        ScanType GetJobType();
        public string GetQuery();
        public Task ExecuteJob();
    }
}
