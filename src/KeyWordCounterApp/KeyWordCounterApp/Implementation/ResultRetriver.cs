using KeyWordCounterApp.Implementation.Jobs;
using KeyWordCounterApp.Models;
using System.Collections.Concurrent;
using System.Text;

namespace KeyWordCounterApp.Implementation
{
    public class ResultRetriver
    {
        private static Lazy<ResultRetriver> _resultRetriver = new Lazy<ResultRetriver>(() => new ResultRetriver());

        private ConcurrentDictionary<string, (ScanStatus Status, ScanType Type, ConcurrentDictionary<int, Result> Result)> Result;

        public ResultRetriver()
        {
            Result = new();
        }

        public static ResultRetriver Instance => _resultRetriver.Value;

        public void InitializeJobResult(IScanningJob scanningJob)
        {
            Result.AddOrUpdate(scanningJob.Name,
                (name) => (ScanStatus.IN_PROGRESS, scanningJob.GetJobType(), new()),
                (name, oldResult) => (ScanStatus.IN_PROGRESS, scanningJob.GetJobType(), new()));
        }

        public void InsertResult(string name, int threadId, Result result)
        {
            var success = Result.TryGetValue(name, out var value);
            if (!success)
            {
                // Log missing job
                return;
            }

            if (value.Result == null)
            {
                value.Result = new();
            }

            value.Result.TryAdd(threadId, result);
        }

        public void UpdateScanStatus(string name, ScanStatus status)
        {
            var success = Result.TryGetValue(name, out var value);
            if (!success)
            {
                // Log missing job
                return;
            }

            value.Status = status;
            Result[name] = value;
        }

        public string GetResult(string name)
        {
            var success = Result.TryGetValue(name, out var value);
            if (!success)
            {
                return "Result does not exist.";
            }

            if(value.Status == ScanStatus.IN_PROGRESS)
            {
                return "Job status is still in progress.";
            }

            var builder = new StringBuilder();
            builder.AppendLine($"{Constants.GetStarBorder(25)} {value.Type.ToString()} - Summary {Constants.GetStarBorder(25)}");
            foreach (var key in value.Result.Keys)
            {
                value.Result.TryGetValue(key, out var result);
                (int a,int b,int c) = result;
                builder.AppendLine("{one=" + a + ", two=" + b + ", three=" + c + "}");
            }
            builder.AppendLine($"{Constants.GetStarBorder(65)}");

            return builder.ToString();
        }
    }
}
