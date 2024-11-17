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

		private PlayerMove _curentPlayerMove = PlayerMove.Player;

		private Renderer _renderer;

		private PlayerInput _playerInput;
		private PlayerInput _enemyInput;

		public Game()
		{
			_playerMap = new(MapSize);
			_playerMap.OnCellBombed += OnEnemyBombedCell;

			_enemyMap = new(MapSize);
			_enemyMap.OnCellBombed += OnPlayerBombedCell;

			_renderer = new(_enemyMap, _playerMap);
			_playerInput = new(_playerMap, Vector2.Zero);
			_enemyInput = new(_enemyMap, Vector2.Zero);
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
					CalculateInput();

					DoLogic();

					Draw();
				}

				DrawEndGameText();		
			}
			while (Console.ReadKey(true).Key == ConsoleKey.Y);
		}	

		private void CalculateInput()
		{
			if (_curentPlayerMove == PlayerMove.Player)
			{
				_playerInput.UpdatePlayerInput();
			}
			else
			{
				_enemyInput.UpdatePlayerInput(true);
			}
		}

		private void DoLogic()
		{
			if (_curentPlayerMove == PlayerMove.Player)
			{
				if (!_playerInput.IsConfirmed)
				{
					return;
				}

				if (_enemyMap.IsValidMove(_playerInput.CurrentPosition.X, _playerInput.CurrentPosition.Y) && !_enemyMap.TryBombCell(_playerInput.CurrentPosition))
				{
					_curentPlayerMove = PlayerMove.Enemy;
				}
			}
			else
			{
				if (!_playerMap.TryBombCell(_enemyInput.CurrentPosition))
				{
					_curentPlayerMove = PlayerMove.Player;
				}
			}
		}

		private bool IsEndGame()
		{
			return _enemyMap.ShipsCount <= 0 || _playerMap.ShipsCount <= 0;
		}

		private void OnPlayerBombedCell(CellState state)
		{
			if (state == CellState.Miss)
			{
				_curentPlayerMove = PlayerMove.Enemy;
			}
		}

		private void OnEnemyBombedCell(CellState state)
		{
			if (state == CellState.Miss)
			{
				_curentPlayerMove = PlayerMove.Player;
			}
		}

		//////////

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
