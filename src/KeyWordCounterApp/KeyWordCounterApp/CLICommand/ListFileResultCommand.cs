using KeyWordCounterApp.Implementation;

namespace KeyWordCounterApp.CLICommand
{
    public class ListFileResultCommand : CommandCLI
    {
        public override string Name => "file_ls";

        public override string Description =>
            "Usage: file_ls\n" +
            "Get list of present file result.";

        public override Action<string[]> HandleCommand => (commands) =>
        {
            try
            {
                var result = ResultRetriver.Instance.ListFileResult();

                if (!string.IsNullOrEmpty(result))
                {
                    CLI.Instance.ConsoleLog(result, consoleColor: ConsoleColor.Yellow);
                }
            }
            catch
            {
                InvalidCommandLog("Wrong usage");
            }
        };
    }
}
