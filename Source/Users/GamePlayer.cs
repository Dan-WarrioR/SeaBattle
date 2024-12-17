using Source.Abilities;
using Source.Configs;
using Source.MapGeneration;
using Source.Tools.Math;

namespace Source.Users
{
	public class GamePlayer
	{
		public Vector2 CurrentPosition { get; private set; }

		public bool IsConfirmed { get; private set; } = true;

		public int ShipsCount => Map.ShipsCount;

		public bool IsAi { get; }

		public Difficulty AiDifficulty { get; }

		public Map Map { get; private set; }

		private List<BaseAbility> _abilities = new();
		private int _selectedAbilityIndex = -1;

		private List<Vector2> _validMoves = new();
		private List<Vector2> _shipCells = new();

		private Random _random = new();	

		public GamePlayer(bool isAi, Difficulty aiDifficulty)
		{
			IsAi = isAi;
			AiDifficulty = aiDifficulty;
			Map = new(Map.MapSize);

			_validMoves = Map.GetAllAvailablesMoves();
			_shipCells = GetShipCells();

			_abilities.Add(new RadarAbility(Map));
		}

		public void CalculateInput()
		{
			IsConfirmed = false;

			if (IsAi)
			{
				CalculateAiInput();

				return;
			}

			CalculateNativeInput();
		}

		public void TryBombCell()
		{
			Map.BombCell(CurrentPosition);
		}

		public BaseAbility GetSelectedAbility()
		{
			if (_selectedAbilityIndex < 0 || _selectedAbilityIndex >= _abilities.Count)
			{
				return null;
			}			

			return _abilities[_selectedAbilityIndex];
		}

		public void ResetCurrentAbility()
		{
			_selectedAbilityIndex = -1;
		}

		private void CalculateNativeInput()
		{
			var key = Console.ReadKey(true).Key;

			if (key == ConsoleKey.Enter && IsValidMove(CurrentPosition))
			{
				IsConfirmed = true;

				_validMoves.Remove(CurrentPosition);

				return;
			}

			if (key >= ConsoleKey.D1 && key <= ConsoleKey.D9)
			{
				_selectedAbilityIndex = key - ConsoleKey.D1;

				_selectedAbilityIndex = Math.Clamp(_selectedAbilityIndex, 0, _abilities.Count - 1);

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

			Vector2 newPosition = new(CurrentPosition.X + delta.X, CurrentPosition.Y + delta.Y);

			if (Map.IsInBorders(newPosition.X, newPosition.Y))
			{
				CurrentPosition = newPosition;
			}
		}

		private void CalculateAiInput()
		{
			if (AiDifficulty > Difficulty.Easy && _random.Next(100) <= DifficultyConfig.GetChance(AiDifficulty))
			{
				CurrentPosition = ChooseRandomCellFromList(_shipCells);
			}
			else
			{
				CurrentPosition = ChooseRandomCellFromList(_validMoves);
			}

			Thread.Sleep(500);

			IsConfirmed = true;
		}

		private Vector2 ChooseRandomCellFromList(List<Vector2> cellList)
		{
			int randomIndex = _random.Next(cellList.Count);
			var selectedCell = cellList[randomIndex];

			cellList.RemoveAt(randomIndex);

			return selectedCell;
		}

		private bool IsValidMove(Vector2 point)
		{
			return _validMoves.Contains(point);
		}

		private List<Vector2> GetShipCells()
		{
			List<Vector2> shipCells = new(Map.ShipsCount);

			foreach (var move in _validMoves)
			{
				if (Map.TryGetCell(move.X, move.Y, out Cell cell) && cell.IsShip)
				{
					shipCells.Add(move);
				}
			}

			return shipCells;
		}
	}
}