using NQueen.Shared;
using NQueen.Shared.Enums;
using NQueen.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NQueen.Kernel
{
    public class Solver : ISolver
    {
        public Solver(sbyte boardSize) => Initialize(boardSize);

        #region ISolverInterface

        public int DelayInMilliseconds { get; set; }

        public bool CancelSolver { get; set; }

        public SolutionMode SolutionMode { get; set; }

        public DisplayMode DisplayMode { get; set; }

        public HashSet<sbyte[]> Solutions { get; set; }

        public ObservableCollection<Solution> ObservableSolutions { get; set; }

        public event QueenPlacedHandler QueenPlaced;

        public event SolutionFoundHandler SolutionFound;

        public event ProgressValueChangedHandler ProgressValueChanged;

        public Task<ISimulationResults> GetSimulationResultsAsync(sbyte boardSize, SolutionMode solutionMode, DisplayMode displayMode = DisplayMode.Hide)
        {
            Initialize(boardSize);
            SolutionMode = solutionMode;
            DisplayMode = displayMode;
            return Task.Factory.StartNew(() => GetResults() );
        }

        #endregion ISolverInterface

        public ISimulationResults GetResults()
        {
            var stopwatch = Stopwatch.StartNew();
            var solutions = MainSolve().ToList();
            stopwatch.Stop();
            var timeInSec = (double)stopwatch.ElapsedMilliseconds / 1000;
            var elapsedTimeInSec = Math.Round(timeInSec, 1);

            return new SimulationResults(solutions)
            {
                BoardSize = BoardSize,
                NoOfSolutions = solutions.Count,
                Solutions = solutions,
                ElapsedTimeInSec = elapsedTimeInSec
            };
        }

        #region PublicProperties

        public double ProgressValue { get; set; }

        public sbyte BoardSize { get; set; }

        public string BoardSizeText { get; set; }

        public int NoOfSolutions => Solutions.Count;

        public sbyte HalfSize { get; set; }

        public sbyte[] QueenList { get; set; }
        #endregion PublicProperties

        protected virtual void OnProgressChanged(object sender, ProgressValueChangedEventArgs e) => ProgressValueChanged?.Invoke(this, e);

        protected virtual void OnQueenPlaced(object sender, QueenPlacedEventArgs e) => QueenPlaced?.Invoke(this, e);

        protected virtual void OnSolutionFound(object sender, SolutionFoundEventArgs e) => SolutionFound?.Invoke(this, e);

        #region PrivateMethods
        private void Initialize(sbyte boardSize)
        {
            BoardSize = boardSize;
            CancelSolver = false;

            HalfSize = (sbyte)(BoardSize % 2 == 0 ?
                BoardSize / 2 :
                BoardSize / 2 + 1);

            QueenList = Enumerable.Repeat((sbyte)-1, BoardSize).ToArray();
            Solutions = new HashSet<sbyte[]>(new SequenceEquality<sbyte>());
            var solutionSize = Utility.FindSolutionSize(BoardSize, SolutionMode);
            ObservableSolutions = new ObservableCollection<Solution>(new List<Solution>(solutionSize));
        }

        private IEnumerable<Solution> MainSolve()
        {
            // Recursive call to start the simulation
            RecSolve(0);

            return Solutions
                    .Select((s, index) => new Solution(s, index + 1));
        }

        private bool RecSolve(sbyte colNo)
        {
            if (CancelSolver || colNo == -1)
            { return false; }

            // All Symmetrical solutions are found and registered. Quit the recursion.
            if (QueenList[0] == HalfSize)
            {
                ProgressValue = Math.Round(100.0 * QueenList[0] / HalfSize, 1);
                OnProgressChanged(this, new ProgressValueChangedEventArgs(ProgressValue));
                return false;
            }

            if (SolutionMode == SolutionMode.Single && NoOfSolutions == 1)
            { return true; }

            // Here a new solution is found.
            if (colNo == BoardSize)
            {
                UpdateSolutions(QueenList.ToArray());

                // Activate this code in case of IsVisulaized == true.
                if (DisplayMode == DisplayMode.Visualize)
                { SolutionFound(this, new SolutionFoundEventArgs(QueenList)); }

                ProgressValue = Math.Round(100.0 * QueenList[0] / HalfSize, 1);
                OnProgressChanged(this, new ProgressValueChangedEventArgs(ProgressValue));
                return false;
            }

            QueenList[colNo] = PlaceQueen(colNo);
            if (QueenList[colNo] == -1)
            { return false; }

            if (DisplayMode == DisplayMode.Visualize)
            {
                OnQueenPlaced(this, new QueenPlacedEventArgs(QueenList));
                Thread.Sleep(DelayInMilliseconds);
            }

            var nextCol = (sbyte)(colNo + 1);
            return RecSolve(nextCol) || RecSolve(colNo);
        }

        private void UpdateSolutions(sbyte[] solution)
        {
            // For SolutionMode.Single:
            if (SolutionMode == SolutionMode.Single)
            {
                Solutions.Add(solution);
                return;
            }

            var symmetricalSolutions = Utility.GetSymmetricalSolutions(solution);

            // For SolutionMode.All, add this solution and all its symmetrical counterparts to Solutions.
            if (SolutionMode == SolutionMode.All)
            {
                Solutions.Add(solution);
                symmetricalSolutions.ForEach(s => Solutions.Add(s));
                return;
            }

            // For SolutionMode.Unique: Add this solution to Solutions only if no overlaps between Solutions and symmetricalSolutions are found.
            if (!Solutions.Overlaps(symmetricalSolutions))
            { Solutions.Add(solution); }
        }

        // Return the first available row for the queen in column "colNo", -1 if impossible.
        private sbyte PlaceQueen(sbyte colNo)
        {
            for (sbyte pos = (sbyte)(QueenList[colNo] + 1); pos < BoardSize; pos++)
            {
                var isValid = true;
                for (int j = 0; j < colNo; j++)
                {
                    int lhs = Math.Abs(pos - QueenList[j]);
                    int rhs = Math.Abs(colNo - j);
                    if (0 != lhs && lhs != rhs)
                    { continue; }

                    isValid = false;
                    break;
                }

                if (isValid)
                { return pos; }
            }

            return -1;
        }

        #endregion PrivateMethods
    }
}