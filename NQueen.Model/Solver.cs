
using NQueen.Common;
using NQueen.Common.Enum;
using NQueen.Common.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace NQueen.Model
{
    public class Solver : ISolver
    {
        #region Constructor
        public Solver(sbyte boardSize, DisplayMode DisplayMode = DisplayMode.Hide) => Initialize(boardSize, DisplayMode);
        #endregion Constructor

        #region ISolverInterface
        public int DelayInMilliseconds { get; set; }

        public bool CancelSolver { get; set; }

        public SolutionMode SolutionMode { get; set; }

        public DisplayMode DisplayMode { get; set; }

        public ObservableCollection<Solution> Solutions { get; set; }

        public event QueenPlacedHandler QueenPlaced;

        public event SolutionFoundHandler SolutionFound;

        public Task<ISimulationResults> GetSimulationResultsAsync(sbyte boardSize, SolutionMode solutionMode)
        {
            return Task.Factory.StartNew(() =>
            {
                Initialize(boardSize, DisplayMode);
                return GetResults(solutionMode);
            });
        }

        public ISimulationResults GetResults(SolutionMode solutionMode)
        {
            var stopwatch = Stopwatch.StartNew();
            var solutions = FindSolutions(solutionMode).ToList();
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

        //public Visibility ProgressVisibility
        //{
        //	get => _progressBarVisibility;
        //	set => Set(ref _progressBarVisibility, value);
        //}

        //public double ProgressValue
        //{
        //	get => _progressValue;
        //	set
        //	{
        //		Set(ref _progressValue, value);
        //		Set(ref _progressLabel, $"{value}%");
        //		RaisePropertyChanged(nameof(ProgressLabel));
        //	}
        //}
        #endregion ISolverInterface

        #region PublicProperties
        public ISimulationResults Results { get; set; }
        public sbyte BoardSize { get; set; }
        public string BoardSizeText { get; set; }
        public int NoOfSolutions => Solutions.Count();
        public sbyte HalfSize { get; set; }
        public sbyte[] QueenList { get; set; }

        //public string ProgressLabel
        //{
        //	get => _progressLabel;
        //	set => Set(ref _progressLabel, value);
        //}
        #endregion PublicProperties

        #region VirtualMethods
        protected virtual void OnQueenPlaced(sbyte[] queenList) => QueenPlaced?.Invoke(this, new QueenPlacedEventArgs(queenList));

        protected virtual void OnSolutionFound(sbyte[] solution) => SolutionFound?.Invoke(this, new SolutionFoundEventArgs(solution));
        #endregion VirtualMethods

        #region PrivateMethods
        private void Initialize(sbyte boardSize, DisplayMode displayMode)
        {
            BoardSize = boardSize;
            HalfSize = (sbyte)(BoardSize % 2 == 0 ?
                BoardSize / 2 :
                BoardSize / 2 + 1);

            QueenList = Enumerable.Repeat((sbyte)-1, BoardSize).ToArray();
            DisplayMode = displayMode;

            var solutionSize = Utility.FindSolutionSize(BoardSize, SolutionMode);

            Solutions = new ObservableCollection<Solution>(new List<Solution>(solutionSize));
            CancelSolver = false;
        }

        private bool UpdateSols(IEnumerable<sbyte> solution, HashSet<sbyte[]> solutions, SolutionMode solutionMode)
        {
            var queens = solution.ToArray();

            // If solutionMode == SolutionMode.Single, then we are done.
            if (solutionMode == SolutionMode.Single)
            {
                solutions.Add(queens);
                return true;
            }

            var symmSols = Utility.GetSymmSols(queens).ToList();

            // If solutionMode == SolutionMode.All, add this solution and all of the symmetrical counterparts to All Solutions.
            if (solutionMode == SolutionMode.All)
            {
                solutions.Add(queens);
                symmSols.ForEach(s => solutions.Add(s));

                return true;
            }

            // One of symmetrical solutions is already in the solutions list, nothing to add.
            if (solutions.Overlaps(symmSols))
            { return false; }

            // None of the symmetrical solutions exists in the solutions list, add the new solution to the Unique Solutions.
            solutions.Add(queens);
            return true;
        }

        private IEnumerable<Solution> FindSolutions(SolutionMode solutionMode)
        {
            var solutions = new HashSet<sbyte[]>(new SequenceEquality<sbyte>());
            var maxSolutionSize = Utility.FindSolutionSize(BoardSize, solutionMode);

            // Private function FindValidPos
            sbyte FindPos(sbyte rowNo = 0)
            {
                var isHalfSizeReachedMultSol = rowNo == HalfSize && solutions.Count > 0 &&
                    Array.IndexOf<sbyte>(QueenList, 0, 0, HalfSize) == -1 && solutionMode != SolutionMode.Single;

                if (isHalfSizeReachedMultSol)
                { return -1; }

                for (var pos = (sbyte)(QueenList[rowNo] + 1); pos < BoardSize; pos++)
                {
                    var isValid = true;
                    for (var j = 0; j < rowNo; j++)
                    {
                        var lhs = Math.Abs(pos - QueenList[j]);
                        var rhs = Math.Abs(rowNo - j);
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

            // Private function FindSolsRec
            bool FindSolsRec(sbyte row = 0)
            {
                if (CancelSolver)
                { return false; }

                if (DisplayMode == DisplayMode.Visualize)
                {
                    OnQueenPlaced(QueenList);
                    Thread.Sleep(DelayInMilliseconds);
                }

                if (solutionMode == SolutionMode.Single && solutions.Count == 1)
                { return true; }

                if (row == -1)
                { return false; }

                // Here a new solution is found.
                if (row == BoardSize)
                {
                    var isUpdated = UpdateSols(QueenList, solutions, solutionMode);

                    // Activate this code in case of IsVisulaized == true.
                    if (isUpdated && DisplayMode == DisplayMode.Visualize)
                    { OnSolutionFound(QueenList); }

                    //ProgressValue = Math.Round(100.0 * solutions.Count / maxSolutionSize);
                    return false;
                }

                QueenList[row] = FindPos(row);
                if (QueenList[row] == -1)
                {
                    return false;
                }

                return FindSolsRec((sbyte)(row + 1)) || FindSolsRec(row);
            }

            // Recursive call to start the simulation
            FindSolsRec(0);

            return solutions
                    .Select((s, index) => new Solution(s, index + 1));
        }
        #endregion PrivateMethods

        #region PrivateFields
        private double _progressValue;
        private string _progressLabel;
        //private Visibility _progressBarVisibility;
        #endregion PrivateFields
    }
}