using NQueen.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NQueen.Shared
{
    public class Solution : ISolution
    {
        public Solution(sbyte[] queenList, int? id = null)
        {
            BoardSize = queenList.Length;
            Id = id;
            Name = ToString();
            QueenList = queenList; //.ToArray();
            Positions = SetPositions(QueenList);
            Details = GetDetails();
        }

        #region PublicProperties
        public List<Position> Positions;

        public int? Id { get; }

        public string Name { get; set; }

        public string Details { get; set; }

        public sealed override string ToString() => $"No. {Id}";

        public sbyte[] QueenList { get; }
        #endregion PublicProperties

        #region PrivateMembers
        private int BoardSize { get; }
        private string GetDetails()
        {
            const int noOfQueensPerLine = 40;
            var noOfLines = (BoardSize % noOfQueensPerLine == 0) ?
                BoardSize / noOfQueensPerLine :
                BoardSize / noOfQueensPerLine + 1;

            StringBuilder sb = new StringBuilder();
            for (var lineNo = 0; lineNo < noOfLines; lineNo++)
            {
                var maxQueensInLastLine = (lineNo < noOfLines - 1 || BoardSize % noOfQueensPerLine == 0) ?
                    noOfQueensPerLine :
                    Math.Min(BoardSize % noOfQueensPerLine, noOfQueensPerLine);

                for (var posInLine = 0; posInLine < maxQueensInLastLine; posInLine++)
                {
                    var posNo = noOfQueensPerLine * lineNo + posInLine;
                    sb.Append($"({Positions[posNo].RowNo + 1,0:N0}, {Positions[posNo].ColumnNo + 1,0:N0})");

                    if (posNo < BoardSize - 1)
                        sb.Append(", ");
                }

                if (lineNo < noOfLines - 1)
                { sb.AppendLine(); }
            }

            return sb.ToString();
        }

        private static List<Position> SetPositions(IEnumerable<sbyte> queenList)
        {
            return queenList.Select((item, index) =>
                new Position((sbyte)index, item)).ToList();
        }
        #endregion PrivateMembers
    }

}