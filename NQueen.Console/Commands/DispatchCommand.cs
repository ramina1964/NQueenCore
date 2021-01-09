using System;
using System.Linq;
using NQueen.Kernel;
using NQueen.Shared.Enums;

namespace NQueen.ConsoleApp.Commands
{
    internal class DispatchCommands
    {
        public static sbyte BoardSize { get; set; }

        public static SolutionMode SolutionMode { get; set; }

        public DisplayMode DisplayMode = DisplayMode.Hide;

        public static bool ProcessCommand(string feature, string value)
        {
            bool ret = false;
            feature = feature.Replace("  ", " ").TrimEnd();
            if (feature == "")
            {
                System.Environment.Exit(0);
            }

            feature = feature.ToUpper();
            if (feature == "RUN")
            {
                var solver = new Solver(BoardSize)
                {
                    SolutionMode = SolutionMode
                };

                var simulationResult = solver.GetSimulationResultsAsync(BoardSize, SolutionMode, DisplayMode.Hide);
                ConsoleUtils.WriteLineColored(ConsoleColor.Blue, $"Here is a summary from NQueen solver with boardsize {simulationResult.Result.BoardSize}:\n");
                ConsoleUtils.WriteLineColored(ConsoleColor.Gray, $"Number of solutions found: {simulationResult.Result.NoOfSolutions}");
                ConsoleUtils.WriteLineColored(ConsoleColor.Gray, $"Elapsed time in seconds: {simulationResult.Result.ElapsedTimeInSec}");
                
                var example = simulationResult.Result.Solutions.First().Details;
                ConsoleUtils.WriteLineColored(ConsoleColor.Gray, "And here, is the queen Locations from the Lower Left Corner, (column No., Row No.):");
                ConsoleUtils.WriteLineColored(ConsoleColor.Blue, "\nExample output - first solution found:\n");
                ConsoleUtils.WriteLineColored(ConsoleColor.Yellow, example);
                return true;
            }

            if (feature == "BOARDSIZE")
            {
                var ok = int.TryParse(value, out int size);
                if (ok)
                {
                    BoardSize = Convert.ToSByte(size);
                    return true;
                }
                else
                {
                    Console.WriteLine("Could not parse displaymode");
                    return false;
                }
            }

            if (feature == "SOLUTIONMODE")
            {
                var svar = value[0].ToString().ToUpper() + value[1..];
                var ok = Enum.TryParse(svar, out SolutionMode solutionMode);
                if (ok)
                {
                    SolutionMode = solutionMode;
                    return true;
                }

                else
                {
                    Console.WriteLine("Could not parse solutionmode");
                }
            }

            return ret;
        }

        internal static void ShowErrorExit(string errorString)
        {

            ConsoleColor priorColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("ERROR: ");
            Console.ForegroundColor = priorColor;
            Console.WriteLine(errorString);
            Console.WriteLine();
            System.Environment.Exit(-1);

        }
    }
}
