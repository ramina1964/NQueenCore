using NQueen.Common;
using NQueen.GUI.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NQueen.GUI.View
{
    public class ChessboardGrid : Grid
    {
        public ChessboardGrid(sbyte size) => Size = size;

        public int WindowHeight => 500;

        public int WindowWidth => 500;

        public sbyte Size { get; set; }

        public sbyte Column { get; set; }

        public sbyte Row { get; set; }

        public int WidthBorder => (WindowWidth - 50) / Size;

        public int HeightBorder => (WindowHeight - 50) / Size;

        // Test of dynamic grid written in code - no Xaml
        public void CreateGrid()
        {
            GridLength width = new GridLength(WidthBorder);
            GridLength height = new GridLength(HeightBorder);
            Grid grid = new Grid() { Height = WindowHeight, Width = WindowHeight };
            for (sbyte i = 0; i < Size; i++)
            {
                ColumnDefinition column = new ColumnDefinition { Width = width, Tag = i };
                RowDefinition row = new RowDefinition { Height = height, Tag = i };
                grid.ColumnDefinitions.Add(column);
                grid.RowDefinitions.Add(row);
                for (sbyte j = 0; j < Size; j++)
                {
                    SolidColorBrush color = new SolidColorBrush(Colors.Wheat);
                    Position pos = new Position(i, j);
                    SquareViewModel sq = new SquareViewModel(pos, color);
                    Border border = new Border
                    {
                        Background = color,
                        Height = HeightBorder,
                        Width = WidthBorder,
                        DataContext = sq
                    };

                    SetColumn(border, j);
                    SetRow(border, i);
                }
            }
        }
    }
}