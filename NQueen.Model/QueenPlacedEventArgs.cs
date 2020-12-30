using System;

namespace NQueen.Model
{
    public class QueenPlacedEventArgs : EventArgs
    {
        public QueenPlacedEventArgs(sbyte[] solution) => Solution = solution;

        public sbyte[] Solution { get; }
    }
}