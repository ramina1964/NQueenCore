﻿using NQueen.Kernel;
using NQueen.Shared.Enums;
using System;
using System.Linq;

namespace NQueen.ConsoleApp.Commands
{
    public class DispatchCommands
    {
        public static char WhiteQueen { get; set; } = '\u2655';

        public static sbyte BoardSize { get; set; }

        public static SolutionMode SolutionMode { get; set; }

        public static bool IsSolutionModeUnique => SolutionMode == SolutionMode.Unique;

        public static bool IsSolutionModeAll => SolutionMode == SolutionMode.All;

        public static sbyte UpperBoardSizeForSingleSolution => 37;

        public static sbyte UpperBoardSizeForUniqueSolutions => 17;

        public static sbyte UpperBoardSizeForAllSolutions => 16;

        public static string TooLargeSizeForAllSolutions =>
            $"BoardSize is too large for 'Unique Solutions'. Choose a number in the range [1, {UpperBoardSizeForUniqueSolutions}].";

        public static string TooLargeSizeForUniqueSolutions =>
            $"BoardSize is too large for 'Unique Solutions'. Choose a number in the range [1, {UpperBoardSizeForUniqueSolutions}].";

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

            var example = simulationResult.Result.Solutions.FirstOrDefault();
            var solutionTitle = (example == null)
                                ? "\nNo Solution Found!"
                                : "\nFirst Solution Found - Numbers in paranteses: Column No. and Row No., Starting from the Lower Left Corner:";
            ConsoleUtils.WriteLineColored(ConsoleColor.Blue, solutionTitle);
            ConsoleUtils.WriteLineColored(ConsoleColor.Yellow, example.Details);
            var board = CreateChessBoard(example.QueenList);
            ConsoleUtils.WriteLineColored(ConsoleColor.Blue, $"\nDrawing of first solution:\n");

            var message = "\tIMPORTANT - You need to set default fonts (in this console window) to SimSun-ExtB in order to show unicode characters.\n";
            ConsoleUtils.WriteLineColored(ConsoleColor.Gray, message);
            Console.WriteLine(board);
            return true;
        }

        private static bool CheckSolutionMode(string value)
        {
            var isValidInt = int.TryParse(value, out int userChoice);
            if (!isValidInt)
            {
                Console.WriteLine("Invalid integer format, try again.\n");
                return false;
            }

            switch (userChoice)
            {
                case 0:
                    SolutionMode = SolutionMode.Single;
                    return true;

                case 1:
                    SolutionMode = SolutionMode.Unique;
                    return true;

                case 2:
                    SolutionMode = SolutionMode.All;
                    return true;

                default:
                    Console.WriteLine("Invalid Option, try 0, 1, or 2.");
                    return false;
            }
        }

        private static bool CheckBoardSize(string value)
        {
            var validInt = int.TryParse(value, out int size);
            if (!validInt)
            {
                Console.WriteLine("Invalid integer format, try again.");
                return false;
            }

            BoardSize = Convert.ToSByte(size);
            if (1 > BoardSize)
            {
                Console.WriteLine("BoardSize must be a positive number.");
                return false;
            }

            if (IsSolutionModeUnique && BoardSize > UpperBoardSizeForUniqueSolutions)
            {
                Console.WriteLine(TooLargeSizeForUniqueSolutions);
                return false;
            }

            if (SolutionMode == SolutionMode.Single && BoardSize > UpperBoardSizeForSingleSolution)
            {
                Console.WriteLine(TooLargeSizeForSingleSolution);
                return false;
            }

            if (IsSolutionModeAll && BoardSize > UpperBoardSizeForUniqueSolutions)
            {
                Console.WriteLine(TooLargeSizeForAllSolutions);
                return false;
            }

            return true;
        }

        private static string[,] ChessBoardHelper(sbyte[] queens)
        {
            var size = queens.Length;
            string[,] arr = new string[size, size];

            for (int col = 0; col < size; col++)
            {
                var rowPlace = queens[col];
                for (int row = 0; row < size; row++)
                {
                    if (row == rowPlace)
                    {
                        arr[row, col] = col == size - 1 ? $"|{WhiteQueen}|" : $"|{WhiteQueen}"; ;
                    }
                    else
                    {
                        arr[row, col] = col == size - 1 ? "|-|" : "|-";
                    }
                }
            }

            return arr;
        }

        private static string CreateChessBoard(sbyte[] queens)
        {
            var arr = ChessBoardHelper(queens);
            var size = queens.Length;
            var board = string.Empty;
            for (int row = size - 1; row >= 0; row--)
            {
                for (int col = 0; col < size; col++)
                {
                    board += arr[row, col];
                }
                board += Environment.NewLine;
            }

            return board;
        }

        #endregion PrivateMethods

    }
}
