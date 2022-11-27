using KeyWordCounterApp.CLICommand;
using KeyWordCounterApp.Models;

namespace KeyWordCounterApp.Implementation
{
    public class CLI : IDisposable
    {
        private bool _onExit;
        private static Lazy<CLI> _instance = new Lazy<CLI>(() => new CLI());

        /* Available commands */
        private static readonly HelpCommand _helpCommand = new();
        private static readonly AddDirectoryCommand _addDirectoryCommand = new();
        private static readonly ExitCommand _exitCommand = new();

        #region Public Methods
        public static CLI Instance => _instance.Value;

        public CommandCLI ParseCommand(string command)
        {
            switch (command)
            {
                case "help":
                    return _helpCommand;
                case "ad":
                    return _addDirectoryCommand;
                case "exit":
                    return _exitCommand;
                default:
                    return null;
            }
        }

        public void ConsoleLog(string message, bool newRow = true, ConsoleColor consoleColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.ForegroundColor = consoleColor;
            Console.BackgroundColor = backgroundColor;

            if (newRow)
            {
                Console.WriteLine(message);
                return;
            }

            Console.Write(message);
        }

        public void RunApplicationCLI()
        {
            while (true)
            {
                if (!_onExit)
                {
                    ConsoleLog(">> ", false);
                    var commandLine = Console.ReadLine();

                    var commandSplit = commandLine?.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                    if (commandSplit is not null && commandSplit.Length > 0)
                    {
                        CommandCLI commandCLI = ParseCommand(commandSplit[0].Trim());

                        if (commandCLI is null)
                        {
                            ConsoleLog("Unknown command.", consoleColor: ConsoleColor.Red);
                        }

                        commandCLI?.HandleCommand.Invoke(commandSplit);
                    }
                }
            }
        }
        #endregion

        #region Dispose

        public void Dispose()
        {
            if (Instance != null)
            {
                ConsoleLog($"{nameof(CLI)} shutted down.", consoleColor: ConsoleColor.Green);
                Console.ForegroundColor = ConsoleColor.White;

                _instance = null;

                GC.SuppressFinalize(this);

                Thread.Sleep(500);
            }
        }
        #endregion

        #region Events
        public virtual void OnStopApplication(StopApplicationArgs stopArgs)
        {
            _onExit = true;

            if (StopApplication != null)
            {
                StopApplication(this, stopArgs);
            }
        }

        public event EventHandler<StopApplicationArgs> StopApplication;
        #endregion
    }
}