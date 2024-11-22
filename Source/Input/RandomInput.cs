using Source.Tools.Math;

namespace Source.Input
{
	public class RandomInput : IInputHandler
	{
		public Vector2? CurrentPosition { get; private set; }

		public Vector2 FuturePosition { get; private set; }

		private List<Vector2> _validMoves = new(); 

		private Random _random = new();

		public RandomInput(List<Vector2> validMoves, Vector2 startedPosition)
		{
			CurrentPosition = startedPosition;

			_validMoves = validMoves;
		}

		public void UpdateInput()
		{
			if (_validMoves.Count > 0)
			{
				int randomIndex = _random.Next(_validMoves.Count);

				CurrentPosition = _validMoves[randomIndex];

				FuturePosition = CurrentPosition.Value;

				_validMoves.RemoveAt(randomIndex);
			}
		}

		public void ResetInput()
		{
			CurrentPosition = null;
		}
	}
}