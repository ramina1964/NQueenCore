using NQueen.Shared;
using System.Collections.Generic;

namespace NQueen.Shared.Interfaces
{
    public interface ISimulationResults
    {
        sbyte BoardSize { get; }

        IEnumerable<Solution> Solutions { get; }

        int NoOfSolutions { get; }

        double ElapsedTimeInSec { get; }
    }
}