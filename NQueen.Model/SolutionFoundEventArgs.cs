using System;

namespace NQueen.Model
{
    public class SolutionFoundEventArgs : EventArgs
    {
        public SolutionFoundEventArgs(sbyte[] solution) => Solution = solution;

        public sbyte[] Solution { get; }
    }
}