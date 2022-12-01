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
        private static readonly GetResultCommand _getResultCommand = new();
        private static readonly ListFileResultCommand _listFileResultCommand = new();

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
                case "get":
                    return _getResultCommand;
                case "file_ls":
                    return _listFileResultCommand;
                default:
                    return null;
            }
        }

        public void ConsoleLog(string message, string location = nameof(CLI), bool newRow = true, ConsoleColor consoleColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.ForegroundColor = consoleColor;
            Console.BackgroundColor = backgroundColor;

            if (newRow)
            {
                LogToFile(message, location);
            }

            if (newRow)
            {
                Console.WriteLine(message);
                return;
            }

            Console.Write(message);
        }

        public void LogToFile(string message, string location)
        {
            File.AppendAllText(Constants.LogFile, $"[{DateTime.Now}][Thread-{Thread.CurrentThread.ManagedThreadId}][{location}] {message}\n");
        }

        public void RunApplicationCLI()
        {
            while (true)
            {
                if (_onExit) break;

                ConsoleLog(">> ", newRow: false);
                var commandLine = Console.ReadLine();
                LogToFile(commandLine, nameof(CLI));

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