using Source.MapGeneration;
using Source.Tools.Math;

namespace Source.Input
{
	public class RandomInput : IInputHandler
	{
		public Vector2 CurrentPosition { get; private set; } = Vector2.Zero;

		public bool IsConfirmed { get; private set; } = true;

		private Map _map;

		private Random _random = new();

		public RandomInput(Map map, Vector2 startedPosition)
		{
			CurrentPosition = startedPosition;

			_map = map;
		}

		public void UpdateInput()
		{
			List<Vector2> availableCells = new();

			for (int y = 0; y < _map.Size; y++)
			{
				for (int x = 0; x < _map.Size; x++)
				{
					if (_map.IsValidMove(x, y))
					{
						availableCells.Add(new Vector2(x, y));
					}
				}
			}

			if (availableCells.Count > 0)
			{
				CurrentPosition = availableCells[_random.Next(availableCells.Count)];

				IsConfirmed = true;
			}
		}
	}
}