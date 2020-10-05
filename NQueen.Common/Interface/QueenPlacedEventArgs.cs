using System;

namespace NQueen.Common.Interface
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