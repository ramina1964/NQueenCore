using NQueen.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NQueen.Common
{
    public static class Utility
    {
        public static int MaxNoOfSolutionsInOutput = 50;

        public static IEnumerable<sbyte[]> GetSymmetricalSolutions(IReadOnlyList<sbyte> solution)
        {
            sbyte boardSize = (sbyte)solution.Count;
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
            };
        }

        public static List<sbyte[]> GetSymmetricalSolutions(List<sbyte[]> solution) =>
            solution.SelectMany(s => GetSymmetricalSolutions(s)).ToList();

        public static int FindSolutionSize(sbyte boardSize, SolutionMode solutionMode) =>
            (solutionMode == SolutionMode.Single)
                ? 1
                : (solutionMode == SolutionMode.Unique)
                ? GetSolutionSizeUnique(boardSize)
                : GetSolutionSizeAll(boardSize);

        private static int GetSolutionSizeUnique(sbyte boardSize) => boardSize switch
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
            _ => throw new ArgumentOutOfRangeException(),
        };

        private static int GetSolutionSizeAll(sbyte boardSize) => boardSize switch
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
            _ => throw new ArgumentOutOfRangeException(),
        };

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
                return (solutionMode == SolutionMode.All)
                 ? $"List of All Solution(s), Included Symmetrical Ones:"
                 : $"List of Unique Solution(s), Excluded Symmetrical Ones:";
            }

            // Here is: NoOfSolutions > MaxNoOfSolutionsInOutput
            return (solutionMode == SolutionMode.All)
                ? $"List of First {MaxNoOfSolutionsInOutput} Solution(s), May Include Symmetrical Ones:"
                : $"List of First {MaxNoOfSolutionsInOutput} Unique Solution(s), Excluded Symmetrical Ones:";
        }
    }
}