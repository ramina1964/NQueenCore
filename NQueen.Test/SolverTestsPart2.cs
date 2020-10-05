using NQueen.Common;
using System;
using System.Collections.Generic;

namespace NQueen.Test
{
	public partial class NQueenSolverTests
	{
		#region PrivateFields
		private static Dictionary<sbyte, int> noOfUniqueSols = new Dictionary<sbyte, int>
		{
			{ 6, 1 },
			{ 7, 6 },
			{ 8, 12 },
			{ 9, 46 },
			{ 10, 92 },
			{ 11, 341 },
			{ 12, 1787 },
			{ 13, 9233 },
			{ 14, 45752 }
		};

		private static Dictionary<sbyte, int> noOfAllSols = new Dictionary<sbyte, int>
		{
			{ 6, 4 },
			{ 7, 40 },
			{ 8, 92 },
			{ 9, 352 },
			{ 10, 724 },
			{ 11, 2680 },
			{ 12, 14200 },
			{ 13, 73712 },
			{ 14, 365596 }
		};

		private static Dictionary<sbyte, List<Solution>> singleSol = new Dictionary<sbyte, List<Solution>>
		{
			{ 1, new List<Solution> { new Solution(new sbyte[] { 0 }, 1) } },
			{ 2, new List<Solution>() { } },
			{ 3, new List<Solution>() { } },
			{ 4, new List<Solution> { new Solution(new sbyte[] { 1, 3, 0, 2 }, 4) } },
			{ 5, new List<Solution> { new Solution(new sbyte[] { 0, 2, 4, 1, 3 }, 5) } },
			{ 6, new List<Solution> { new Solution(new sbyte[] { 1, 3, 5, 0, 2, 4 }, 6) } },
			{ 7, new List<Solution> { new Solution(new sbyte[] { 0, 2, 4, 6, 1, 3, 5 }, 7) } },
			{ 8, new List<Solution> { new Solution(new sbyte[] { 0, 4, 7, 5, 2, 6, 1, 3 }, 8) } },
			{ 9, new List<Solution> { new Solution(new sbyte[] { 0, 2, 5, 7, 1, 3, 8, 6, 4 }, 9) } },

			{ 18, new List<Solution> {new Solution(new sbyte[] {0, 2, 4, 1, 7, 14, 11, 15, 12, 16, 5, 17, 6, 3, 10, 8, 13,
				9 }, 18) } },

			{ 19, new List<Solution> {new Solution(new sbyte[] {0, 2, 4, 1, 3, 8, 12, 14, 16, 18, 6, 15, 17, 10, 5, 7, 9,
				11, 13 }, 19) } },

			{ 20, new List<Solution> {new Solution(new sbyte[] {0, 2, 4, 1, 3, 12, 14, 11, 17, 19, 16, 8, 15, 18, 7, 9, 6,
				13, 5, 10}, 20) } },

			{ 21, new List<Solution> {new Solution(new sbyte[] {0, 2, 4, 1, 3, 8, 10, 14, 20, 17, 19, 16, 18, 6, 11, 9, 7, 5,
				13, 15, 12}, 21) }  },

			{ 22, new List<Solution> {new Solution(new sbyte[] {0, 2, 4, 1, 3, 9, 13, 16, 19, 12, 18, 21, 17, 7, 20, 11, 8, 5,
					15, 6, 10, 14 }, 22) } },

			{ 23, new List<Solution> {new Solution(new sbyte[] {0, 2, 4, 1, 3, 8, 10, 12, 17, 19, 21, 18, 20, 9, 7, 5, 22, 6,
					15, 11, 14, 16, 13 }, 23) } },

			{ 24, new List<Solution> {new Solution(new sbyte[] {0, 2, 4, 1, 3, 8, 10, 13, 17, 21, 18, 22, 19, 23, 9, 20, 5, 7,
					11, 15, 12, 6, 16, 14}, 24) } },

			{ 25, new List<Solution> {new Solution(new sbyte[] {0, 2, 4, 1, 3, 8, 10, 12, 14, 18, 20, 23, 19, 24, 22, 5, 7, 9,
					6, 13, 15, 17, 11, 16, 21}, 25) } },

			{ 26, new List<Solution> {new Solution(new sbyte[] {0, 2, 4, 1, 3, 8, 10, 12, 14, 20, 22, 24, 19, 21, 23, 25, 9, 6,
					15, 11, 7, 5, 17, 13, 18, 16}, 26) } },

			{ 27, new List<Solution> {new Solution(new sbyte[]  {0, 2, 4, 1, 3, 8, 10, 12, 14, 16, 18, 22, 24, 26, 23, 25, 5, 9,
					6, 15, 7, 11, 13, 20, 17, 19, 21}, 27) } },

			{ 28, new List<Solution> {new Solution(new sbyte[] {0, 2, 4, 1, 3, 8, 10, 12, 14, 16, 22, 24, 21, 27, 25, 23, 26, 6,
					11, 15, 17, 7, 9, 13, 19, 5, 20, 18}, 28) } },
		};

