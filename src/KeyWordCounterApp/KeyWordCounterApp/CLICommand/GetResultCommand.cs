using KeyWordCounterApp.Implementation;

namespace KeyWordCounterApp.CLICommand
{
    public class GetResultCommand : CommandCLI
    {
        public override string Name => "get";

        public override string Description =>
            "Usage: get <result_name>\n" +
            "Get sumamry of scanning job";

        public override Action<string[]> HandleCommand => (commands) =>
        {
            try
            {
                var name = commands[1].Trim();

                var result = ResultRetriver.Instance.GetResult(name);

                CLI.Instance.ConsoleLog(result, consoleColor: ConsoleColor.Yellow);
            }
            catch
            {
                InvalidCommandLog("Wrong usage");
            }
        };
    }
}
