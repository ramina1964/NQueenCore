using System.Collections.Generic;

namespace NQueen.Common.Interface
{
    public interface ISimulationResults
    {
        sbyte BoardSize { get; }

        IEnumerable<Solution> Solutions { get; }

        int NoOfSolutions { get; }

        double ElapsedTimeInSec { get; }
    }
}