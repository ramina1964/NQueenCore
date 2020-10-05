using NQueen.Common;
using NQueen.Common.Interface;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NQueen.Model
{
	public class SimulationResults : ISimulationResults
	{
		#region Constructor
		public SimulationResults(IEnumerable<Solution> allSolutions)
		{
			Debug.Assert(allSolutions != null, "allSolutions != null");
			var enumerable = allSolutions as IList<Solution> ?? allSolutions.ToList();
			var sol = enumerable.FirstOrDefault();
			if (sol == null)
			{
				NoOfSolutions = 0;
				Solutions = new List<Solution>();
			}
			else
			{
				BoardSize = sol.Positions.Count;
				NoOfSolutions = enumerable.Count();
				Solutions = new List<Solution>(enumerable);
			}
		}
		#endregion Constructor

		#region PublicProperties
		public int BoardSize { get; set; }
		public IEnumerable<Solution> Solutions { get; set; }
		public int NoOfSolutions { get; set; }
		public double ElapsedTimeInSec { get; set; }
		#endregion PublicProperties
	}
}