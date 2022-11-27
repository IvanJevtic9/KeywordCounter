using KeyWordCounterApp.Implementation;

namespace KeyWordCounterApp.CLICommand
{
    public abstract class CommandCLI
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract Action<string[]> HandleCommand { get; }

        protected void InvalidCommandLog(string errMessage, bool showCommandDescription = true)
        {
            CLI.Instance.ConsoleLog(errMessage, consoleColor: ConsoleColor.Red);

            if (showCommandDescription)
            {
                CLI.Instance.ConsoleLog(Description, consoleColor: ConsoleColor.Red);
            }
        }
    }
}
