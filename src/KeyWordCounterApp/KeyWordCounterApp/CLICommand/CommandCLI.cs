namespace KeyWordCounterApp.CLICommand
{
    public abstract class CommandCLI
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract Action<string[]> HandleCommand { get; }

        protected void InvalidCommandLog(string errMessage)
        {
            CLI.Instance.ConsoleLog($"INVALID COMMAND: {errMessage}", consoleColor: ConsoleColor.Red);
            CLI.Instance.ConsoleLog(Description, consoleColor: ConsoleColor.Red);
        }
    }
}
