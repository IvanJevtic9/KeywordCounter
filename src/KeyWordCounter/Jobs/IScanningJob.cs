using System.Collections.Generic;
using System.Threading.Tasks;

namespace KeyWordCounter.Jobs
{
    public enum ScanType
    {
        FILE,
        WEB
    }

    public interface IScanningJob
    {
        ScanType GetJobType();
        string GetQuery();
        Task<Dictionary<string, int>> Initiate();
    }
}
