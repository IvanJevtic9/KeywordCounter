namespace KeyWordCounterApp.CLICommand
{
    public class AddDirectoryCommand : CommandCLI
    {
        public override string Name => "ad";
        public override string Description => "Usage: ad <valid_directory_path>\nCommand for adding new directory path in list that directory crawler is going to search.";

        public override Action<string[]> HandleCommand => (commands) =>
        {
            try
            {
                var dir = commands[1].Trim();

                // Some example
                CLI.Instance.ConsoleLog("Starting file scan for file|corpus_a2", consoleColor: ConsoleColor.DarkGreen);
                CLI.Instance.ConsoleLog("Starting file scan for file|corpus_b", consoleColor: ConsoleColor.DarkGreen);
            }
            catch
            {
                InvalidCommandLog("Wrong usage");
            }
        };
    }
}
