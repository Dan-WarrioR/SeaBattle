using Source.Characters;
using Source.Configs;
using Source.MapGeneration;
using Source.Tools.Math;

namespace Source.Rendering
{
	public class Renderer
	{
		private const int SpaceBetweenMaps = 5;

		private Player _firstPlayer;
		private Player _secondPlayer;

		private GameMode _gameMode;

		public Renderer(GameMode gameMode, Player firstPlayer, Player secondPlayer)
		{
			_gameMode = gameMode;

			_firstPlayer = firstPlayer;
			_secondPlayer = secondPlayer;
		}

		public void Draw()
		{
			Console.Clear();

			DrawMap(Vector2.Zero, _gameMode == GameMode.EVE, _firstPlayer.Map);
			DrawMap(new(_secondPlayer.Map.Size + SpaceBetweenMaps, 0), _gameMode != GameMode.PVE, _secondPlayer.Map);

			DrawPlayerCursor(_firstPlayer.CurrentPosition);

			Vector2 cursorPosition = new(_secondPlayer.CurrentPosition.X + _secondPlayer.Map.Size + SpaceBetweenMaps, _secondPlayer.CurrentPosition.Y);

			DrawPlayerCursor(cursorPosition);

			DrawStats();
		}

		public void DrawMap(Vector2 startPosition, bool isShipsVisible, Map map)
		{
			for (int i = 0; i < map.Size; i++)
			{
				for (int j = 0; j < map.Size; j++)
				{
					if (map.TryGetCell(j, i, out Cell cell))
					{
						var iconRenderer = GetCellRender(cell, isShipsVisible);

						WriteSymbolAtPlace(startPosition + (Vector2)(j, i), iconRenderer.color, iconRenderer.icon);
					}
				}
			}

			Console.SetCursorPosition(0, startPosition.Y + map.Size);
		}

		public void DrawPlayerCursor(Vector2 cursorPosition)
		{
			WriteSymbolAtPlace(cursorPosition, ColorsConfig.PlayerCursorColor, IconsConfig.PlayerCursorIcon);
		}

		public void DrawStats()
		{
			if (_gameMode == GameMode.PVE)
			{
				Console.WriteLine($"\nEnemy ships - {_firstPlayer.ShipsCount} \nMy ships - {_secondPlayer.ShipsCount}");

				return;
			}

			Console.WriteLine($"\nLeft map ships - {_firstPlayer.ShipsCount} \nRight map ships - {_secondPlayer.ShipsCount}");
		}

		public void DrawEndGameText(bool playerWin)
		{
			Console.Clear();

			Console.WriteLine(playerWin ? "You Won!" : "You Lose!");
			Console.WriteLine("Play again? (Y / N)");
		}

		private (ConsoleColor color, char icon) GetCellRender(Cell cell, bool isShipsVisible)
		{
			(ConsoleColor color, char icon) cellRenderer;

			cellRenderer = cell switch
			{
				Cell when !isShipsVisible && !cell.IsScaned && cell.IsShip && !cell.IsDestroyed => (ColorsConfig.SeaColor, IconsConfig.SeaIcon),
				Cell when cell.IsDestroyed && !cell.IsShip => (ColorsConfig.MissShotColor, IconsConfig.MissShotIcon),
				Cell when cell.IsDestroyed && cell.IsShip => (ColorsConfig.DestroyedShipColor, IconsConfig.DestroyedShipIcon),
				Cell when cell.IsShip => (ColorsConfig.ShipColor, IconsConfig.ShipIcon),
				_ => (ColorsConfig.SeaColor, IconsConfig.SeaIcon),
			};

			if (cell.IsScaned)
			{
				cellRenderer.color = ColorsConfig.ScannedAreaColor;
			}

			return cellRenderer;
		}

		private void WriteColoredSymbol(ConsoleColor color, char symbol)
		{
			var currentColor = Console.ForegroundColor;

			Console.ForegroundColor = color;
			Console.Write(symbol);
			Console.ForegroundColor = currentColor;
		}

		private void WriteSymbolAtPlace(Vector2 position, char symbol)
		{
			WriteSymbolAtPlace(position, Console.ForegroundColor, symbol);
		}

		private void WriteSymbolAtPlace(Vector2 position, ConsoleColor color, char symbol)
		{
			var cursorPosition = Console.GetCursorPosition();

			Console.SetCursorPosition(position.X, position.Y);

			WriteColoredSymbol(color, symbol);

			Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);
		}
	}
}
