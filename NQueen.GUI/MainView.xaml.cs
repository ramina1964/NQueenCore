using NQueen.GUI.ViewModel;

namespace NQueen.GUI
{
	public partial class MainView
	{
		public MainView(MainViewModel mainViewModel)
		{
			InitializeComponent();
			Loaded += MainView_Loaded;
			MainViewModel = mainViewModel;
		}

		public MainViewModel MainViewModel { get; set; }

		private void MainView_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			var view = e.OriginalSource as MainView;
			var board = chessboard;
			var size = (int)System.Math.Min(board.ActualWidth, board.ActualHeight);
			board.Width = size;
			board.Height = size;
			MainViewModel.SolverViewModel.SetChessboard(size);
			DataContext = MainViewModel.SolverViewModel;
		}
	}
}