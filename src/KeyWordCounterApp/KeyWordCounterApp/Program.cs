using KeyWordCounterApp.Config;
using KeyWordCounterApp.Implementation;
using KeyWordCounterApp.Models;

public partial class Program
{
    public static AppSettings AppSettings { get; } = new AppSettings();
    public static CancellationTokenSource Source { get; } = new CancellationTokenSource();

    private static void Instance_StopApplication(object sender, StopApplicationArgs e)
    {
        Source.Cancel();

        DirectoryCrawler.Instance.Dispose();
        CLI.Instance.Dispose();

        Environment.Exit(0);
    }

    static void Main(string[] args)
    {
        AppSettings.Load();
        CLI.Instance.StopApplication += Instance_StopApplication;

        Task.Factory.StartNew(() => DirectoryCrawler.Instance.Search(Source.Token));
        Task.Factory.StartNew(() => JobDispatcher.Instance.Run(Source.Token));
        CLI.Instance.RunApplicationCLI();
    }
}

// ad D:\source\Git\KeywordCounter\src\KeyWordCounterApp\KeyWordCounterApp\corpus_A\
// corpus_A - Copy
