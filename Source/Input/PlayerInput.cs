using Source.Tools.Math;

namespace Source.Input
{
    public class PlayerInput : IInputHandler
	{
		public Vector2? CurrentPosition { get; private set; }

		public Vector2 FuturePosition { get; private set; }

		private Random _random = new();

		private Border _border;

		public PlayerInput(Border border, Vector2 startedPosition)
		{
			FuturePosition = startedPosition;
			
			_border = border;
		}

		public void UpdateInput()
		{
			CalculateNativeInput();		
		}

		private void CalculateNativeInput()
		{
			var key = Console.ReadKey(true).Key;

			if (key == ConsoleKey.Enter)
			{
				CurrentPosition = FuturePosition;

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

			if (IsInBorders(FuturePosition.X + delta.X, FuturePosition.Y + delta.Y))
			{
				FuturePosition += (delta.X, delta.Y);
			}
		}

		public void ResetInput()
		{
			if (CurrentPosition.HasValue)
			{
				FuturePosition = CurrentPosition.Value;
			}		

			CurrentPosition = null;
		}

		private bool IsInBorders(int x, int y)
		{
			return x >= _border.X && x < _border.Y && y >= _border.Z && y < _border.W;
		}
	}
}