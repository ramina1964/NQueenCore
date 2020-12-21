using System;

namespace NQueen.Common
{
    public class QueenPlacedEventArgs : EventArgs
    {
        public QueenPlacedEventArgs(sbyte[] solution) => QueenList = solution;

        public sbyte[] QueenList { get; }
    }
}