using KeyWordCounter.Clawler;
using KeyWordCounter.Jobs;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KeyWordCounter
{
    public class MainCLI
    {
        /*Should be singleton - FIX*/
        private static DirectoryClawler directoryClawler;
        private static JobDispatcher jobDispatcher;
        private static JobsQueue jobQueue;
        private static CancellationTokenSource crs;

        public static void Main(string[] args)
        {
            Initialize();
            ProcessCLI();
        }

        private static void Initialize()
        {
            crs = new CancellationTokenSource();
            jobQueue = new JobsQueue();
            directoryClawler = new DirectoryClawler(jobQueue);
            jobDispatcher = new JobDispatcher(jobQueue);

            /*clawler and dispatcher thread*/
            var clawlerThread = new Task(directoryClawler.Run, crs.Token);
            var dispatcherThread = new Task(jobDispatcher.Run, crs.Token);

            clawlerThread.Start();
            dispatcherThread.Start();
        }

        private static void ProcessCLI()
        {
            while (true)
            {
                Console.Write("Enter command: ");

                var command = Console.ReadLine();
                var commandSplit = command.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                if (commandSplit.Length > 0)
                {
                    switch (commandSplit[0].Trim())
                    {
                        case "ad":
                            if (commandSplit.Length == 2)
                            {
                                directoryClawler.AddDir(commandSplit[1]);
                            }
                            else
                            {
                                // invalid command
                            }
                            break;
                        case "aw":
                            break;
                        case "stop":
                            crs.Cancel();
                            break;
                        default:
                            Console.WriteLine("Unknown command.");
                            break;
                    }
                }
            }
        }
    }
}
