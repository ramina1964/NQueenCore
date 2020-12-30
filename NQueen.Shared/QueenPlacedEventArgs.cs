using System;

namespace NQueen.Shared
{
    public class QueenPlacedEventArgs : EventArgs
    {
        public QueenPlacedEventArgs(sbyte[] solution) => Solution = solution;

        public sbyte[] Solution { get; }
    }
}