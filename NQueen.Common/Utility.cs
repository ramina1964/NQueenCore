using NQueen.Common.Enum;
using System;
using System.Collections.Generic;

namespace NQueen.Common
{
	public static class Utility
	{
		public static int MaxNoOfSolutionsInOutput = 50;

		public static IEnumerable<sbyte[]> GetSymmSols(IReadOnlyList<sbyte> solution)
		{
			var boardSize = solution.Count;
			var midLineHorizontal = new sbyte[boardSize];
			var midLineVertical = new sbyte[boardSize];
			var diagonalToUpperRight = new sbyte[boardSize];
			var diagonalToUpperLeft = new sbyte[boardSize];
			var counter90 = new sbyte[boardSize];
			var counter180 = new sbyte[boardSize];
			var counter270 = new sbyte[boardSize];

			for (sbyte j = 0; j < boardSize; j++)
			{
				var index1 = (sbyte)(boardSize - j - 1);
				var index2 = (sbyte)(boardSize - solution[j] - 1);

				midLineHorizontal[index1] = solution[j];
				counter180[index1] = midLineVertical[j] = index2;
				counter270[solution[j]] = diagonalToUpperRight[index2] = index1;
				counter90[index2] = diagonalToUpperLeft[solution[j]] = j;
			}

			return new HashSet<sbyte[]>
			{
				midLineVertical,
				diagonalToUpperRight,
				diagonalToUpperLeft,
				counter90,
				counter180,
				counter270,
				midLineHorizontal
			};
		}

		public static int FindSolutionSize(sbyte boardSize, SolutionMode solutionMode)
		{
			return (solutionMode == SolutionMode.Single)
				? 1
				: (solutionMode == SolutionMode.Unique)
				? FindSolutionSizeUnique(boardSize)
				: FindSolutionSizeAll(boardSize);
		}

		public static int FindSolutionSizeUnique(sbyte boardSize)
		{
            return boardSize switch
            {
                1 => 1,
                2 => 1,
                3 => 1,
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
        }

		public static int FindSolutionSizeAll(sbyte boardSize)
		{
            return boardSize switch
            {
                1 => 1,
                2 => 1,
                3 => 1,
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
        }

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