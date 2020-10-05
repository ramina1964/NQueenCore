using NQueen.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NQueen.Common
{
	public class Solution : ISolution
	{
		#region Constructor
		public Solution(sbyte[] queenList, int id)
		{
			BoardSize = queenList.Length;
			Id = id;
			Name = ToString();
			QueenList = queenList.Select(e => (int)e).ToArray();
			Positions = SetPositions(QueenList);
			Details = GetDetails();
		}
		#endregion Constructor

		#region PublicProperties
		public List<Position> Positions;
		public int Id { get; }
		public string Name { get; set; }
		public string Details { get; set; }
		public sealed override string ToString() => $"No. {Id}";
		public int[] QueenList { get; }
		#endregion PublicProperties

		#region PrivateMembers
		private int BoardSize { get; }
		private string GetDetails()
		{
			//const int noOfQueensPerLine = 20;
			const int noOfQueensPerLine = 40;
			var noOfLines = (BoardSize % noOfQueensPerLine == 0) ?
				BoardSize / noOfQueensPerLine :
				BoardSize / noOfQueensPerLine + 1;

			var sb = new StringBuilder();
			for (var lineNo = 0; lineNo < noOfLines; lineNo++)
			{
				var maxQueensInLastLine = (lineNo < noOfLines - 1 || BoardSize % noOfQueensPerLine == 0) ?
					noOfQueensPerLine :
					Math.Min(BoardSize % noOfQueensPerLine, noOfQueensPerLine);

				for (var posInLine = 0; posInLine < maxQueensInLastLine; posInLine++)
				{
					var posNo = noOfQueensPerLine * lineNo + posInLine;
					sb.Append($"({Positions[posNo].Row + 1,0:N0}, {Positions[posNo].Column + 1,0:N0})");

					if (posNo < BoardSize - 1)
						sb.Append(", ");
				}

				if (lineNo < noOfLines - 1)
				{ sb.AppendLine(); }
			}

			return sb.ToString();
		}

		private List<Position> SetPositions(IEnumerable<int> queenList)
		{
			return queenList.Select((item, index) =>
				new Position(index, item)).ToList();
		}
		#endregion PrivateMembers
	}
}