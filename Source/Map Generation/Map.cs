using Source.Tools.Math;

namespace Source.MapGeneration
{
	public class Map
	{
		private const int ShipFrequency = 10;

		public int Size => _map.GetLength(0);

		public int ShipsCount { get; private set; } = 0;

		public event Action<CellState> OnCellBombed;

		private Cell[,] _map;

		private Random _random = new();

		public Map(int size)
		{
			_map = new Cell[size, size];

			FillMap();
		}

		public bool IsInBorders(int x, int y)
		{
			return x >= 0 && x < Size && y >= 0 && y < Size;
		}

		public bool IsValidMove(int x, int y)
		{
			if (!IsInBorders(x, y) || !_map[x, y].CanBombCell())
			{
				return false;
			}

			return true;
		}

		public bool TryBombCell(Vector2 position)
		{
			if (!IsValidMove(position.X, position.Y))
			{
				return false;
			}

			_map[position.X, position.Y].BombCell();

			return true;
		}

		public bool TryGetCell(int x, int y, out Cell cell)
		{
			if (!IsInBorders(x, y))
			{
				cell = null;

				return false;
			}

			cell = _map[x, y];

			return true;
		}

		private void FillMap()
		{
			for (int i = 0; i < Size; i++)
			{
				for (int j = 0; j < Size; j++)
				{
					Cell cell = new();

					GenerateCell(cell);

					cell.OnBombed += OnBombed;

					_map[i, j] = cell;
				}
			}
		}

		private void GenerateCell(Cell cell)
		{
			if (_random.Next(0, 100) < ShipFrequency)
			{
				ShipsCount++;

				cell.PlaceShip();
			}
		}

		private void OnBombed(CellState state)
		{
			if (state == CellState.Hit)
			{
				ShipsCount--;
			}

			OnCellBombed?.Invoke(state);
		}
	}
}