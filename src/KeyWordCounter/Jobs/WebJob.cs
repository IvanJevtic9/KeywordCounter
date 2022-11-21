using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KeyWordCounter.Jobs
{
    public class WebJob : IScanningJob
    {
        public string Url { get; }
        public int Hops { get; }

        public WebJob(string url, int hops)
        {
            Url = url;
            Hops = hops;
        }

        public string GetQuery()
        {
            // Query for retriving result of this query.
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, int>> Initiate()
        {
            throw new NotImplementedException();
            //poziv web scanner-a
            //upis u result retriver dobijen iz web scanner
        }

        public ScanType GetJobType()
        {
            return ScanType.WEB;
        }
    }
}
