using FluentValidation;
using FluentValidation.Results;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NQueen.Shared;
using NQueen.Shared.Enums;
using NQueen.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace NQueen.GUI.ViewModel
{
    public sealed class MainViewModel : ViewModelBase, IDataErrorInfo
    {
        public MainViewModel(ISolver solver)
        {
            Initialize(solver);
            Solver.QueenPlaced += OnQueenPlaced;
            Solver.SolutionFound += OnSolutionFound;
            Solver.ProgressValueChanged += OnProgressValueChanged;
        }

        #region IDataErrorInfo
        public string this[string columnName]
        {
            get
            {
                var firstOrDefault = _validation.Validate(this).Errors.FirstOrDefault(lol => lol.PropertyName == columnName);
                if (firstOrDefault != null)
                { return _validation != null ? firstOrDefault.ErrorMessage : ""; }

                return string.Empty;
            }
        }

        public string Error
        {
            get
            {
                var results = _validation?.Validate(this);
                if (results == null || !results.Errors.Any())
                { return string.Empty; }

                var errors = string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage).ToArray());
                return errors;
            }
        }
        #endregion IDataErrorInfo

        #region PublicProperties
        public RelayCommand SimulateCommand { get; set; }

        public RelayCommand CancelCommand { get; set; }

        public RelayCommand SaveCommand { get; set; }

        public double ProgressValue
        {
            get { return _progressValue; }
            set
            {
                if (Set(ref _progressValue, value))
                {
                    ProgressLabel = $"{_progressValue} %";
                    RaisePropertyChanged(nameof(ProgressLabel));
                }
            }
        }

        public string ProgressLabel
        {
            get => _progressLabel;
            set => Set(ref _progressLabel, value);
        }

        public Visibility ProgressVisibility
        {
            get => _progressVisibility;
            set
            {
                if (Set(ref _progressVisibility, value))
                { RaisePropertyChanged(nameof(ProgressLabel)); }
            }
        }

        public Visibility BusyIndicatorVisibility
        {
            get => _busyIndicatorVisibility;
            set => Set(ref _busyIndicatorVisibility, value);
        }

        public IEnumerable<SolutionMode> EnumSolutionModes
        {
            get => Enum.GetValues(typeof(SolutionMode)).Cast<SolutionMode>();
            set => Set(ref _enumSolutionModes, value);
        }

        public IEnumerable<DisplayMode> EnumDisplayModes
        {
            get => Enum.GetValues(typeof(DisplayMode)).Cast<DisplayMode>();
            set => Set(ref _enumDisplayModes, value);
        }

        public bool IsVisualized
        {
            get => _isVisualized;
            set => Set(ref _isVisualized, value);
        }

        public int DelayInMilliseconds
        {
            get => _delayInMilliseconds;
            set
            {
                Set(ref _delayInMilliseconds, value);
                Solver.DelayInMilliseconds = value;
            }
        }

        public ISimulationResults SimulationResults
        {
            get => _simulationResults;
            set => Set(ref _simulationResults, value);
        }

        public ObservableCollection<Solution> ObservableSolutions { get; set; }

        public Solution SelectedSolution
        {
            get => _selectedSolution;
            set
            {
                Set(ref _selectedSolution, value);
                if (value != null)
                { Chessboard.PlaceQueens(_selectedSolution.Positions); }
            }
        }

        public SolutionMode SolutionMode
        {
            get => _solutionMode;
            set
            {
                var isChanged = Set(ref _solutionMode, value);
                if (Solver == null || !isChanged)
                { return; }

                Solver.SolutionMode = value;
                SolutionTitle =
                    (SolutionMode == SolutionMode.Single)
                    ? $"Solution"
                    : $"Solutions (Max: {Utility.MaxNoOfSolutionsInOutput})";

                RaisePropertyChanged(nameof(BoardSizeText));
                RaisePropertyChanged(nameof(SolutionTitle));
                ValidationResult = _validation.Validate(this, options => options.IncludeAllRuleSets());
                IsValid = ValidationResult.IsValid;

                if (!IsValid)
                {
                    IsIdle = false;
                    IsRunning = false;
                    IsOutputReady = false;
                    return;
                }

                IsIdle = true;
                IsRunning = false;
                UpdateGui();
            }
        }

        public DisplayMode DisplayMode
        {
            get => _displayMode;
            set
            {
                var isChanged = Set(ref _displayMode, value);
                if (Solver != null || !isChanged)
                { Solver.DisplayMode = value; }

                ValidationResult = _validation.Validate(this);
                IsValid = ValidationResult.IsValid;

                if (IsValid)
                {
                    IsIdle = true;
                    IsRunning = false;
                    IsOutputReady = false;
                    IsVisualized = DisplayMode.Visualize == value;
                    RaisePropertyChanged(nameof(BoardSizeText));
                    UpdateGui();
                }
            }
        }

        public string BoardSizeText
        {
            get => _boardSizeText;
            set
            {
                if (!Set(ref _boardSizeText, value))
                { return; }

                ValidationResult = _validation.Validate(this);
                IsValid = ValidationResult.IsValid;
                if (!IsValid)
                {
                    IsIdle = false;
                    IsRunning = false;
                }

                else
                {
                    IsIdle = true;
                    IsRunning = false;
                    IsOutputReady = false;
                    Set(ref _boardSize, sbyte.Parse(value));
                    RaisePropertyChanged(nameof(BoardSize));
                    SaveCommand.RaiseCanExecuteChanged();
                    UpdateGui();
                }
            }
        }

        public sbyte BoardSize
        {
            get => _boardSize;
            set => Set(ref _boardSize, value);
        }

        public ValidationResult ValidationResult { get; set; }

        public string ResultTitle => Utility.SolutionTitle(SolutionMode);

        public bool IsValid
        {
            get => _isValid;
            set => Set(ref _isValid, value);
        }

        public ISolver Solver
        {
            get => _solver;
            set => Set(ref _solver, value);
        }

        public string SolutionTitle
        {
            get => _solutionTitle;
            set => Set(ref _solutionTitle, value);
        }

        public string NoOfSolutions
        {
            get => _noOfSoltions;
            set
            {
                if (Set(ref _noOfSoltions, value))
                { RaisePropertyChanged(nameof(ResultTitle)); }
            }
        }

        public Chessboard Chessboard { get; set; }

        public void SetChessboard(double boardDimension)
        {
            BoardSizeText = BoardSize.ToString();
            Chessboard = new Chessboard { WindowWidth = boardDimension, WindowHeight = boardDimension };
            Chessboard.CreateSquares(BoardSize, new List<SquareViewModel>());

            IsIdle = true;
            IsRunning = false;
            SolutionMode = SolutionMode.Unique;
            DisplayMode = DisplayMode.Hide;
        }

        public string ElapsedTimeInSec
        {
            get => _elapsedTime;
            set => Set(ref _elapsedTime, value);
        }

        // Returns true if a simulation is running, false otherwise.
        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                if (Set(ref _isRunning, value))
                { UpdateButtonFunctionality(); }
            }
        }

        public bool IsInInputMode
        {
            get => _isInInputMode;
            set
            {
                if (Set(ref _isInInputMode, value))
                { UpdateButtonFunctionality(); }
            }
        }

        public bool IsSingleRunning
        {
            get => _isSingleRunning;
            set => Set(ref _isSingleRunning, value);
        }

        public bool IsIdle
        {
            get => _isIdle;
            set
            {
                if (Set(ref _isIdle, value))
                { UpdateButtonFunctionality(); }
            }
        }

        public bool IsOutputReady
        {
            get => _isOutputReady;
            set
            {
                if (Set(ref _isOutputReady, value))
                { UpdateButtonFunctionality(); }
            }
        }

        #endregion PublicProperties

        #region PrivateMethods
        private void Initialize(ISolver solver)
        {
            _validation = new InputViewModel { CascadeMode = CascadeMode.Stop };
            SimulateCommand = new RelayCommand(SimulateAsync, CanSimulate);
            CancelCommand = new RelayCommand(Cancel, CanCancel);
            SaveCommand = new RelayCommand(Save, CanSave);

            Solver = solver;
            BoardSize = Solver.BoardSize;
            IsIdle = true;
            IsInInputMode = true;
            IsRunning = false;
            IsOutputReady = false;

            ObservableSolutions = Solver.ObservableSolutions;
            NoOfSolutions = $"{ObservableSolutions.Count,0:N0}";

            DelayInMilliseconds = Utility.DefaultDelayInMilliseconds;
            ProgressVisibility = Visibility.Hidden;
            BusyIndicatorVisibility = Visibility.Hidden;
            ProgressValue = 0;
        }

        private void UpdateGui()
        {
            Solver.ObservableSolutions.Clear();
            BoardSize = sbyte.Parse(BoardSizeText);
            NoOfSolutions = "0";
            ElapsedTimeInSec = $"{0,0:N1}";
            RaisePropertyChanged(nameof(ResultTitle));
            ObservableSolutions?.Clear();
            Chessboard?.Squares.Clear();
            Chessboard?.CreateSquares(BoardSize, new List<SquareViewModel>());
        }

        private void UpdateProgressIndicator(SimulationStatus simulationStatus)
        {
            IsIdle = true;
            IsInInputMode = true;
            IsRunning = false;
            IsSingleRunning = false;
            IsOutputReady = true;
            BusyIndicatorVisibility = Visibility.Hidden;
            ProgressVisibility = Visibility.Hidden;

            switch (simulationStatus)
            {
                case SimulationStatus.Started:
                    IsIdle = false;
                    IsInInputMode = false;
                    IsRunning = true;
                    IsOutputReady = false;

                    if (SolutionMode == SolutionMode.Single)
                    {
                        IsSingleRunning = true;
                        BusyIndicatorVisibility = Visibility.Visible;
                    }

                    // If SolutionMode.Unique || SolutionMode.All
                    else if (IsRunning)
                    {
                        IsSingleRunning = false;
                        ProgressVisibility = Visibility.Visible;
                        ProgressValue = 0;
                    }
                    break;

                case SimulationStatus.Finished:
                    break;
            }
        }

        private void UpdateButtonFunctionality()
        {
            CancelCommand.RaiseCanExecuteChanged();
            SimulateCommand.RaiseCanExecuteChanged();
            SaveCommand.RaiseCanExecuteChanged();
        }

        private void OnProgressValueChanged(object sender, ProgressValueChangedEventArgs e) => ProgressValue = e.Value;

        private void OnQueenPlaced(object sender, QueenPlacedEventArgs e)
        {
            var sol = new Solution(e.Solution, 1);
            var positions = sol
                            .QueenList.Where(q => q > -1)
                            .Select((item, index) => new Position((sbyte)index, item)).ToList();

            Chessboard.PlaceQueens(positions);
        }

        private void OnSolutionFound(object sender, SolutionFoundEventArgs e)
        {
            var id = ObservableSolutions.Count + 1;
            var sol = new Solution(e.Solution, id);

            Application
                .Current
                .Dispatcher
                .BeginInvoke(DispatcherPriority.Background, new Action(() => ObservableSolutions.Add(sol)));

            SelectedSolution = sol;
        }

        private void ExtractCorrectNoOfSols()
        {
            var sols = SimulationResults
                        .Solutions
                        .Take(Utility.MaxNoOfSolutionsInOutput)
                        .ToList();

            sols.ForEach(s => ObservableSolutions.Add(s));

            // In case of activated visualization, clear all solutions before adding a no. of MaxNoOfSolutionsInOutput to the solutions.
            if (DisplayMode == DisplayMode.Visualize)
            {
                ObservableSolutions.Clear();

                sols
                .ForEach(sol => ObservableSolutions.Add(sol));
            }
        }

        private async void SimulateAsync()
        {
            UpdateProgressIndicator(SimulationStatus.Started);

            UpdateGui();
            SimulationResults = await Solver
                                .GetSimulationResultsAsync(BoardSize, SolutionMode, DisplayMode);

            ProgressVisibility = Visibility.Hidden;
            ExtractCorrectNoOfSols();
            NoOfSolutions = $"{SimulationResults.NoOfSolutions,0:N0}";
            ElapsedTimeInSec = $"{SimulationResults.ElapsedTimeInSec,0:N1}";
            SelectedSolution = ObservableSolutions.FirstOrDefault();

            UpdateProgressIndicator(SimulationStatus.Finished);
        }

        private bool CanSimulate() => IsValid && IsIdle;

        private void Cancel() => Solver.CancelSolver = true;

        private bool CanCancel() => IsRunning;

        private void Save()
        {
            var results = new ResultPresentation(SimulationResults);
            var filePath = results.Write2File(SolutionMode);
            var msg = $"Successfully wrote results to: {filePath}";
            MessageBox.Show(msg);
            IsIdle = true;
        }

        private bool CanSave() => IsIdle && IsOutputReady;
        #endregion PrivateMethods

        #region PrivateFields
        private double _progressValue;
        private string _progressLabel;
        private Visibility _progressVisibility;
        private Visibility _busyIndicatorVisibility;
        private IEnumerable<SolutionMode> _enumSolutionModes;
        private IEnumerable<DisplayMode> _enumDisplayModes;
        private InputViewModel _validation;
        private static ISimulationResults _simulationResults;
        private int _delayInMilliseconds;
        private string _noOfSoltions;
        private string _elapsedTime;
        private SolutionMode _solutionMode;
        private DisplayMode _displayMode;
        private string _boardSizeText;
        private sbyte _boardSize;
        private bool _isVisualized;
        private bool _isValid;
        private bool _isSingleRunning;
        private bool _isIdle;
        private bool _isRunning;
        private bool _isInInputMode;
        private bool _isOutputReady;
        private ISolver _solver;
        private Solution _selectedSolution;
        private string _solutionTitle;
        #endregion PrivateFields
    }
}