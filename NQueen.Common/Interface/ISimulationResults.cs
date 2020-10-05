using System.Collections.Generic;

namespace NQueen.Common.Interface
{
	public interface ISimulationResults
	{
		int BoardSize { get; set; }
		IEnumerable<Solution> Solutions { get; set; }
		int NoOfSolutions { get; set; }
		double ElapsedTimeInSec { get; set; }
	}
}