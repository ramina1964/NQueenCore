using NQueen.Common;
using NQueen.GUI.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NQueen.GUI.View
{
	public class ChessboardGrid : Grid
	{
		public ChessboardGrid(int size) => Size = size;

		public int WindowHeight => 500;
		public int WindowWidth => 500;
		public int Size { get; set; }
		public int Column { get; set; }
		public int Row { get; set; }
		public int WidthBorder => (WindowWidth - 50) / Size;
		public int HeightBorder => (WindowHeight - 50) / Size;

		// Test of dynamic grid written in code - no Xaml
		public void CreateGrid()
		{
			var width = new GridLength(WidthBorder);
			var height = new GridLength(HeightBorder);
			var grid = new Grid() { Height = WindowHeight, Width = WindowHeight };
			for (var i = 0; i < Size; i++)
			{
				var column = new ColumnDefinition { Width = width, Tag = i };
				var row = new RowDefinition { Height = height, Tag = i };
				grid.ColumnDefinitions.Add(column);
				grid.RowDefinitions.Add(row);
				for (var j = 0; j < Size; j++)
				{
					var color = new SolidColorBrush(Colors.Wheat);
					var pos = new Position(i, j);
					var sq = new SquareViewModel(pos, color);
					var border = new Border
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