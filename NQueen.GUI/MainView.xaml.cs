using NQueen.GUI.ViewModel;
using System;

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
            var board = chessboard;
            var size = (int) Math.Min(board.ActualWidth, board.ActualHeight);
            board.Width = size;
            board.Height = size;
            MainViewModel.SetChessboard(size);
            DataContext = MainViewModel;
        }
    }
}