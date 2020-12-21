using GalaSoft.MvvmLight;
using NQueen.Common.Interface;

namespace NQueen.GUI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(ISolver solver) => SolverViewModel = new SolverViewModel(solver);

        public SolverViewModel SolverViewModel { get; }
    }
}