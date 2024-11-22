using Source.Input;
using Source.MapGeneration;
using Source.Rendering;
using Source.Tools.Math;
using Source.Characters;

namespace Source
{
	public class Game
	{		
		private const int MapSize = 10;

		private Renderer _renderer;

		private Player _player;
		private Player _enemy;

		private Player _currentPlayer;

		private bool _isGameEnd = false;

		public void Start()
		{
			InitializeConsoleSettings();

			PlayGameCycle();
		}

		private void PlayGameCycle()
		{
			do
			{
				ResetGame();

				Draw();

				while (!_isGameEnd)
				{
					CalculateInput();

					ProcessTurn();

					Draw();
				}

				DrawEndGameText();
			}
			while (WantPlayerRepeat());
		}

		private void ResetGame()
		{
			if (_enemy != null)
			{
				_enemy.Map.OnCellBombed -= OnBombedCell;
			}

			if (_player != null)
			{
				_player.Map.OnCellBombed -= OnBombedCell;
			}

			InitializeGame();
		}

		private void InitializeGame()
		{
			_isGameEnd = false;

			_player = new(new(MapSize), new PlayerInput(new(0, MapSize, 0, MapSize), Vector2.Zero));
			_player.Map.OnCellBombed += OnBombedCell;

			Map playerMap = new(MapSize);

			_enemy = new(playerMap, new RandomInput(playerMap.GetAllAvailablesMoves(), Vector2.Zero));
			_enemy.Map.OnCellBombed += OnBombedCell;

			_renderer = new(_player.Map, _enemy.Map);

			_currentPlayer = _player;
		}	

		private void CalculateInput()
		{
			_currentPlayer.UpdateInput();
		}

		private void ProcessTurn()
		{
			_currentPlayer.TryBombCell();
		}

		private void SwitchTurn()
		{
			_currentPlayer = _currentPlayer == _player ? _enemy : _player;
		}

		private bool WantPlayerRepeat()
		{
			return Console.ReadKey(true).Key == ConsoleKey.Y;
		}

		private void OnBombedCell(Cell cell)
		{
			if (!cell.IsShip)
			{
				SwitchTurn();
			}

			_isGameEnd = _player.ShipsCount <= 0 || _enemy.ShipsCount <= 0;
		}

		private void Draw()
		{
			_renderer.Draw(_player.Input.FuturePosition);
		}

		private void DrawEndGameText()
		{
			_renderer.DrawEndGameText(_player.ShipsCount <= 0);
		}

		private void InitializeConsoleSettings()
		{
			Console.OutputEncoding = System.Text.Encoding.UTF8;
			Console.CursorVisible = false;
		}
	}
}
