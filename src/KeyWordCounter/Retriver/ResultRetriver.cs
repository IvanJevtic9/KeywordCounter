using System.Collections.Generic;
using System;
using KeyWordCounter.Jobs;
using System.Text;

namespace KeyWordCounter.Retriver
{
    public class ResultRetriver
    {
        private class CorpusResult
        {
            public bool IsCompleted { get; set; } = false;
            public Dictionary<string, int> Result;

            public CorpusResult()
            {
                Result = new Dictionary<string, int>();
                foreach (var word in ApplicationSettings.Instance.KeyWords)
                {
                    Result.Add(word, 0);
                }
            }

            public void Reset()
            {
                IsCompleted = false;
                Result.Clear();
            }
        }

        private static Dictionary<string, CorpusResult> corpusRecords;
        private static Dictionary<string, CorpusResult> webRecords;

        private ResultRetriver()
        {
            corpusRecords = new Dictionary<string, CorpusResult>();
            webRecords = new Dictionary<string, CorpusResult>();
        }

        public Dictionary<string, int> GetResult(string query)
        {
            throw new NotImplementedException();
        }
        public Dictionary<string, int> QueryResult(string query)
        {
            throw new NotImplementedException();
        }
        public void ClearSummary(ScanType summaryType)
        {
            throw new NotImplementedException();
        }
        public Dictionary<string, Dictionary<string, int>> GetSummary(ScanType summaryType)
        {
            throw new NotImplementedException();
        }
        public Dictionary<string, Dictionary<string, int>> QuerySummary(ScanType summaryType)
        {
            throw new NotImplementedException();
        }
        public void AddCorpusResult(string corpusName, Dictionary<string, int> corpusResult)
        {
            var str = new StringBuilder($"Storing results for {corpusName}\n");
            foreach(var key in corpusResult.Keys)
            {
                str.AppendLine($"{key} - {corpusResult[key]}");
            }

            Console.WriteLine(str.ToString());
        }

        public void SignalForCorpusSearching(string corpusName)
        {
            if (corpusRecords.ContainsKey(corpusName)) corpusRecords[corpusName].Reset();
            else
            {
                corpusRecords.TryAdd(corpusName, new CorpusResult());
            }
        }

        private static Lazy<ResultRetriver> instance = new Lazy<ResultRetriver>(() => new ResultRetriver());
        public static ResultRetriver Instance => instance.Value;
    }
}
