using System;

namespace NQueen.Common
{
    public class SolutionFoundEventArgs : EventArgs
    {
        public SolutionFoundEventArgs(sbyte[] solution) => Solution = solution;

        public sbyte[] Solution { get; }
    }
}