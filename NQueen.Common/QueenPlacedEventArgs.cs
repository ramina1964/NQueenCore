using System;

namespace NQueen.Common
{
    public class QueenPlacedEventArgs : EventArgs
    {
        public QueenPlacedEventArgs(sbyte[] solution) => Solution = solution;

        public sbyte[] Solution { get; }
    }
}