using Source.Tools.Math;
using Source.MapGeneration;

namespace Source.Input
{
    public class PlayerInput : IInputHandler
	{
		public Vector2 CurrentPosition { get; private set; }

		public bool IsConfirmed { get; private set; } = false;

		private Map _map;

		private Random _random = new();

		public PlayerInput(Map map, Vector2 startedPosition)
		{
			CurrentPosition = startedPosition;
			
			_map = map;
		}

		public void UpdateInput()
		{
			IsConfirmed = false;

			CalculateNativeInput();		
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
				CurrentPosition += (delta.X, delta.Y);
			}
		}
	}
}