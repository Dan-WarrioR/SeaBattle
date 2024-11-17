using Source.Input;
using Source.MapGeneration;
using Source.Rendering;
using Source.Tools.Math;

namespace Source
{
	public class Game
	{
		private enum PlayerMove
		{
			Player,
			Enemy,
		}
		
		private const int MapSize = 10;
		
		private Map _playerMap;
		private Map _enemyMap;

		private PlayerMove _currentPlayerMove = PlayerMove.Player;

		private Renderer _renderer;

		private IInputHandler _playerInput;
		private IInputHandler _enemyInput;

		public Game()
		{
			_playerMap = new(MapSize);
			_playerMap.OnCellBombed += OnBombedCell;

			_enemyMap = new(MapSize);
			_enemyMap.OnCellBombed += OnBombedCell;

			_renderer = new(_enemyMap, _playerMap);
			_playerInput = new PlayerInput(_playerMap, Vector2.Zero);
			_enemyInput = new RandomInput(_enemyMap, Vector2.Zero);
		}

		//////////

		public void Start()
		{
			InitializeConsoleSettings();

			PlayGameCycle();
		}

		private void InitializeConsoleSettings()
		{
			Console.OutputEncoding = System.Text.Encoding.UTF8;
			Console.CursorVisible = false;
		}

		private void PlayGameCycle()
		{
			do
			{
				Draw();

				while (!IsEndGame())
				{
					ProcessTurn();

					Draw();
				}

				DrawEndGameText();		
			}
			while (Console.ReadKey(true).Key == ConsoleKey.Y);
		}	

		private void ProcessTurn()
		{
			(IInputHandler input, Map map) = _currentPlayerMove == PlayerMove.Player ? (_playerInput, _enemyMap) : (_enemyInput, _playerMap);

			input.UpdateInput();

			if (!input.IsConfirmed)
			{
				return;
			}

			if (!map.TryBombCell(input.CurrentPosition))
			{
				SwitchTurn();
			}
		}

		private bool IsEndGame()
		{
			return _enemyMap.ShipsCount <= 0 || _playerMap.ShipsCount <= 0;
		}

		private void SwitchTurn()
		{
			_currentPlayerMove = _currentPlayerMove == PlayerMove.Player ? PlayerMove.Enemy : PlayerMove.Player;
		}

		private void OnBombedCell(CellState state)
		{
			if (state != CellState.Hit)
			{
				SwitchTurn();
			}
		}

		private void Draw()
		{
			_renderer.Draw(_playerInput.CurrentPosition);
		}

		private void DrawEndGameText()
		{
			_renderer.DrawEndGameText(_playerMap.ShipsCount > 0);
		}
	}
}
