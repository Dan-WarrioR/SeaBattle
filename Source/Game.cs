using Source.MapGeneration;
using Source.Rendering;
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

		private bool _onePlayerLostAllShips = false;
		private bool _onShipBombed = false;

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

				while (!IsEndGame())
				{
					CalculateInput();

					ProcessTurn();

					Draw();

					ResetTemporaryChanges();
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
			_onePlayerLostAllShips = false;

			Map enemyMap = new(MapSize);

			_player = new(false, enemyMap);
			_player.Map.OnCellBombed += OnBombedCell;

			Map playerMap = new(MapSize);

			_enemy = new(true, playerMap);
			_enemy.Map.OnCellBombed += OnBombedCell;

			_renderer = new(_player.Map, _enemy.Map);

			_currentPlayer = _player;
		}	

		private void CalculateInput()
		{
			_currentPlayer.CalculateInput();
		}

		private void ResetTemporaryChanges()
		{
			_currentPlayer.ResetAbilities();
		}

		private void ProcessTurn()
		{
			ApplyAbility();

			if (!_currentPlayer.IsConfirmed)
			{
				return;
			}

			_currentPlayer.TryBombCell();

			if (!_onShipBombed)
			{
				SwitchTurn();

				_onShipBombed = false;
			}
		}

		private void ApplyAbility()
		{
			var ability = _currentPlayer.GetSelectedAbility();

			if (ability != null)
			{
				ability.Apply(_currentPlayer.CurrentPosition);
			}
		}

		private void SwitchTurn()
		{
			_currentPlayer = _currentPlayer == _player ? _enemy : _player;
		}

		private void OnBombedCell(Cell cell)
		{
			_onShipBombed = cell.IsShip;	

			_onePlayerLostAllShips = _player.ShipsCount <= 0 || _enemy.ShipsCount <= 0;
		}



		private bool WantPlayerRepeat()
		{
			return Console.ReadKey(true).Key == ConsoleKey.Y;
		}

		private bool IsEndGame()
		{
			return _onePlayerLostAllShips;
		}



		private void Draw()
		{
			_renderer.Draw(_player.CurrentPosition);
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
