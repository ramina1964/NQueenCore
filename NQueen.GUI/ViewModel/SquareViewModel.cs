using GalaSoft.MvvmLight;
using NQueen.Common;
using System.Windows.Media;

namespace NQueen.GUI.ViewModel
{
    public class SquareViewModel : ViewModelBase
    {
        #region Constructor
        public SquareViewModel(Position pos, Brush color)
        {
            Color = color;
            Position = pos;
        }
        #endregion Constructor

        #region PublicProperties
        public Brush Color { get; set; }
        public Position Position { get; set; }

        public double Width
        {
            get => _width;
            set => Set(ref _width, value);
        }

        public double Height
        {
            get => _height;
            set => Set(ref _height, value);
        }

        public string ImagePath
        {
            get => _imagePath;
            set => Set(ref _imagePath, value);
        }

        public override string ToString() => $"{Position.RowNo}, {Position.ColumnNo}";
        #endregion PublicProperties

        #region PrivateFields
        private double _width;
        private double _height;
        private string _imagePath;
        #endregion PrivateFields
    }
}