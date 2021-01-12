using System;

namespace NQueen.ConsoleApp.Commands
{
    public class HelpCommands
    {
        public const string VALID_COMMANDS = "BOARDSIZE, SOLUTIONMODE";
        public const string COMMANDEXAMPLE = "BOARDSIZE=8 SOLUTIONMODE=2";

        static void DumpAllHelp()
        {
            DumpHelpText(NQUEEN_HELP_SOLVE);
            DumpHelpText(NQUEEN_HELP_SOLUTIONMODE);
        }

        public static void ProcessHelpCommand(string cmd)
        {
            cmd = cmd.ToUpper();
            string[] parts = cmd.Split(" ");
            if (parts.Length != 2)
            {
                Console.WriteLine();
                ConsoleUtils.WriteLineColored(ConsoleColor.Cyan, "AVAILABLE SUBCOMMANDS");
                DumpAllHelp();
            }

            else if (parts[1] == "SOLVE")
                DumpHelpText(NQUEEN_HELP_SOLVE);
            else if (parts[1] == "SOLUTIONMODE")
                DumpHelpText(NQUEEN_HELP_SOLUTIONMODE);
            else
                DispatchCommands.ShowErrorExit($"Unrecognized command {parts[1]}, try " + HelpCommands.VALID_COMMANDS);
        }

        public static void DumpHelpText(string text)
        {
            var index = 0;
            foreach (string line in text.Split("\n"))
            {
                if (index++ == 0)
                    ConsoleUtils.WriteLineColored(ConsoleColor.Yellow, line);
                else
                    Console.WriteLine(line);
            }
        }

        public const string NQUEEN_HELP_SOLVE =
        @"  BOARDSIZE - Run solver on inside [1, 37] for a Single Solution, inside [1, 17] for Unique Solutions, or inside [1, 16] for All Solutions]";

        public const string NQUEEN_HELP_SOLUTIONMODE =
        @"  SOLUTIONMODE - Runs solver with SolutionMode set to Single, Unique, or All";

    }

}