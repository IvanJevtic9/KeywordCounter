using KeyWordCounterApp.Implementation;
using KeyWordCounterApp.Models;

namespace KeyWordCounterApp.CLICommand
{
    public class ExitCommand : CommandCLI
    {
        public override string Name => "exit";
        public override string Description =>
            "Usage: exit\n" +
            "Shut down the application.";

        public override Action<string[]> HandleCommand => (commands) =>
        {
            CLI.Instance.OnStopApplication(new StopApplicationArgs());
        };
    }
}
