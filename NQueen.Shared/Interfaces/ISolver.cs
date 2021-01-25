using NQueen.Shared.Enums;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace NQueen.Shared.Interfaces
{
    public delegate void QueenPlacedHandler(object sender, QueenPlacedEventArgs e);
    public delegate void SolutionFoundHandler(object sender, SolutionFoundEventArgs e);
    public delegate void ProgressValueChangedHandler(object sender, ProgressValueChangedEventArgs e);

    public interface ISolver
    {
        sbyte BoardSize { get; set; }

        int DelayInMilliseconds { get; set; }

        bool CancelSolver { get; set; }

        SolutionMode SolutionMode { get; set; }

        DisplayMode DisplayMode { get; set; }

        double ProgressValue { get; set; }

        ObservableCollection<Solution> ObservableSolutions { get; set; }

        Task<ISimulationResults> GetSimulationResultsAsync(sbyte boardSize, SolutionMode solutionMode, DisplayMode displayMode);

        event QueenPlacedHandler QueenPlaced;

        event SolutionFoundHandler SolutionFound;

        event ProgressValueChangedHandler ProgressValueChanged;
    }
}