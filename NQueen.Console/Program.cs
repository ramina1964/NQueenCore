using System;
using NQueen.ConsoleApp.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace NQueen.ConsoleApp
{
    class Program
    {
        public static Dictionary<string, bool> Commands { get; set; }

        public static Dictionary<string, string> AvailableCommands { get; set; }

        // In order to enable dotnet-counters you need to install dotnet-counters tool with the following command (use cmd)
        // dotnet tool install --global dotnet-counters
        // link: https://docs.microsoft.com/en-us/dotnet/core/diagnostics/dotnet-counters#:~:text=dotnet-counters%20is%20a%20performance%20monitoring%20tool%20for%20ad-hoc,values%20that%20are%20published%20via%20the%20EventCounter%20API.

        static void Main(string[] args)
        {
            // You need to change to font to SimSun-ExtB in order to show unicode characters in console - IMPORTANT
            Console.OutputEncoding = System.Text.Encoding.UTF8;            
            InitCommands();
            OutputBanner();
            LaunchConsoleMonitor();

            // If console app is started without args:
            if (args.Length == 0)
            {
                while (!Commands.All(e => e.Value))
                {
                    var required = GetRequiredCommand();
                    if (required == "RUN")
                    {
                        ConsoleUtils.WriteLineColored(ConsoleColor.Cyan, $"\nSolver is running ...");
                        DispatchCommands.ProcessCommand("RUN", "ok");
                        var runAgain = true;
                        while (runAgain)
                        {
                            Console.WriteLine("\nRun again to debug memory usage?");
                            Console.WriteLine("\tYes or No\n");
                            var ans = Console.ReadLine().Trim().ToLower();
                            if (ans == "yes" || ans == "y")
                            {
                                Console.WriteLine();
                                DispatchCommands.ProcessCommand("RUN", "ok");
                            }
                            else
                            { runAgain = false; }
                        }
                        break;
                    }

                    ConsoleUtils.WriteLineColored(ConsoleColor.Cyan, $"Enter a {required} ");
                    Console.WriteLine($"\tAvailable commands: {AvailableCommands[required]}");
                    var answer = Console.ReadLine().Trim().ToLower();
                    if (answer == "help" || answer == "-h")
                    { HelpCommands.ProcessHelpCommand(answer); }
                    else
                    {
                        var ok = DispatchCommands.ProcessCommand(required, answer);
                        if (ok)
                        {
                            Commands[required] = true;
                            if (required.Trim().ToUpper() == "BOARDSIZE")
                            { BoardSize = Convert.ToSByte(answer); }
                        }
                    }
                }
            }

            // Console app is started with custom args:
            else
            {
                for (var i = 0; i < args.Length; i++)
                {
                    (string feature, string value) = ParseInput(args[i]);
                    var ok = DispatchCommands.ProcessCommand(feature, value);
                    if (ok)
                    {
                        Commands[feature.ToUpper()] = true;
                        if (feature.ToUpper() == "BOARDSIZE")
                        {
                            BoardSize = Convert.ToSByte(value);
                        }
                    }
                }

                if (GetRequiredCommand() == "RUN")
                {
                    ConsoleUtils.WriteLineColored(ConsoleColor.Cyan, "Solver is running:\n");
                    DispatchCommands.ProcessCommand("RUN", "ok");
                }
            }
        }

        static (string feature, string value) ParseInput(string msg)
        {
            var option = msg.ToCharArray().TakeWhile(e => e != '=').ToArray();
            var n = msg[(option.Length + 1)..];
            return (new string(option), n);
        }

        static string GetRequiredCommand()
        {
            var cmd = Commands.Where(e => !e.Value).Select(e => e.Key).FirstOrDefault();
            return cmd ?? "";
        }


        const string BannerString =
@"
|====================================================|
| NQueen.ConsoleApp - A .NET 5.0 Console Application |
|                                                    |
| (c) 2021 - Ramin Anvar and Lars Erik Pedersen      |
|                                                    |
| App Developed for Solving N-Queen Problem          |
| Using the Recursive Backtracking Algorithm         |
|                                                    |
| Version 0.50. Use help to list available commands. |
|                                                    |
|====================================================|
";
        static void InitCommands()
        {
            Commands = new Dictionary<string, bool>
            {
                ["SOLUTIONMODE"] = false,
                ["BOARDSIZE"] = false,
                ["RUN"] = false
            };
            AvailableCommands = new Dictionary<string, string>
            {
                ["SOLUTIONMODE"] = "0 - Single Solution, 1 - Unique Solutions, 2 - All Solutions",
                ["BOARDSIZE"] = "Value in the Range: [1, 37] for Single Solution, [1, 17] for Unique Solution, [1, 16] for All Solutions",
            };
        }

        static void OutputBanner()
        {
            string[] bannerLines = BannerString.Split("\r\n");
            foreach (string line in bannerLines)
            {
                if (line.StartsWith("| NQueen"))
                {
                    ConsoleColor defaultColor = Console.ForegroundColor;
                    Console.Write("|");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write(line[1..^1]);
                    Console.ForegroundColor = defaultColor;
                    Console.WriteLine("|");
                }
                else
                {
                    Console.WriteLine(line);
                }
            }
        }

        static void LaunchConsoleMonitor(string extraSourceNames = "")
        {
            if (DOTNETCOUNTERSENABLED)
            {
                int processID = Environment.ProcessId;
                ProcessStartInfo ps = new ProcessStartInfo()
                {
                    FileName = "dotnet-counters",
                    Arguments = $"monitor --process-id {processID} NQueen.ConsoleApp System.Runtime " + extraSourceNames,
                    UseShellExecute = true
                };
                Process.Start(ps);
            }
        }

        // This is used for enabling dotnet-counters performance utility when you run the application
        private static readonly bool DOTNETCOUNTERSENABLED = false;

        private static sbyte BoardSize { get; set; }
    }
}
