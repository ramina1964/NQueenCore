using System;

namespace NQueen.Shared
{
    public class SolutionFoundEventArgs : EventArgs
    {
        public SolutionFoundEventArgs(sbyte[] solution) => Solution = solution;

        public sbyte[] Solution { get; }
    }
}