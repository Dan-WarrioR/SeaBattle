using Source.Tools.Math;

namespace Source.MapGeneration
{
	public interface IReadOnlyMap
	{
		public int Size { get; }

		public int ShipsCount { get; }

		public event Action<Cell> OnCellBombed;

		public bool IsInBorders(int x, int y);

		public bool IsValidMove(int x, int y);

		public bool TryGetCell(int x, int y, out Cell cell);

		public List<Vector2> GetAllAvailablesMoves();
	}
}
