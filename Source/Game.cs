﻿using Source.MapGeneration;
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
		private int _playerScore = 0;

		private Player _enemy;
		private int _enemyScore = 0;

		private Player _currentPlayer;

		private bool _onePlayerLostAllShips = false;
		private bool _isShipBombed = false;

		private List<BaseAbility> _activeAbilities = new();

		public Game(GameMode gameMode)
		{
			_currentGameMode = gameMode;
		}

		public void PlayGame()
		{
			while (!IsGameEnd())
			{
				ResetGame();
				PlayGameCycle();
				DrawRoundScore();
			}
		}

		public void PlayGameCycle()
		{
			Draw();

			while (!IsRoundEnd())
			{
				CalculateInput();
				ProcessTurn();
				Draw();
			}
		}

		public void ResetGame()
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

			UpdateScore();

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

			_activeAbilities.Clear();
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

		private void UpdateScore()
		{
			if (_currentPlayer.ShipsCount > 0)
			{
				return;
			}

			if (_currentPlayer == _player)
			{
				_playerScore++;

				return;
			}

			_enemyScore++;
		}



		public bool IsGameEnd()
		{
			return _playerScore >= 3 || _enemyScore >= 3;
		}

		private bool IsRoundEnd()
		{
			return _onePlayerLostAllShips;
		}



		public void DrawRoundScore()
		{
			Console.Clear();

			Console.WriteLine($"\tCurrent Score: {_playerScore} | {_enemyScore}");

			Thread.Sleep(5000);
		}

		private void Draw()
		{
			_renderer.Draw();
		}	
	}
}