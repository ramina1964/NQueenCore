namespace NQueen.Common.Interface
{
    public interface ISolution
    {
        string Details { get; }

        int Id { get; }

        string Name { get; }

        int[] QueenList { get; }

        string ToString();
    }
}