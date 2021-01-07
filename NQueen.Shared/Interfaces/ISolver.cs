using NQueen.Shared.Enums;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace NQueen.Shared.Interfaces
{
    public delegate void QueenPlacedHandler(object sender, QueenPlacedEventArgs e);
    public delegate void SolutionFoundHandler(object sender, SolutionFoundEventArgs e);
    public delegate void ProgressChangedHandler(object sender, ProgressValueChangedEventArgs e);

    public interface ISolver
    {
        int DelayInMilliseconds { get; set; }

        bool CancelSolver { get; set; }

        SolutionMode SolutionMode { get; set; }

        DisplayMode DisplayMode { get; set; }

        double ProgressValue { get; set; }

        ObservableCollection<Solution> ObservableSolutions { get; set; }

        Task<ISimulationResults> GetSimulationResultsAsync(sbyte boardSize, SolutionMode solutionMode, DisplayMode displayMode);

        event ProgressChangedHandler RaiseProgressChanged;

        event QueenPlacedHandler QueenPlaced;

        event SolutionFoundHandler SolutionFound;
    }
}