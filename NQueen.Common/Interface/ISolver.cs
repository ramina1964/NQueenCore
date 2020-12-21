using NQueen.Common.Enum;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace NQueen.Common.Interface
{
    public delegate void QueenPlacedHandler(object sender, QueenPlacedEventArgs e);
    public delegate void SolutionFoundHandler(object sender, SolutionFoundEventArgs e);

    public interface ISolver
    {
        int DelayInMilliseconds { get; set; }

        bool CancelSolver { get; set; }

        SolutionMode SolutionMode { get; set; }

        DisplayMode DisplayMode { get; set; }

        //Visibility ProgressVisibility { get; set; }

        //double ProgressValue { get; set; }

        ObservableCollection<Solution> ObservableSolutions { get; set; }

        Task<ISimulationResults> GetSimulationResultsAsync(sbyte boardSize, SolutionMode solutionMode);

        event QueenPlacedHandler QueenPlaced;

        event SolutionFoundHandler SolutionFound;
    }
}