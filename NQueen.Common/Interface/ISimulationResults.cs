using System.Collections.Generic;

namespace NQueen.Common.Interface
{
	public interface ISimulationResults
	{
		int BoardSize { get; }

		IEnumerable<Solution> Solutions { get; }

		int NoOfSolutions { get; }

		double ElapsedTimeInSec { get; }
	}
}