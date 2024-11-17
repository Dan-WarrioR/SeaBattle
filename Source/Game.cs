namespace Source
{
	public class Game
	{
		//Configs
		private const ConsoleColor PlayerCursorColor = ConsoleColor.Green;
		private const ConsoleColor DestroyedShipColor = ConsoleColor.Red;
		private const ConsoleColor MissShotColor = ConsoleColor.Gray;
		private const ConsoleColor SeaColor = ConsoleColor.Gray;
		private const ConsoleColor ShipColor = ConsoleColor.DarkBlue;

		private const int MapSize = 10;
		private const int SpaceBetweenMaps = 15;

		private const char PlayerCursorIcon = 'o';
		private const char SeaIcon = '.';
		private const char ShipIcon = '⌂';
		private const char DestroyedShipIcon = 'X';
		private const char MissShotIcon = '#';

		private const int ShipFrequency = 10;



		private char[,] _playerMap = new char[MapSize, MapSize];
		private char[,] _enemyMap = new char[MapSize, MapSize];

		private int _playerShipsCount = 0;
		private int _enemyShipsCount = 0;

		private Random _random = new();

		private (int x, int y) _playerPosition = (0, 0);

		private bool _isMoveApproved = false;

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
				GenerateMaps();

				Draw();

				while (!IsEndGame())
				{
					CalculatePlayerInput();

					Logic();

					Draw();
				}

				DrawEndGameText();

				Console.WriteLine("Play again? (Y / N)");
			}
			while (Console.ReadKey(true).Key == ConsoleKey.Y);
		}

		//Player Input

		private void CalculatePlayerInput()
		{
			var key = Console.ReadKey(true).Key;

			GetPlayerMove(key);

			GetPlayerCommand(key);
		}

		private void GetPlayerMove(ConsoleKey key)
		{
			(int x, int y) delta = key switch
			{
				ConsoleKey.UpArrow or ConsoleKey.W => (0, -1),
				ConsoleKey.DownArrow or ConsoleKey.S => (0, 1),
				ConsoleKey.LeftArrow or ConsoleKey.A => (-1, 0),
				ConsoleKey.RightArrow or ConsoleKey.D => (1, 0),
				_ => (0, 0),
			};

			if (IsInBorders(_playerPosition.x + delta.x, _playerPosition.y + delta.y))
			{
				_playerPosition.x += delta.x;
				_playerPosition.y += delta.y;
			}
		}

		private void GetPlayerCommand(ConsoleKey key)
		{
			switch (key)
			{
				case ConsoleKey.Enter:
					_isMoveApproved = true;
					break;
				default:
					break;
			}
		}

		private bool IsInBorders(int x, int y)
		{
			return x >= 0 && x < MapSize && y >= 0 && y < MapSize;
		}

		//Map Generation
		private void GenerateMaps()
		{
			_playerMap = new char[MapSize, MapSize];
			_enemyMap = new char[MapSize, MapSize];

			for (int i = 0; i < MapSize; i++)
			{
				for (int j = 0; j < MapSize; j++)
				{
					_enemyMap[i, j] = GenerateCell(ref _enemyShipsCount);
					_playerMap[i, j] = GenerateCell(ref _playerShipsCount);
				}
			}
		}

		private char GenerateCell(ref int shipCount)
		{
			if (_random.Next(0, 100) < ShipFrequency)
			{
				shipCount++;

				return ShipIcon;
			}

			return SeaIcon;
		}

		//Game Logic

		private void Logic()
		{
			if (!CanDoLogic())
			{
				return;
			}

			TryPlayerShoot(_playerPosition.x, _playerPosition.y);

			TryEnemyShoot();
		}

		private void TryPlayerShoot(int x, int y)
		{
			if (!CanPlayerDoShoot(x, y))
			{
				return;
			}

			DoPlayerShoot(x, y);
		}

		private bool CanPlayerDoShoot(int x, int y)
		{
			if (!IsValidMove(x, y, _enemyMap))
			{
				return false;
			}

			if (_enemyMap[x, y] == DestroyedShipIcon)
			{
				return false;
			}

			return true;
		}

		private void DoPlayerShoot(int x, int y)
		{
			if (_enemyMap[x, y] == ShipIcon)
			{
				_enemyMap[x, y] = DestroyedShipIcon;

				_enemyShipsCount--;
			}
			else
			{
				_enemyMap[x, y] = MissShotIcon;
			}

			_isMoveApproved = false;
		}

		private void TryEnemyShoot()
		{
			if (!CanEnemyShoot())
			{
				return;
			}

			DoEnemyShoot();
		}

		private bool CanEnemyShoot()
		{
			return !_isMoveApproved;
		}

		private void DoEnemyShoot()
		{
			(int x, int y) = GetEnemyShootPosition();

			if (_playerMap[x, y] == ShipIcon)
			{
				_playerMap[x, y] = DestroyedShipIcon;

				_playerShipsCount--;
			}
			else
			{
				_playerMap[x, y] = MissShotIcon;
			}
		}

		private (int x, int y) GetEnemyShootPosition()
		{
			List<(int x, int y)> freeCells = new();

			for (int i = 0; i < MapSize; i++)
			{
				for (int j = 0; j < MapSize; j++)
				{
					if (IsValidMove(j, i, _playerMap))
					{
						freeCells.Add((j, i));
					}
				}
			}

			return freeCells[_random.Next(0, freeCells.Count)];
		}

		//Game Conditions Checkers

		private bool IsEndGame()
		{
			return _playerShipsCount <= 0 || _enemyShipsCount <= 0;
		}

		private bool CanDoLogic()
		{
			return _isMoveApproved;
		}

		private bool IsValidMove(int x, int y, char[,] map)
		{
			if (map[x, y] == DestroyedShipIcon || map[x, y] == MissShotIcon)
			{
				return false;
			}

			return true;
		}	


		//Frontend


		private void Draw()
		{
			DrawMaps();

			DrawPlayerCursor();

			DrawStats();
		}

		private void DrawMaps()
		{
			Console.Clear();

			for (int i = 0; i < MapSize; i++)
			{
				for (int j = 0; j < MapSize; j++)
				{
					DrawEnemyMapCell(j, i);

					DrawPlayerMapCell(j, i);
				}	

				Console.WriteLine();
			}
		}

		private void DrawEnemyMapCell(int x, int y)
		{
			char cellIcon = _enemyMap[x, y] == ShipIcon ? SeaIcon : _enemyMap[x, y];

			ConsoleColor consoleColor = _enemyMap[x, y] switch
			{
				MissShotIcon => MissShotColor,
				DestroyedShipIcon => DestroyedShipColor,
				_ => SeaColor,
			};

			WriteColoredSymbol(consoleColor, cellIcon);
		}

		private void DrawPlayerMapCell(int x, int y)
		{
			ConsoleColor consoleColor = _playerMap[x, y] switch
			{
				MissShotIcon => MissShotColor,
				DestroyedShipIcon => DestroyedShipColor,
				ShipIcon => ShipColor,
				_ => SeaColor,
			};

			WriteSymbolAtPlace(x + SpaceBetweenMaps + MapSize, y, consoleColor, _playerMap[x, y]);
		}

		private void DrawPlayerCursor()
		{
			WriteSymbolAtPlace(_playerPosition.x, _playerPosition.y, PlayerCursorColor, PlayerCursorIcon);
		}

		private void DrawStats()
		{
			Console.WriteLine($"\nEnemy ships - {_enemyShipsCount} | My ships - {_playerShipsCount}");
		}

		private void DrawEndGameText()
		{
			if (_playerShipsCount <= 0)
			{
				Console.WriteLine("You Lose!");
			}
			else if (_enemyShipsCount <= 0)
			{
				Console.WriteLine("You Won!");
			}
		}


		//Help Methods


		private void WriteSymbolAtPlace(int x, int y, char symbol)
		{
			WriteSymbolAtPlace(x, y, Console.ForegroundColor, symbol);
		}

		private void WriteSymbolAtPlace(int x, int y, ConsoleColor color, char symbol)
		{
			var cursorPosition = Console.GetCursorPosition();

			Console.SetCursorPosition(x, y);

			WriteColoredSymbol(color, symbol);

			Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);
		}

		private void WriteColoredSymbol(ConsoleColor color, char symbol)
		{
			var currentColor = Console.ForegroundColor;

			Console.ForegroundColor = color;
			Console.Write(symbol);
			Console.ForegroundColor = currentColor;
		}
	}
}
