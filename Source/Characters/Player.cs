using Source.Input;
using Source.MapGeneration;

namespace Source.Characters
{
	public class Player
	{
		public int ShipsCount => _map.ShipsCount;

		public IReadOnlyMap Map => _map;

		public IInputHandler Input => _inputHandler;

		private Map _map;

		private IInputHandler _inputHandler;

		public Player(Map map, IInputHandler inputHandler)
		{
			_map = map;
			_inputHandler = inputHandler;
		}

		public void UpdateInput()
		{
			_inputHandler.UpdateInput();
		}

		public bool TryBombCell()
		{
			return _map.TryBombCell(_inputHandler.CurrentPosition);
		}
	}
}
