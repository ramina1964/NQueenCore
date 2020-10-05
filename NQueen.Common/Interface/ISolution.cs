namespace NQueen.Common.Interface
{
	public interface ISolution
	{
		string Details { get; set; }
		int Id { get; }
		string Name { get; set; }
		int[] QueenList { get; }

		string ToString();
	}
}