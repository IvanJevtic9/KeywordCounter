using KeyWordCounterApp.Implementation;

namespace KeyWordCounterApp.CLICommand
{
    public class AddDirectoryCommand : CommandCLI
    {
        public override string Name => "ad";
        public override string Description =>
            $"Usage: {Name} <path>\n" +
            "Description: Command for adding new directory path that dir crawler is going to visit during his search.";

        public override Action<string[]> HandleCommand => (commands) =>
        {
            try
            {
                var dir = string.Join(' ', commands[1..]);

                var success = DirectoryCrawler.Instance.AddDir(dir, out string errorMessage);

                if (!success)
                {
                    InvalidCommandLog(errorMessage, false);
                }
            }
            catch
            {
                InvalidCommandLog("Wrong usage");
            }
        };
    }
}