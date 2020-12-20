namespace NQueen.Common
{
    public class Position
    {
        public Position(sbyte rowNo, sbyte columnNo)
        {
            RowNo = rowNo;
            ColumnNo = columnNo;
        }

        public sbyte RowNo { get; set; }

        public sbyte ColumnNo { get; set; }
    }
}