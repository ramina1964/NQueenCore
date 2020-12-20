
using GalaSoft.MvvmLight;
using NQueen.Common;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace NQueen.GUI.ViewModel
{
    public class Chessboard : ViewModelBase
    {
        #region Constructor
        public Chessboard()
        {
            Squares = new ObservableCollection<SquareViewModel>();
            QueenImagePath = @"..\..\Images\WhiteQueen.png";
        }
        #endregion Constructor

        #region PublicProperties
        public string QueenImagePath { get; }
        public ObservableCollection<SquareViewModel> Squares { get; set; }
        public double WindowWidth { get; set; }
        public double WindowHeight { get; set; }
        #endregion PublicProperties

        #region PublicMethods
        public void PlaceQueens(IEnumerable<Position> positions)
        {
            // Clear board
            ClearImages();

            // Place queens
            positions
                .ToList()
                .ForEach(pos =>
                    Squares.First(sq => pos.RowNo == sq.Position.RowNo && pos.ColumnNo == sq.Position.ColumnNo)
                    .ImagePath = QueenImagePath);
        }

        public void CreateSquares(sbyte boardSize, IEnumerable<SquareViewModel> squares)
        {
            var width = (int)WindowWidth / boardSize;
            var height = width;

            var sqList = squares.ToList();
            for (sbyte i = 0; i < boardSize; i++)
            {
                for (sbyte j = 0; j < boardSize; j++)
                {
                    var pos = new Position(i, j);
                    var square = new SquareViewModel(pos, FindColor(pos))
                    {
                        ImagePath = null,
                        Height = height,
                        Width = width,
                    };

                    sqList.Add(square);
                }
            }

            sqList
                .OrderByDescending(sq => sq.Position.ColumnNo)
                .ThenBy(sq => sq.Position.RowNo).ToList()
                .ForEach(sq => Squares.Add(sq));
        }
        #endregion PublicMethods

        #region PrivateMethods
        private void ClearImages() =>
            Squares
                .ToList()
                .ForEach(sq => sq.ImagePath = null);

        private Brush FindColor(Position pos)
        {
            var col = (pos.RowNo + pos.ColumnNo) % 2 == 1 ? Colors.Wheat : Colors.Brown;
            return new SolidColorBrush(col);
        }
        #endregion PrivateMethods
    }
}