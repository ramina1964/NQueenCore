using System;

namespace NQueen.ConsoleApp.Commands
{
    public class HelpCommands
    {
        public const string VALID_COMMANDS = "BOARDSIZE, SOLUTIONMODE";
        public const string COMMANDEXAMPLE = "BOARDSIZE=8 SOLUTIONMODE=2";

        static void DumpAllHelp()
        {
            DumpHelpText(NQUEEN_HELP_SOLUTIONMODE);
            DumpHelpText(NQUEEN_HELP_BOARDSIZE);
        }

        public static void ProcessHelpCommand(string cmd)
        {
            cmd = cmd.Trim().ToUpper();
            string[] parts = cmd.Split(" ");
            if (parts.Length != 2)
            {
                Console.WriteLine();
                ConsoleUtils.WriteLineColored(ConsoleColor.Cyan, "AVAILABLE SUBCOMMANDS");
                DumpAllHelp();
            }

            else if (parts[1] == "SOLUTIONMODE")
                DumpHelpText(NQUEEN_HELP_SOLUTIONMODE);
            else if (parts[1] == "SOLVE")
                DumpHelpText(NQUEEN_HELP_BOARDSIZE);
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

        public const string NQUEEN_HELP_SOLUTIONMODE =
        @"  SOLUTIONMODE - Values one of the following: 0 - 'Single', 1 - 'Unique', 2 - 'All'";

        public const string NQUEEN_HELP_BOARDSIZE =
        @"  BOARDSIZE    - Whole Numbers in the Range:  [1, 37] for 'Single', [1, 17] for 'Unique', [1, 16] for 'All' Solutions";

        public const string NQUEEN_SOLUTIONMODE =
        @" Values one of the following: 0 - 'Single', 1 - 'Unique', or 2 - 'All'";

        public const string NQUEEN_BOARDSIZE =
        @" Whole Numbers in the Range:  [1, 37] for 'Single', [1, 17] for 'Unique', [1, 16] for 'All Solutions'";
    }

}