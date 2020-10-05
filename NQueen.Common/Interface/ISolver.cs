using NQueen.Common.Enum;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace NQueen.Common.Interface
{
	public delegate void SolutionFoundDelegate(object sender, sbyte[] e);

	public delegate void PlaceQueenDelegate(object sender, sbyte[] e);

	public interface ISolver
	{
		int DelayInMilliseconds { get; set; }
		bool CancelSolver { get; set; }
		SolutionMode SolutionMode { get; set; }
		DisplayMode DisplayMode { get; set; }
		//Visibility ProgressVisibility { get; set; }
		//double ProgressValue { get; set; }
		ObservableCollection<Solution> Solutions { get; set; }

		event PlaceQueenDelegate QueenPlaced;
		event SolutionFoundDelegate ShowSolution;
		Task<ISimulationResults> GetSimulationResultsAsync(sbyte boardSize, SolutionMode solutionMode);
	}
}