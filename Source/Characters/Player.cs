using Source.Abilities;
using Source.MapGeneration;
using Source.Tools.Math;

namespace Source.Characters
{
	public class Player
	{
		public Vector2 CurrentPosition { get; private set; }

		public bool IsConfirmed { get; private set; } = true;

		public int ShipsCount => Map.ShipsCount;

		public bool IsAi { get; }

		public Map Map { get; private set; }

		private List<IAbility> _abilities = new();
		private int _selectedAbilityIndex = -1;

		private List<Vector2> _validMoves = new();

		private Random _random = new();	

		public Player(bool isAi, Map map)
		{
			IsAi = isAi;
			Map = map;

			_validMoves = Map.GetAllAvailablesMoves();

			_abilities.Add(new RadarAbility(map));
		}

		public void CalculateInput()
		{
			IsConfirmed = false;

			if (IsAi)
			{
				CalculateRandomPosition();

				return;
			}

			CalculateNativeInput();
		}

		public void TryBombCell()
		{
			Map.BombCell(CurrentPosition);
		}

		public IAbility GetSelectedAbility()
		{
			if (_selectedAbilityIndex < 0 || _selectedAbilityIndex >= _abilities.Count)
				return null;

			return _abilities[_selectedAbilityIndex];
		}

		public void ResetAbilities()
		{
			_selectedAbilityIndex = -1;

			foreach (var ability in _abilities)
			{
				ability.Reset();
			}
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

		private void CalculateRandomPosition()
		{
			if (_validMoves.Count > 0)
			{
				int randomIndex = _random.Next(_validMoves.Count);

				CurrentPosition = _validMoves[randomIndex];

				_validMoves.RemoveAt(randomIndex);

				IsConfirmed = true;
			}
		}

		private bool IsValidMove(Vector2 point)
		{
			return _validMoves.Contains(point);
		}
	}
}
