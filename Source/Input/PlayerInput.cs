using Source.Tools.Math;
using Source.MapGeneration;

namespace Source.Input
{
    public class PlayerInput
	{
		public Vector2 CurrentPosition;

		public bool IsConfirmed { get; private set; } = false;

		private Map _map;

		private Random _random = new();

		public PlayerInput(Map map, Vector2 startedPosition)
		{
			CurrentPosition = startedPosition;
			
			_map = map;
		}

		public void UpdatePlayerInput(bool isAuto = false)
		{
			IsConfirmed = false;

			if (!isAuto)
			{
				CalculateNativeInput();
			}
			else
			{
				CalculateAutoInput();
			}		
		}

		private void CalculateNativeInput()
		{
			var key = Console.ReadKey(true).Key;

			if (key == ConsoleKey.Enter)
			{
				IsConfirmed = true;

				return;
			}

			var delta = key switch
			{
				ConsoleKey.UpArrow or ConsoleKey.W => new(0, -1),
				ConsoleKey.DownArrow or ConsoleKey.S => new(0, 1),
				ConsoleKey.LeftArrow or ConsoleKey.A => new(-1, 0),
				ConsoleKey.RightArrow or ConsoleKey.D => new(1, 0),
				_ => Vector2.Zero,
			};

			if (_map.IsInBorders(CurrentPosition.X + delta.X, CurrentPosition.Y + delta.Y))
			{
				CurrentPosition.X += delta.X;
				CurrentPosition.Y += delta.Y;
			}
		}

		private void CalculateAutoInput()
		{
			List<Vector2> freeCells = new();

			for (int i = 0; i < _map.Size; i++)
			{
				for (int j = 0; j < _map.Size; j++)
				{
					if (_map.IsValidMove(j, i))
					{
						freeCells.Add(new(j, i));
					}
				}
			}

			CurrentPosition = freeCells[_random.Next(0, freeCells.Count)];

			IsConfirmed = true;
		}
	}
}