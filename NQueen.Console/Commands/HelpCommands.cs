using System;

namespace NQueen.ConsoleApp.Commands
{
    internal class HelpCommands
    {
        internal const string VALID_COMMANDS = "BOARDSIZE, SOLUTIONMODE";
        internal const string COMMANDEXAMPLE = "BOARDSIZE=8 SOLUTIONMODE=ALL";
        static void DumpAllHelp()
        {
            DumpHelpText(NQUEEN_HELP_SOLVE);
            DumpHelpText(NQUEEN_HELP_SOLUTIONMODE);
            //var example = $"Example command: {COMMANDEXAMPLE}";
            //ConsoleUtils.WriteLineColored(ConsoleColor.Cyan, example);
        }

        internal static void ProcessHelpCommand(string cmd)
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
            //System.Environment.Exit(0);
        }

        static void DumpHelpText(string text)
        {
            int index = 0;
            foreach (string line in text.Split("\n"))
            {
                if (index++ == 0)
                    ConsoleUtils.WriteLineColored(ConsoleColor.Yellow, line);
                else
                    Console.WriteLine(line);
            }
        }

        const string NQUEEN_HELP_SOLVE =
        @"  BOARDSIZE - Run solver on a specific boardsize between 4 - 17";    

        const string NQUEEN_HELP_SOLUTIONMODE =
        @"  SOLUTIONMODE - Runs solver with SolutionMode set to all, unique or single";

    }

}