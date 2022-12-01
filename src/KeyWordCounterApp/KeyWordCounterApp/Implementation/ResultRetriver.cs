using KeyWordCounterApp.Implementation.Jobs;
using KeyWordCounterApp.Models;
using System.Text;

namespace KeyWordCounterApp.Implementation
{
    public class ResultRetriver
    {
        private static Lazy<ResultRetriver> _resultRetriver = new Lazy<ResultRetriver>(() => new ResultRetriver());

        private Mutex _resultMutex = new();
        private Dictionary<string, (ScanStatus Status, ScanType Type, Result Result)> _result;

        public ResultRetriver()
        {
            _result = new();
        }

        public static ResultRetriver Instance => _resultRetriver.Value;

        public void InitializeJobResult(IScanningJob scanningJob)
        {
            var hasLock = _resultMutex.WaitOne();
            try
            {
                _result.Add(scanningJob.Name, (ScanStatus.IN_PROGRESS, scanningJob.GetJobType(), new Result(0, 0, 0)));
            }
            finally
            {
                if (hasLock) _resultMutex.ReleaseMutex();
            }
        }

        public void InsertResult(string name, Result result)
        {
            var resultLock = _resultMutex.WaitOne();
            try
            {
                if (!_result.ContainsKey(name))
                {
                    return;
                }

                var r = _result[name];
                _result[name] = (r.Status, r.Type, r.Result + result);
            }
            finally
            {
                if (resultLock) _resultMutex.ReleaseMutex();
            }
        }

        public void UpdateScanStatus(string name, ScanStatus status)
        {
            var resultLock = _resultMutex.WaitOne();
            try
            {
                if (!_result.ContainsKey(name))
                {
                    return;
                }

                var r = _result[name];
                _result[name] = (status, r.Type, r.Result);
            }
            finally
            {
                if (resultLock) _resultMutex.ReleaseMutex();
            }
        }

        public string ListFileResult()
        {
            var result = "";
            foreach(var key in _result.Keys)
            {
                if (_result[key].Type == ScanType.FILE) result += $"{key} ";
            }
            return result;
        }

        public string GetResult(string name)
        {
            var hasLock = _resultMutex.WaitOne();

            if (!_result.ContainsKey(name))
            {
                if (hasLock) _resultMutex.ReleaseMutex();
                return $"Job {name} result not found.";
            }

            var value = _result[name];
            if (hasLock) _resultMutex.ReleaseMutex();

            if (value.Status == ScanStatus.IN_PROGRESS)
            {
                return "Job status is still in progress.";
            }

            var builder = new StringBuilder();
            
            builder.AppendLine($"\n{Constants.GetStarBorder(25)} {value.Type.ToString()} - Summary {Constants.GetStarBorder(25)}");
            builder.AppendLine($"{name} {value.Result.ToString()}");
            builder.Append($"{Constants.GetStarBorder(66)}");

            return builder.ToString();
        }
    }
}