		private static Dictionary<sbyte, List<Solution>> uniqueSol = new Dictionary<sbyte, List<Solution>>
		{
			{1, new List<Solution> { new Solution(new sbyte[] { 0 }, 1) } },
			{2, new List<Solution>() },
			{3, new List<Solution>() },
			{4, new List<Solution> { new Solution(new sbyte[] { 1, 3, 0, 2 }, 1) } },
			{5, new List<Solution>
					{
						new Solution(new sbyte[] {0, 2, 4, 1, 3}, 1),
						new Solution(new sbyte[] {3, 0, 2, 4, 1}, 2)
					}
			},

			{6, new List<Solution> { new Solution(new sbyte[] { 1, 3, 5, 0, 2, 4 }, 1) } },
			{7, new List<Solution>
					{
						new Solution( new sbyte[ ] {0, 2, 4, 6, 1, 3, 5}, 1),
						new Solution( new sbyte[ ] {0, 3, 6, 2, 5, 1, 4}, 2),
						new Solution( new sbyte[ ] {1, 3, 0, 6, 4, 2, 5}, 3),
						new Solution( new sbyte[ ] {1, 4, 0, 3, 6, 2, 5}, 4),
						new Solution( new sbyte[ ] {2, 6, 3, 0, 4, 1, 5}, 5),
						new Solution( new sbyte[ ] {4, 0, 5, 3, 1, 6, 2}, 6)
					}
			},

			{8, new List<Solution>
					{
						new Solution( new sbyte[ ]  {0, 4, 7, 5, 2, 6, 1, 3}, 1),
						new Solution( new sbyte[ ]  {0, 5, 7, 2, 6, 3, 1, 4}, 2),
						new Solution( new sbyte[ ]  {1, 4, 6, 0, 2, 7, 5, 3}, 3),
						new Solution( new sbyte[ ]  {1, 5, 0, 6, 3, 7, 2, 4}, 4),
						new Solution( new sbyte[ ]  {1, 7, 5, 0, 2, 4, 6, 3}, 5),
						new Solution( new sbyte[ ]  {2, 5, 3, 0, 7, 4, 6, 1}, 6),
						new Solution( new sbyte[ ]  {2, 5, 7, 0, 3, 6, 4, 1}, 7),
						new Solution( new sbyte[ ]  {2, 5, 7, 0, 4, 6, 1, 3}, 8),
						new Solution( new sbyte[ ]  {4, 0, 3, 5, 7, 1, 6, 2}, 9),
						new Solution( new sbyte[ ]  {4, 1, 5, 0, 6, 3, 7, 2}, 10),
						new Solution( new sbyte[ ]  {4, 2, 0, 6, 1, 7, 5, 3}, 11),
						new Solution( new sbyte[ ]  {4, 6, 0, 3, 1, 7, 5, 2}, 12)
					}
			},

			{9, new List<Solution>
					{
						new Solution( new sbyte[ ]  {0, 2, 5, 7, 1, 3, 8, 6, 4}, 1),
						new Solution( new sbyte[ ]  {0, 2, 6, 1, 7, 4, 8, 3, 5}, 2),
						new Solution( new sbyte[ ]  {0, 2, 7, 5, 8, 1, 4, 6, 3}, 3),
						new Solution( new sbyte[ ]  {0, 3, 5, 2, 8, 1, 7, 4, 6}, 4),
						new Solution( new sbyte[ ]  {0, 3, 5, 7, 1, 4, 2, 8, 6}, 5),
						new Solution( new sbyte[ ]  {0, 3, 6, 2, 7, 1, 4, 8, 5}, 6),
						new Solution( new sbyte[ ]  {0, 3, 6, 8, 1, 4, 7, 5, 2}, 7),
						new Solution( new sbyte[ ]  {0, 3, 7, 2, 8, 6, 4, 1, 5}, 8),
						new Solution( new sbyte[ ]  {0, 4, 6, 8, 2, 7, 1, 3, 5}, 9),
						new Solution( new sbyte[ ]  {0, 4, 6, 8, 3, 1, 7, 5, 2}, 10),
						new Solution( new sbyte[ ]  {0, 4, 8, 5, 3, 1, 7, 2, 6}, 11),
						new Solution( new sbyte[ ]  {0, 5, 7, 2, 6, 3, 1, 8, 4}, 12),
						new Solution( new sbyte[ ]  {0, 6, 3, 7, 2, 4, 8, 1, 5}, 13),
						new Solution( new sbyte[ ]  {0, 6, 3, 7, 2, 8, 5, 1, 4}, 14),
						new Solution( new sbyte[ ]  {1, 3, 0, 6, 8, 5, 2, 4, 7}, 15),
						new Solution( new sbyte[ ]  {1, 3, 6, 0, 2, 8, 5, 7, 4}, 16),
						new Solution( new sbyte[ ]  {1, 4, 7, 0, 2, 5, 8, 6, 3}, 17),
						new Solution( new sbyte[ ]  {1, 4, 7, 0, 8, 5, 2, 6, 3}, 18),
						new Solution( new sbyte[ ]  {1, 4, 8, 3, 0, 7, 5, 2, 6}, 19),
						new Solution( new sbyte[ ]  {1, 5, 0, 2, 6, 8, 3, 7, 4}, 20),
						new Solution( new sbyte[ ]  {1, 5, 0, 6, 4, 2, 8, 3, 7}, 21),
						new Solution( new sbyte[ ]  {1, 5, 0, 8, 4, 7, 3, 6, 2}, 22),
						new Solution( new sbyte[ ]  {1, 5, 2, 0, 7, 3, 8, 6, 4}, 23),
						new Solution( new sbyte[ ]  {1, 6, 4, 0, 8, 3, 5, 7, 2}, 24),
						new Solution( new sbyte[ ]  {1, 7, 0, 3, 6, 8, 5, 2, 4}, 25),
						new Solution( new sbyte[ ]  {2, 4, 8, 3, 0, 6, 1, 5, 7}, 26),
						new Solution( new sbyte[ ]  {2, 5, 7, 0, 4, 8, 1, 3, 6}, 27),
						new Solution( new sbyte[ ]  {2, 5, 7, 4, 0, 8, 6, 1, 3}, 28),
						new Solution( new sbyte[ ]  {2, 5, 8, 0, 7, 3, 1, 6, 4}, 29),
						new Solution( new sbyte[ ]  {2, 5, 8, 6, 0, 3, 1, 4, 7}, 30),
						new Solution( new sbyte[ ]  {2, 6, 8, 0, 4, 1, 7, 5, 3}, 31),
						new Solution( new sbyte[ ]  {2, 7, 5, 0, 8, 1, 4, 6, 3}, 32),
						new Solution( new sbyte[ ]  {2, 8, 3, 0, 7, 5, 1, 6, 4}, 33),
						new Solution( new sbyte[ ]  {2, 8, 5, 3, 0, 6, 4, 1, 7}, 34),
						new Solution( new sbyte[ ]  {3, 1, 6, 8, 0, 7, 4, 2, 5}, 35),
						new Solution( new sbyte[ ]  {3, 1, 8, 4, 0, 7, 5, 2, 6}, 36),
						new Solution( new sbyte[ ]  {3, 5, 8, 2, 0, 7, 1, 4, 6}, 37),
						new Solution( new sbyte[ ]  {3, 8, 4, 2, 0, 5, 7, 1, 6}, 38),
						new Solution( new sbyte[ ]  {3, 8, 4, 2, 0, 6, 1, 7, 5}, 39),
						new Solution( new sbyte[ ]  {3, 8, 4, 7, 0, 2, 5, 1, 6}, 40),
						new Solution( new sbyte[ ]  {5, 0, 6, 3, 7, 2, 4, 8, 1}, 41),
						new Solution( new sbyte[ ]  {5, 2, 0, 7, 3, 8, 6, 4, 1}, 42),
						new Solution( new sbyte[ ]  {5, 2, 0, 7, 4, 1, 8, 6, 3}, 43),
						new Solution( new sbyte[ ]  {5, 7, 0, 6, 3, 1, 8, 4, 2}, 44),
						new Solution( new sbyte[ ]  {6, 0, 3, 7, 4, 2, 8, 5, 1}, 45),
						new Solution( new sbyte[ ]  {6, 0, 5, 1, 4, 7, 3, 8, 2}, 46)
					}
			},
		};

