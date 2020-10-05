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
			switch (boardSize)
			{
				case 1:
					return 1;
				case 2:
					return 1;
				case 3:
					return 1;
				case 4:
					return 1;
				case 5:
					return 2;
				case 6:
					return 1;
				case 7:
					return 6;
				case 8:
					return 12;
				case 9:
					return 46;
				case 10:
					return 92;
				case 11:
					return 341;
				case 12:
					return 1787;
				case 13:
					return 9233;
				case 14:
					return 45752;
				case 15:
					return 285053;
				case 16:
					return 1846955;
				case 17:
					return 11977939;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public static int FindSolutionSizeAll(sbyte boardSize)
		{
			switch (boardSize)
			{
				case 1:
					return 1;
				case 2:
					return 1;
				case 3:
					return 1;
				case 4:
					return 2;
				case 5:
					return 10;
				case 6:
					return 4;
				case 7:
					return 40;
				case 8:
					return 92;
				case 9:
					return 352;
				case 10:
					return 724;
				case 11:
					return 2680;
				case 12:
					return 14200;
				case 13:
					return 73712;
				case 14:
					return 365596;
				case 15:
					return 2279184;
				case 16:
					return 14772512;
				case 17:
					return 95815104;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public static string SolutionTitle(SolutionMode solutionMode)
		{
			switch (solutionMode)
			{
				case SolutionMode.Single:
					return "No. of Solutions";

				case SolutionMode.Unique:
					return $"No. of Unique Solutions";

				case SolutionMode.All:
					return $"No. of All Solutions";

				default:
					throw new MissingFieldException("Non-Existent Enum Value!");
			}
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