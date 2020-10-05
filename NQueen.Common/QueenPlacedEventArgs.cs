using System;

namespace NQueen.Common
{
    public class QueenPlacedEventArgs : EventArgs
    {
        public QueenPlacedEventArgs(sbyte[] queenList)
        {
            QueenList = queenList;
        }

        public sbyte[] QueenList { get; }
    }
}