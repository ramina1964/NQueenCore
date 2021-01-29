using NQueen.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NQueen.Shared
{
    public static class Utility
    {
        public const int MaxNoOfSolutionsInOutput = 50;
        public const int DefaultDelayInMilliseconds = 250;  
        public const sbyte DefaultBoardSize = 8;
        public const sbyte RelativeFactor = 8;
        public const sbyte MinBoardSize = 1;
        public const sbyte MaxBoardSizeForSingleSolution = 37;
        public const sbyte MaxBoardSizeForUniqueSolutions = 17;
        public const sbyte MaxBoardSizeForAllSolutions = 16;

        public static string InvalidSByteError => $"Board size must be a valid integer in the interval [1, 127].";

        public static string NoSolutionMessage => $"No Solutions found. Try a larger board size!";

        public static string ValueNullOrWhiteSpaceMsg => $"Board size can not be null, empty or contain exclusively spaces.";

        public static string SizeTooSmallMsg => $"Board size must be greater than or equal to {MinBoardSize}.";

        public static string SizeTooLargeForSingleSolutionMsg => $"Board size for single solution must not exceed {MaxBoardSizeForSingleSolution}.";

        public static string SizeTooLargeForUniqueSolutionsMsg => $"Board size for unique solutions must not exceed {MaxBoardSizeForUniqueSolutions}.";

        public static string SizeTooLargeForAllSolutionsMsg => $"Board size for all solutions must not exceed {MaxBoardSizeForAllSolutions}.";

        public static List<sbyte[]> GetSymmetricalSolutions(sbyte[] solution)
        {
            sbyte boardSize = (sbyte)solution.Length;
            var symmToMidHorizontal = new sbyte[boardSize];
            var symmToMidVertical = new sbyte[boardSize];
            var symmToMainDiag = new sbyte[boardSize];
            var symmToBiDiag = new sbyte[boardSize];
            var rotCounter90 = new sbyte[boardSize];
            var rotCounter180 = new sbyte[boardSize];
            var rotCounter270 = new sbyte[boardSize];

            for (sbyte j = 0; j < boardSize; j++)
            {
                sbyte index1 = (sbyte)(boardSize - j - 1);
                sbyte index2 = (sbyte)(boardSize - solution[j] - 1);

                symmToMidHorizontal[index1] = solution[j];
                rotCounter90[index2] = symmToMainDiag[solution[j]] = j;
                rotCounter180[index1] = symmToMidVertical[j] = index2;
                rotCounter270[solution[j]] = symmToBiDiag[index2] = index1;
            }

            return new HashSet<sbyte[]>(new SequenceEquality<sbyte>())
            {
                symmToMidVertical,
                symmToMidHorizontal,
                symmToMainDiag,
                symmToBiDiag,
                rotCounter90,
                rotCounter180,
                rotCounter270,
            }.ToList();
        }

        public static List<sbyte[]> GetSymmetricalSolutions(List<sbyte[]> solution) =>
            solution.SelectMany(s => GetSymmetricalSolutions(s)).ToList();

        public static int FindSolutionSize(sbyte boardSize, SolutionMode solutionMode) =>
            solutionMode == SolutionMode.Single
                ? 1
                : solutionMode == SolutionMode.Unique
                ? GetSolutionSizeUnique(boardSize)
                : GetSolutionSizeAll(boardSize);

        public static string SolutionTitle(SolutionMode solutionMode)
        {
            return solutionMode switch
            {
                SolutionMode.Single => "No. of Solutions",
                SolutionMode.Unique => $"No. of Unique Solutions",
                SolutionMode.All => $"No. of All Solutions",
                _ => throw new MissingFieldException("Non-Existent Enum Value!"),
            };
        }

        public static string SolutionTitle(SolutionMode solutionMode, int noOfSolutions)
        {
            if (solutionMode == SolutionMode.Single)
            { return "Solution:"; }

            if (noOfSolutions <= MaxNoOfSolutionsInOutput)
            {
                return solutionMode == SolutionMode.All
                 ? $"List of All Solutions (Included Symmetrical Ones):"
                 : $"List of Unique Solutions (Excluded Symmetrical Ones):";
            }

            // Here is: NoOfSolutions > MaxNoOfSolutionsInOutput
            return solutionMode == SolutionMode.All
                ? $"List of First {MaxNoOfSolutionsInOutput} Solution(s), May Include Symmetrical Ones:"
                : $"List of First {MaxNoOfSolutionsInOutput} Unique Solution(s), Excluded Symmetrical Ones:";
        }

        #region PrivateMembers
        private static int GetSolutionSizeUnique(sbyte boardSize) =>
            boardSize switch
            {
                1 => 1,
                2 => 0,
                3 => 0,
                4 => 1,
                5 => 2,
                6 => 1,
                7 => 6,
                8 => 12,
                9 => 46,
                10 => 92,
                11 => 341,
                12 => 1787,
                13 => 9233,
                14 => 45752,
                15 => 285053,
                16 => 1846955,
                17 => 11977939,
                _ => throw new ArgumentOutOfRangeException(SizeTooLargeForUniqueSolutionsMsg)
            };

        private static int GetSolutionSizeAll(sbyte boardSize) =>
            boardSize switch
            {
                1 => 1,
                2 => 0,
                3 => 0,
                4 => 2,
                5 => 10,
                6 => 4,
                7 => 40,
                8 => 92,
                9 => 352,
                10 => 724,
                11 => 2680,
                12 => 14200,
                13 => 73712,
                14 => 365596,
                15 => 2279184,
                16 => 14772512,
                17 => 95815104,
                _ => throw new ArgumentOutOfRangeException(SizeTooLargeForAllSolutionsMsg)
            };
        #endregion PrivateMembers
    }
}