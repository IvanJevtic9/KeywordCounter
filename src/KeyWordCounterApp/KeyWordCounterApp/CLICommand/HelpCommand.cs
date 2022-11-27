using KeyWordCounterApp.Implementation;

namespace KeyWordCounterApp.CLICommand
{
    public class HelpCommand : CommandCLI
    {
        public override string Name => "help";
        public override string Description =>
            "Usage: help <command>\n" +
            "Available commands: [ad, exit]";
        public override Action<string[]> HandleCommand => (commands) =>
        {
            try
            {
                var comm = CLI.Instance.ParseCommand(commands[1].Trim());

                if (comm is null)
                {
                    InvalidCommandLog("Wrong usage");
                    return;
                }

                CLI.Instance.ConsoleLog(comm.Description, consoleColor: ConsoleColor.DarkGreen);
            }
            catch
            {
                InvalidCommandLog("Wrong usage");
            }
        };
    }
}
