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

namespace NQueen.Model
{
    public class Solver : ISolver
    {
        public Solver(sbyte boardSize, DisplayMode DisplayMode = DisplayMode.Hide) => Initialize(boardSize, DisplayMode);

        #region ISolverInterface

        public int DelayInMilliseconds { get; set; }

        public bool CancelSolver { get; set; }

        public SolutionMode SolutionMode { get; set; }

        public DisplayMode DisplayMode { get; set; }

        public HashSet<sbyte[]> Solutions = new HashSet<sbyte[]>(new SequenceEquality<sbyte>());

        public ObservableCollection<Solution> ObservableSolutions { get; set; }


        public event SolutionFoundHandler SolutionFound;

        public event QueenPlacedHandler QueenPlaced;

        public Task<ISimulationResults> GetSimulationResultsAsync(sbyte boardSize, SolutionMode solutionMode)
        {
            return Task.Factory.StartNew(() =>
            {
                Initialize(boardSize, DisplayMode);
                return GetResults(solutionMode);
            });
        }

        #endregion ISolverInterface

        #region PublicProperties
        public ISimulationResults Results { get; set; }

        public sbyte BoardSize { get; set; }

        public string BoardSizeText { get; set; }

        public int NoOfSolutions => ObservableSolutions.Count;

        public sbyte HalfSize { get; set; }

        public sbyte[] QueenList { get; set; }

        //public string ProgressLabel
        //{
        //	get => _progressLabel;
        //	set => Set(ref _progressLabel, value);
        //}
        #endregion PublicProperties

        #region VirtualMethods

        protected virtual void OnQueenPlaced(sbyte[] e) => QueenPlaced?.Invoke(this, e);

        protected virtual void OnSolutionFound(sbyte[] e) => SolutionFound?.Invoke(this, e);
        #endregion VirtualMethods

        public ISimulationResults GetResults(SolutionMode solutionMode)
        {
            var stopwatch = Stopwatch.StartNew();
            var solutions = MainSolve(solutionMode).ToList();
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

        #region PrivateMethods
        private void Initialize(sbyte boardSize, DisplayMode displayMode)
        {
            BoardSize = boardSize;
            HalfSize = (sbyte)(BoardSize % 2 == 0 ?
                BoardSize / 2 :
                BoardSize / 2 + 1);
            CancelSolver = false;
            DisplayMode = displayMode;

            QueenList = Enumerable.Repeat((sbyte)-1, BoardSize).ToArray();
            Solutions = new HashSet<sbyte[]>(new SequenceEquality<sbyte>());

            //ObservableSolutions = new ObservableCollection<Solution>();
            var solutionSize = Utility.FindSolutionSize(BoardSize, SolutionMode);
            ObservableSolutions = new ObservableCollection<Solution>(new List<Solution>(solutionSize));
        }
        private bool UpdateSolutions(IEnumerable<sbyte> solution, SolutionMode solutionMode)
        {
            var queens = solution.ToArray();

            // If solutionMode == SolutionMode.Single, then we are done.
            if (solutionMode == SolutionMode.Single)
            {
                Solutions.Add(queens);
                return true;
            }

            var symmetricalSolutions = Utility.GetSymmetricalSolutions(queens).ToList();

            // If solutionMode == SolutionMode.All, add this solution and all of the symmetrical counterparts to All Solutions.
            if (solutionMode == SolutionMode.All)
            {
                Solutions.Add(queens);
                symmetricalSolutions.ForEach(s => Solutions.Add(s));

                return true;
            }

            // One of symmetrical solutions is already in the solutions list, nothing to add.
            if (Solutions.Overlaps(symmetricalSolutions))
            { return false; }

            // None of the symmetrical solutions exists in the solutions list, add the new solution to the Unique Solutions.
            Solutions.Add(queens);
            return true;
        }

        private IEnumerable<Solution> MainSolve(SolutionMode solutionMode)
        {
            // Recursive call to start the simulation
            SolveRec(solutionMode);

            return Solutions
                    .Select((s, index) => new Solution(s, index + 1));
        }

        private bool SolveRec(SolutionMode solutionMode, sbyte colNo = 0)
        {
            if (CancelSolver)
            { return false; }

            if (DisplayMode == DisplayMode.Visualize)
            {
                OnQueenPlaced(QueenList);
                Thread.Sleep(DelayInMilliseconds);
            }

            if (solutionMode == SolutionMode.Single && NoOfSolutions == 1)
            { return true; }

            if (colNo == -1)
            { return false; }

            // Here a new solution is found.
            if (colNo == BoardSize)
            {
                bool isUpdated = UpdateSolutions(QueenList, solutionMode);

                // Activate this code in case of IsVisulaized == true.
                if (isUpdated && DisplayMode == DisplayMode.Visualize)
                { SolutionFound(this, QueenList); }

                //ProgressValue = Math.Round(100.0 * QueenList[0] / BoardSize);
                return false;
            }

            QueenList[colNo] = LocateQueen(colNo, solutionMode);
            if (QueenList[colNo] == -1)
            {
                return false;
            }

            var nextCol = (sbyte)(colNo + 1);
            return SolveRec(solutionMode, nextCol) || SolveRec(solutionMode, colNo);
        }

        private sbyte LocateQueen(sbyte colNo, SolutionMode solutionMode)
        {
            var isHalfSizeReachedMultSol = colNo == HalfSize && Solutions.Count > 0 &&
                Array.IndexOf<sbyte>(QueenList, 0, 0, HalfSize) == -1 && solutionMode != SolutionMode.Single;

            if (isHalfSizeReachedMultSol)
            { return -1; }

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

        #region PrivateFields
        //private double _progressValue;
        //private string _progressLabel;
        //private Visibility _progressBarVisibility;
        #endregion PrivateFields
    }
}