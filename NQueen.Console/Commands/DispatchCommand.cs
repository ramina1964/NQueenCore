using System;
using System.Linq;
using NQueen.Kernel;
using NQueen.Shared.Enums;

namespace NQueen.ConsoleApp.Commands
{
    public class DispatchCommands
    {
        public static sbyte BoardSize { get; set; }

        public static SolutionMode SolutionMode { get; set; }

        public static bool IsSolutionModeMultiple => (SolutionMode == SolutionMode.Unique || SolutionMode == SolutionMode.All);

        public static sbyte UpperBoardSizeForMultipleSolutions => 17;

        public static sbyte UpperBoardSizeForSingleSolution => 37;

        public static string TooLargeSizeForMultipleSolutions =>
            $"BoardSize is too large for 'Unique Solutions' or 'All Solutions'. Choose a number in the range [1, {UpperBoardSizeForMultipleSolutions}].";

        public static string TooLargeSizeForSingleSolution =>
            $"BoardSize is too large for a 'Single Solutions'. Choose a number in the range [1, {UpperBoardSizeForSingleSolution}].";

        public static bool ProcessCommand(string key, string value)
        {
            var returnValue = false;
            key = key.Replace("  ", " ").TrimEnd().ToUpper();
            if (key == string.Empty)
            { Environment.Exit(0); }

            return key switch
            {
                "RUN" => RunApp(),
                "SOLUTIONMODE" => CheckSolutionMode(value),
                "BOARDSIZE" => CheckBoardSize(value),
                _ => returnValue,
            };
        }

        public static void ShowErrorExit(string errorString)
        {
            ConsoleColor priorColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("ERROR: ");
            Console.ForegroundColor = priorColor;
            Console.WriteLine(errorString);
            Console.WriteLine();
            Environment.Exit(-1);
        }

        #region PrivateMethods
        private static bool RunApp()
        {
            var solver = new Solver(BoardSize)
            { SolutionMode = SolutionMode };

            var simulationResult = solver.GetSimulationResultsAsync(BoardSize, SolutionMode, DisplayMode.Hide);
            var simTitle = $"Summary of the Results for BoardSize = { BoardSize } and DisplayMode = { DispatchCommands.SolutionMode }";
            ConsoleUtils.WriteLineColored(ConsoleColor.Blue, $"\n{simTitle}:");

            var noOfSolutions = string.Format("{0:n0}", simulationResult.Result.NoOfSolutions);
            var elapsedTime = string.Format("{0:n1}", simulationResult.Result.ElapsedTimeInSec);
            ConsoleUtils.WriteLineColored(ConsoleColor.Gray, $"Number of solutions found: {noOfSolutions,10}");
            ConsoleUtils.WriteLineColored(ConsoleColor.Gray, $"Elapsed time in seconds: {elapsedTime,14}");

            var example = simulationResult.Result.Solutions.FirstOrDefault()?.Details;
            var solutionTitle = (example == null)
                                ? "\nNo Solution Found!"
                                : "\nFirst Solution Found - Numbers in paranteses: Column No. and Row No., Starting from the Lower Left Corner:";
            ConsoleUtils.WriteLineColored(ConsoleColor.Blue, solutionTitle);
            ConsoleUtils.WriteLineColored(ConsoleColor.Yellow, example);
            return true;
        }

        private static bool CheckSolutionMode(string value)
        {
            var isValidInt = int.TryParse(value.ToString(), out int userChoice);
            if (isValidInt)
            {
                var validEnum = Enum.TryParse(typeof(SolutionMode), userChoice.ToString(), out object mode);
                if (validEnum)
                {
                    SolutionMode = (SolutionMode)mode;
                    return true;
                }
                return false;
            }

            else
            {
                Console.WriteLine("Could not parse solutionmode");
                return false;
            }
        }

        private static bool CheckBoardSize(string value)
        {
            var ok = sbyte.TryParse(value, out sbyte size);
            if (!ok)
            {
                Console.WriteLine("Could not parse displaymode");
                return false;
            }

            BoardSize = Convert.ToSByte(size);
            if (1 > BoardSize)
            {
                Console.WriteLine("BoardSize must be a positive number.");
                return false;
            }

            if (IsSolutionModeMultiple && UpperBoardSizeForMultipleSolutions < BoardSize)
            {
                Console.WriteLine(TooLargeSizeForMultipleSolutions);
                return false;
            }

            if (SolutionMode == SolutionMode.Single && UpperBoardSizeForSingleSolution < BoardSize)
            {
                Console.WriteLine(TooLargeSizeForSingleSolution);
                return false;
            }
            return true;
        }
        #endregion PrivateMethods

    }
}