		private static Dictionary<sbyte, List<Solution>> allSol = new Dictionary<sbyte, List<Solution>>
		{
			{ 1, new List<Solution> { new Solution(new sbyte[] { 0 }, 1) } },
			{ 2, new List<Solution>() { } },
			{ 3, new List<Solution>() { } },
			{ 4, new List<Solution>()
				{
					new Solution(new sbyte[] { 1, 3, 0, 2 }, 1),
					new Solution(new sbyte[] { 2, 0, 3, 1 }, 2)
				}
			},

			{ 5, new List<Solution>()
				{
					new Solution(new sbyte[] {0, 2, 4, 1, 3}, 1),
					new Solution(new sbyte[] {0, 3, 1, 4, 2}, 2),
					new Solution(new sbyte[] {1, 3, 0, 2, 4}, 3),
					new Solution(new sbyte[] {1, 4, 2, 0, 3}, 4),
					new Solution(new sbyte[] {2, 0, 3, 1, 4}, 5),
					new Solution(new sbyte[] {2, 4, 1, 3, 0}, 6),
					new Solution(new sbyte[] {3, 0, 2, 4, 1}, 7),
					new Solution(new sbyte[] {3, 1, 4, 2, 0}, 8),
					new Solution(new sbyte[] {4, 1, 3, 0, 2}, 9),
					new Solution(new sbyte[] {4, 2, 0, 3, 1}, 10)
				}
			},

			{ 6, new List<Solution>()
				{
					new Solution(new sbyte[] {1, 3, 5, 0, 2, 4}, 1),
					new Solution(new sbyte[] {2, 5, 1, 4, 0, 3}, 2),
					new Solution(new sbyte[] {3, 0, 4, 1, 5, 2}, 3),
					new Solution(new sbyte[] {4, 2, 0, 5, 3, 1}, 4)
				}
			},

			{ 7, new List<Solution>()
					{
						new Solution(new sbyte[] {0, 2, 4, 6, 1, 3, 5}, 1),
						new Solution(new sbyte[] {0, 3, 6, 2, 5, 1, 4}, 2),
						new Solution(new sbyte[] {0, 4, 1, 5, 2, 6, 3}, 3),
						new Solution(new sbyte[] {0, 5, 3, 1, 6, 4, 2}, 4),
						new Solution(new sbyte[] {1, 3, 0, 6, 4, 2, 5}, 5),
						new Solution(new sbyte[] {1, 3, 5, 0, 2, 4, 6}, 6),
						new Solution(new sbyte[] {1, 4, 0, 3, 6, 2, 5}, 7),
						new Solution(new sbyte[] {1, 4, 2, 0, 6, 3, 5}, 8),
						new Solution(new sbyte[] {1, 4, 6, 3, 0, 2, 5}, 9),
						new Solution(new sbyte[] {1, 5, 2, 6, 3, 0, 4}, 10),
						new Solution(new sbyte[] {1, 6, 4, 2, 0, 5, 3}, 11),
						new Solution(new sbyte[] {2, 0, 5, 1, 4, 6, 3}, 12),
						new Solution(new sbyte[] {2, 0, 5, 3, 1, 6, 4}, 13),
						new Solution(new sbyte[] {2, 4, 6, 1, 3, 5, 0}, 14),
						new Solution(new sbyte[] {2, 5, 1, 4, 0, 3, 6}, 15),
						new Solution(new sbyte[] {2, 6, 1, 3, 5, 0, 4}, 16),
						new Solution(new sbyte[] {2, 6, 3, 0, 4, 1, 5}, 17),
						new Solution(new sbyte[] {3, 0, 2, 5, 1, 6, 4}, 18),
						new Solution(new sbyte[] {3, 0, 4, 1, 5, 2, 6}, 19),
						new Solution(new sbyte[] {3, 1, 6, 4, 2, 0, 5}, 20),
						new Solution(new sbyte[] {3, 5, 0, 2, 4, 6, 1}, 21),
						new Solution(new sbyte[] {3, 6, 2, 5, 1, 4, 0}, 22),
						new Solution(new sbyte[] {3, 6, 4, 1, 5, 0, 2}, 23),
						new Solution(new sbyte[] {4, 0, 3, 6, 2, 5, 1}, 24),
						new Solution(new sbyte[] {4, 0, 5, 3, 1, 6, 2}, 25),
						new Solution(new sbyte[] {4, 1, 5, 2, 6, 3, 0}, 26),
						new Solution(new sbyte[] {4, 2, 0, 5, 3, 1, 6}, 27),
						new Solution(new sbyte[] {4, 6, 1, 3, 5, 0, 2}, 28),
						new Solution(new sbyte[] {4, 6, 1, 5, 2, 0, 3}, 29),
						new Solution(new sbyte[] {5, 0, 2, 4, 6, 1, 3}, 30),
						new Solution(new sbyte[] {5, 1, 4, 0, 3, 6, 2}, 31),
						new Solution(new sbyte[] {5, 2, 0, 3, 6, 4, 1}, 32),
						new Solution(new sbyte[] {5, 2, 4, 6, 0, 3, 1}, 33),
						new Solution(new sbyte[] {5, 2, 6, 3, 0, 4, 1}, 34),
						new Solution(new sbyte[] {5, 3, 1, 6, 4, 2, 0}, 35),
						new Solution(new sbyte[] {5, 3, 6, 0, 2, 4, 1}, 36),
						new Solution(new sbyte[] {6, 1, 3, 5, 0, 2, 4}, 37),
						new Solution(new sbyte[] {6, 2, 5, 1, 4, 0, 3}, 38),
						new Solution(new sbyte[] {6, 3, 0, 4, 1, 5, 2}, 39),
						new Solution(new sbyte[] {6, 4, 2, 0, 5, 3, 1}, 40)
					}
			},

			{ 8, new List<Solution>()
				{
					new Solution(new sbyte[] {0, 4, 7, 5, 2, 6, 1, 3}, 1),
					new Solution(new sbyte[] {0, 5, 7, 2, 6, 3, 1, 4}, 2),
					new Solution(new sbyte[] {0, 6, 3, 5, 7, 1, 4, 2}, 3),
					new Solution(new sbyte[] {0, 6, 4, 7, 1, 3, 5, 2}, 4),
					new Solution(new sbyte[] {1, 3, 5, 7, 2, 0, 6, 4}, 5),
					new Solution(new sbyte[] {1, 4, 6, 0, 2, 7, 5, 3}, 6),
					new Solution(new sbyte[] {1, 4, 6, 3, 0, 7, 5, 2}, 7),
					new Solution(new sbyte[] {1, 5, 0, 6, 3, 7, 2, 4}, 8),
					new Solution(new sbyte[] {1, 5, 7, 2, 0, 3, 6, 4}, 9),
					new Solution(new sbyte[] {1, 6, 2, 5, 7, 4, 0, 3}, 10),
					new Solution(new sbyte[] {1, 6, 4, 7, 0, 3, 5, 2}, 11),
					new Solution(new sbyte[] {1, 7, 5, 0, 2, 4, 6, 3}, 12),
					new Solution(new sbyte[] {2, 0, 6, 4, 7, 1, 3, 5}, 13),
					new Solution(new sbyte[] {2, 4, 1, 7, 0, 6, 3, 5}, 14),
					new Solution(new sbyte[] {2, 4, 1, 7, 5, 3, 6, 0}, 15),
					new Solution(new sbyte[] {2, 4, 6, 0, 3, 1, 7, 5}, 16),
					new Solution(new sbyte[] {2, 4, 7, 3, 0, 6, 1, 5}, 17),
					new Solution(new sbyte[] {2, 5, 1, 4, 7, 0, 6, 3}, 18),
					new Solution(new sbyte[] {2, 5, 1, 6, 0, 3, 7, 4}, 19),
					new Solution(new sbyte[] {2, 5, 1, 6, 4, 0, 7, 3}, 20),
					new Solution(new sbyte[] {2, 5, 3, 0, 7, 4, 6, 1}, 21),
					new Solution(new sbyte[] {2, 5, 3, 1, 7, 4, 6, 0}, 22),
					new Solution(new sbyte[] {2, 5, 7, 0, 3, 6, 4, 1}, 23),
					new Solution(new sbyte[] {2, 5, 7, 0, 4, 6, 1, 3}, 24),
					new Solution(new sbyte[] {2, 5, 7, 1, 3, 0, 6, 4}, 25),
					new Solution(new sbyte[] {2, 6, 1, 7, 4, 0, 3, 5}, 26),
					new Solution(new sbyte[] {2, 6, 1, 7, 5, 3, 0, 4}, 27),
					new Solution(new sbyte[] {2, 7, 3, 6, 0, 5, 1, 4}, 28),
					new Solution(new sbyte[] {3, 0, 4, 7, 1, 6, 2, 5}, 29),
					new Solution(new sbyte[] {3, 0, 4, 7, 5, 2, 6, 1}, 30),
					new Solution(new sbyte[] {3, 1, 4, 7, 5, 0, 2, 6}, 31),
					new Solution(new sbyte[] {3, 1, 6, 2, 5, 7, 0, 4}, 32),
					new Solution(new sbyte[] {3, 1, 6, 2, 5, 7, 4, 0}, 33),
					new Solution(new sbyte[] {3, 1, 6, 4, 0, 7, 5, 2}, 34),
					new Solution(new sbyte[] {3, 1, 7, 4, 6, 0, 2, 5}, 35),
					new Solution(new sbyte[] {3, 1, 7, 5, 0, 2, 4, 6}, 36),
					new Solution(new sbyte[] {3, 5, 0, 4, 1, 7, 2, 6}, 37),
					new Solution(new sbyte[] {3, 5, 7, 1, 6, 0, 2, 4}, 38),
					new Solution(new sbyte[] {3, 5, 7, 2, 0, 6, 4, 1}, 39),
					new Solution(new sbyte[] {3, 6, 0, 7, 4, 1, 5, 2}, 40),
					new Solution(new sbyte[] {3, 6, 2, 7, 1, 4, 0, 5}, 41),
					new Solution(new sbyte[] {3, 6, 4, 1, 5, 0, 2, 7}, 42),
					new Solution(new sbyte[] {3, 6, 4, 2, 0, 5, 7, 1}, 43),
					new Solution(new sbyte[] {3, 7, 0, 2, 5, 1, 6, 4}, 44),
					new Solution(new sbyte[] {3, 7, 0, 4, 6, 1, 5, 2}, 45),
					new Solution(new sbyte[] {3, 7, 4, 2, 0, 6, 1, 5}, 46),
					new Solution(new sbyte[] {4, 0, 3, 5, 7, 1, 6, 2}, 47),
					new Solution(new sbyte[] {4, 0, 7, 3, 1, 6, 2, 5}, 48),
					new Solution(new sbyte[] {4, 0, 7, 5, 2, 6, 1, 3}, 49),
					new Solution(new sbyte[] {4, 1, 3, 5, 7, 2, 0, 6}, 50),
					new Solution(new sbyte[] {4, 1, 3, 6, 2, 7, 5, 0}, 51),
					new Solution(new sbyte[] {4, 1, 5, 0, 6, 3, 7, 2}, 52),
					new Solution(new sbyte[] {4, 1, 7, 0, 3, 6, 2, 5}, 53),
					new Solution(new sbyte[] {4, 2, 0, 5, 7, 1, 3, 6}, 54),
					new Solution(new sbyte[] {4, 2, 0, 6, 1, 7, 5, 3}, 55),
					new Solution(new sbyte[] {4, 2, 7, 3, 6, 0, 5, 1}, 56),
					new Solution(new sbyte[] {4, 6, 0, 2, 7, 5, 3, 1}, 57),
					new Solution(new sbyte[] {4, 6, 0, 3, 1, 7, 5, 2}, 58),
					new Solution(new sbyte[] {4, 6, 1, 3, 7, 0, 2, 5}, 59),
					new Solution(new sbyte[] {4, 6, 1, 5, 2, 0, 3, 7}, 60),
					new Solution(new sbyte[] {4, 6, 1, 5, 2, 0, 7, 3}, 61),
					new Solution(new sbyte[] {4, 6, 3, 0, 2, 7, 5, 1}, 62),
					new Solution(new sbyte[] {4, 7, 3, 0, 2, 5, 1, 6}, 63),
					new Solution(new sbyte[] {4, 7, 3, 0, 6, 1, 5, 2}, 64),
					new Solution(new sbyte[] {5, 0, 4, 1, 7, 2, 6, 3}, 65),
					new Solution(new sbyte[] {5, 1, 6, 0, 2, 4, 7, 3}, 66),
					new Solution(new sbyte[] {5, 1, 6, 0, 3, 7, 4, 2}, 67),
					new Solution(new sbyte[] {5, 2, 0, 6, 4, 7, 1, 3}, 68),
					new Solution(new sbyte[] {5, 2, 0, 7, 3, 1, 6, 4}, 69),
					new Solution(new sbyte[] {5, 2, 0, 7, 4, 1, 3, 6}, 70),
					new Solution(new sbyte[] {5, 2, 4, 6, 0, 3, 1, 7}, 71),
					new Solution(new sbyte[] {5, 2, 4, 7, 0, 3, 1, 6}, 72),
					new Solution(new sbyte[] {5, 2, 6, 1, 3, 7, 0, 4}, 73),
					new Solution(new sbyte[] {5, 2, 6, 1, 7, 4, 0, 3}, 74),
					new Solution(new sbyte[] {5, 2, 6, 3, 0, 7, 1, 4}, 75),
					new Solution(new sbyte[] {5, 3, 0, 4, 7, 1, 6, 2}, 76),
					new Solution(new sbyte[] {5, 3, 1, 7, 4, 6, 0, 2}, 77),
					new Solution(new sbyte[] {5, 3, 6, 0, 2, 4, 1, 7}, 78),
					new Solution(new sbyte[] {5, 3, 6, 0, 7, 1, 4, 2}, 79),
					new Solution(new sbyte[] {5, 7, 1, 3, 0, 6, 4, 2}, 80),
					new Solution(new sbyte[] {6, 0, 2, 7, 5, 3, 1, 4}, 81),
					new Solution(new sbyte[] {6, 1, 3, 0, 7, 4, 2, 5}, 82),
					new Solution(new sbyte[] {6, 1, 5, 2, 0, 3, 7, 4}, 83),
					new Solution(new sbyte[] {6, 2, 0, 5, 7, 4, 1, 3}, 84),
					new Solution(new sbyte[] {6, 2, 7, 1, 4, 0, 5, 3}, 85),
					new Solution(new sbyte[] {6, 3, 1, 4, 7, 0, 2, 5}, 86),
					new Solution(new sbyte[] {6, 3, 1, 7, 5, 0, 2, 4}, 87),
					new Solution(new sbyte[] {6, 4, 2, 0, 5, 7, 1, 3}, 88),
					new Solution(new sbyte[] {7, 1, 3, 0, 6, 4, 2, 5}, 89),
					new Solution(new sbyte[] {7, 1, 4, 2, 0, 6, 3, 5}, 90),
					new Solution(new sbyte[] {7, 2, 0, 5, 1, 4, 6, 3}, 91),
					new Solution(new sbyte[] {7, 3, 0, 2, 5, 1, 6, 4}, 92)
				}
			},
		};
		#endregion PrivateFields

		#region PrivateMethods
		private static int GetNoOfSingleSol()
		{ return 1; }

		private static int GetNoOfUniqueSols(sbyte boardSize)
		{ return noOfUniqueSols[boardSize]; }

		private static int GetNoOfAllSols(sbyte boardSize)
		{ return noOfAllSols[boardSize]; }

		private static List<Solution> GetSingleSol(sbyte boardSize)
		{ return singleSol[boardSize]; }

		private static List<Solution> GetUniqueSols(sbyte boardSize)
		{ return uniqueSol[boardSize]; }

		private static List<Solution> GetAllSols(sbyte boardSize)
		{ return allSol[boardSize]; }
		#endregion PrivateMethods
	}
}