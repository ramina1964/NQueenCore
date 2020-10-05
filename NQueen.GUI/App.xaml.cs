using NQueen.Common.Enum;
using NQueen.GUI.ViewModel;
using NQueen.Model;
using System.Windows;

namespace NQueen.GUI
{
    public partial class App
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Load();
            MainView = new MainView(MainViewModel);
            MainView.Show();
        }

        private void Load()
        {
            var boardSize = (sbyte)12;
            var solver = new Solver(boardSize, DisplayMode.Hide);
            MainViewModel = new MainViewModel(solver);
        }

        #region PrivateFields
        public MainView MainView { get; set; }
        public MainViewModel MainViewModel { get; set; }
        #endregion PrivateFields
    }
}