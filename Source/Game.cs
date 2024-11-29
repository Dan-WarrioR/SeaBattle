using Source.MapGeneration;
using Source.Rendering;
using Source.Characters;
using Source.Abilities;

namespace Source
{
	public enum GameMode
	{
		PVE,
		EVE,
		PVP,
	}

	public class Game
	{		
		private const int MapSize = 10;

		private GameMode _currentGameMode;

		private Renderer _renderer;

		private Player _player;
		private Player _enemy;

		private Player _currentPlayer;

		private bool _onePlayerLostAllShips = false;
		private bool _isShipBombed = false;

		private List<BaseAbility> _activeAbilities = new();

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

			SelectGameMode();

			InitializeGame();
		}

		private void InitializeGame()
		{
			_onePlayerLostAllShips = false;

			Map enemyMap = new(MapSize);
			Map playerMap = new(MapSize);

			(Player player, Player enemy) = _currentGameMode switch
			{
				GameMode.PVP => (new Player(false, enemyMap), new Player(false, playerMap)),
				GameMode.PVE => (new Player(false, enemyMap), new Player(true, playerMap)),
				GameMode.EVE => (new Player(true, enemyMap), new Player(true, playerMap)),
			};

			_player = player;
			_enemy = enemy;

			_player.Map.OnCellBombed += OnBombedCell;
			_enemy.Map.OnCellBombed += OnBombedCell;

			_renderer = new(_currentGameMode, _player, _enemy);

			_currentPlayer = _player;
		}

		private void SelectGameMode()
		{
			DrawAvailableModes();

			_currentGameMode = GetGameMode();
		}

		private GameMode GetGameMode()
		{
			while (true)
			{
				var key = Console.ReadKey(true).Key;

				int keyAsNumber = key - ConsoleKey.D1;

				if (Enum.IsDefined(typeof(GameMode), keyAsNumber))
				{
					return (GameMode)keyAsNumber;
				}
			}
		}

		private void CalculateInput()
		{
			_currentPlayer.CalculateInput();
		}

		private void ProcessTurn()
		{
			ResetPreviousActiveAbilities();

			ProcessAbility();

			if (!_currentPlayer.IsConfirmed)
			{
				return;
			}

			_currentPlayer.TryBombCell();

			if (!_isShipBombed)
			{
				SwitchTurn();

				_isShipBombed = false;
			}

			_currentPlayer.ResetCurrentAbility();
		}

		private void ProcessAbility()
		{
			var ability = _currentPlayer.GetSelectedAbility();

			if (ability != null)
			{
				ability.Apply(_currentPlayer.CurrentPosition);

				_activeAbilities.Add(ability);
			}
		}

		private void ResetPreviousActiveAbilities()
		{
			foreach (var ability in _activeAbilities)
			{
				ability.Reset();
			}
		}

		private void SwitchTurn()
		{
			_currentPlayer = _currentPlayer == _player ? _enemy : _player;
		}

		private void OnBombedCell(Cell cell)
		{
			_isShipBombed = cell.IsShip;	

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



		private void DrawAvailableModes()
		{
			Console.Clear();

			Console.WriteLine(
				"==================================================" +
				"\nChoose game mode: " +
				"\n 1 - Player vs Enemy (PVE)" +
				"\n 2 - Enemy vs Enemy (EVE)" +
				"\n 3 - Player vs Player (PVP)" +
				"\n==================================================");
		}

		private void Draw()
		{
			_renderer.Draw();
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
