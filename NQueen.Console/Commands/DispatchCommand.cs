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

        public static bool ProcessCommand(string key, string value)
        {
            var returnValue = false;
            key = key.Replace("  ", " ").TrimEnd();
            if (key == string.Empty)
            { Environment.Exit(0); }

            key = key.ToUpper();
            if (key == "RUN")
            {
                var solver = new Solver(BoardSize)
                { SolutionMode = SolutionMode };

                var simulationResult = solver.GetSimulationResultsAsync(BoardSize, SolutionMode, DisplayMode.Hide);
                var simTitle = $"Summary of the Results for BoardSize = { BoardSize } and DisplayMode = { DispatchCommands.SolutionMode }";
                ConsoleUtils.WriteLineColored(ConsoleColor.Blue, $"\n{simTitle}:");

                var noOfSolutions = String.Format("{0:n0}", simulationResult.Result.NoOfSolutions);
                var elapsedTime = String.Format("{0:n1}", simulationResult.Result.ElapsedTimeInSec);
                ConsoleUtils.WriteLineColored(ConsoleColor.Gray, $"Number of solutions found: {noOfSolutions, 10}");
                ConsoleUtils.WriteLineColored(ConsoleColor.Gray, $"Elapsed time in seconds: {elapsedTime, 14}");

                var example = simulationResult.Result.Solutions.FirstOrDefault()?.Details;
                var solutionTitle = (example == null)
                                    ? "\nNo Solution Found!"
                                    : "\nFirst Solution Found - Starting from the Lower Left Corner - (column No., Row No.):";
                ConsoleUtils.WriteLineColored(ConsoleColor.Blue, solutionTitle);
                ConsoleUtils.WriteLineColored(ConsoleColor.Yellow, example);
                return true;
            }

            if (key == "BOARDSIZE")
            {
                var ok = sbyte.TryParse(value, out sbyte size);
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

            if (key == "SOLUTIONMODE")
            {
                var response = value[0].ToString().ToUpper() + value[1..];
                var ok = Enum.TryParse(response, out SolutionMode solutionMode);
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

            return returnValue;
        }

        internal static void ShowErrorExit(string errorString)
        {

            ConsoleColor priorColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("ERROR: ");
            Console.ForegroundColor = priorColor;
            Console.WriteLine(errorString);
            Console.WriteLine();
            Environment.Exit(-1);
        }
    }
}
