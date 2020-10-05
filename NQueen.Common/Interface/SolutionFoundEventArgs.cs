using System;

namespace NQueen.Common.Interface
{
    public class SolutionFoundEventArgs : EventArgs
    {
        public SolutionFoundEventArgs(sbyte[] solution)
        { Solution = solution; }

        public sbyte[] Solution { get; }
    }
}