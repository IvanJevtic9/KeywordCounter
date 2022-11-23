using KeyWordCounterApp.Models;
using System.Collections.Concurrent;

namespace KeyWordCounterApp
{
    public class CLI
    {
        private static bool _inputLock;

        /* Available commands */
        private static readonly HelpCommand _helpCommand = new();
        private static readonly AddDirectoryCommand _addDirectoryCommand = new();

        private CLI()
        { }

        private static Lazy<CLI> _instance = new Lazy<CLI>(() => new CLI());
        public static CLI Instance => _instance.Value;

        public static void SetInputLock(bool value)
        {
            _inputLock = value;
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
                if (!_inputLock)
                {
                    ConsoleLog(">> ", false);
                    var commandLine = Console.ReadLine();

                    var commandSplit = commandLine?.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                    if (commandSplit is not null && commandSplit.Length > 0)
                    {
                        CommandCLI? commandCLI = ParseCommand(commandSplit[0].Trim());
                        
                        if(commandCLI is null)
                        {
                            ConsoleLog("Unknown command.", consoleColor: ConsoleColor.Red);
                        }

                        commandCLI?.HandleCommand.Invoke(commandSplit);
                    }
                }
            }
        }

        public CommandCLI? ParseCommand(string command)
        {
            switch (command)
            {
                case "help":
                    return _helpCommand;
                case "ad":
                    return _addDirectoryCommand;
                default:
                    return null;
            }
        }
    }
}


/*
   Slucajevi koriscenja
    1. Cekamo na unos komande && Ne ispisuje se output (lock za ispis)
    2. Ispisuje se output komande (lock za upis)
 */