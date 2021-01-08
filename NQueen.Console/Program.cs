using System;
using System.Text;
using NQueen.Kernel;
using NQueen.ConsoleApp.Commands;
using System.Collections.Generic;
using System.Linq;

namespace NQueen.ConsoleApp
{
    class Program
    {
        public static Dictionary<string, bool> Commands { get; set; }
        public static Dictionary<string, string> AvailableCommands { get; set; }

        private static int NqueenSize;
        static void Main(string[] args)
        {
            InitCommands();
            OutputBanner();

            //if console app is started without args
            if (args.Length == 0)
            {
                while (!Commands.All(e => e.Value))
                {
                    var required = GetRequiredCommand();
                    if (required == "RUN")
                    {
                        ConsoleUtils.WriteLineColored(ConsoleColor.Cyan, $"\nSolver is running nqueen size {NqueenSize}:\n");
                        DispatchCommands.ProcessCommand("RUN", "ok");
                        bool runagain = true;
                        while (runagain)
                        {
                            Console.WriteLine("\nRun again to debug memory usage?");
                            var ans = Console.ReadLine().ToLower();
                            if (ans == "yes")
                            {
                                Console.WriteLine();
                                DispatchCommands.ProcessCommand("RUN", "ok");                                
                            }
                            else
                            {
                                runagain = false;
                            }

                        }
                        break;
                    }
                    ConsoleUtils.WriteLineColored(ConsoleColor.Cyan, $"Enter a {required} ");
                    Console.WriteLine($"\tAvailable commands: {AvailableCommands[required]}");
                    var answer = Console.ReadLine();
                    if (answer.ToLower() == "-h" || answer.ToLower() == "help")
                    {
                        HelpCommands.ProcessHelpCommand(answer);
                    }
                    else
                    {
                        var ok = DispatchCommands.ProcessCommand(required, answer);
                        if (ok)
                        {
                            Commands[required] = true;
                            if (required.ToUpper() == "BOARDSIZE")
                            {
                                NqueenSize = Convert.ToInt32(answer);
                            }
                        }
                    }
                }
            }
            //console app is started with custom args
            else
            {
                for (int i = 0; i < args.Length; i++)
                {
                    (string feature, string value) = ParseInput(args[i]);
                    var ok = DispatchCommands.ProcessCommand(feature, value);
                    if (ok)
                    {
                        Commands[feature.ToUpper()] = true;
                        if (feature.ToUpper() == "BOARDSIZE")
                        {
                            NqueenSize = Convert.ToInt32(value);
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
            var n = msg.Substring(option.Count() + 1);
            return (new string(option), n);
        }

        static string GetRequiredCommand()
        {
            var cmd = Commands.Where(e => !e.Value).Select(e => e.Key).FirstOrDefault();
            return cmd ?? "";
        }


        const string BannerString =
@"
|=====================================================|
| NQueen - A .NET 5.0 Console Application             |
|                                                     |
| (c) 2021 - Ramin Anvar and Lars Erik Pedersen       |
|                                                     |
| App developed for Solving N-Queen Problem           |
| using the Recursive Backtracking Algorithm          |
|                                                     |
| Version 0.50. Use help to list available commands.  |
|                                                     |
|=====================================================|
";

        static void InitCommands()
        {
            Commands = new Dictionary<string, bool>();
            Commands["BOARDSIZE"] = false;
            Commands["SOLUTIONMODE"] = false;
            Commands["RUN"] = false;
            AvailableCommands = new Dictionary<string, string>();
            AvailableCommands["BOARDSIZE"] = "Value between 4 - 17";
            AvailableCommands["SOLUTIONMODE"] = "Available commands are: All, unique, single";
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
                    Console.Write(line.Substring(1, line.Length - 2));
                    Console.ForegroundColor = defaultColor;
                    Console.WriteLine("|");
                }
                else
                {
                    Console.WriteLine(line);
                }
            }
        }
    }

}
