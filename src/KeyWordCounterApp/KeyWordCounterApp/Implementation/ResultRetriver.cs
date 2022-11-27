using KeyWordCounterApp.Models;
using System.Collections.Concurrent;

namespace KeyWordCounterApp.Implementation
{
    public class ResultRetriver
    {
        private Lazy<ResultRetriver> _resultRetriver = new Lazy<ResultRetriver>(() => new ResultRetriver());

        private ConcurrentDictionary<string, (ScanStatus Status, ScanType Type, Result Result)> Result;

        public ResultRetriver()
        {
            Result = new();
        }

        public ResultRetriver Instance => _resultRetriver.Value;

        //public void RegisterJob();
    }
}
