using Source.Configs;
using Source.MapGeneration;
using Source.Tools.Math;

namespace Source.Rendering
{
	public class Renderer
	{
		private const int SpaceBetweenMaps = 5;

		private Map _firstPlayerMap;
		private Map _secondPlayerMap;

		public Renderer(Map firstPlayerMap, Map secondPlayerMap)
		{
			_firstPlayerMap = firstPlayerMap;
			_secondPlayerMap = secondPlayerMap;
		}

		//////////

		public void Draw(Vector2 playerCursor)
		{
			Console.Clear();

			DrawMap(Vector2.Zero, false, _firstPlayerMap);
			DrawMap(new(_secondPlayerMap.Size + SpaceBetweenMaps, 0), true, _secondPlayerMap);

			DrawPlayerCursor(playerCursor);

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
			Console.WriteLine($"\nEnemy ships - {_firstPlayerMap.ShipsCount} \nMy ships - {_secondPlayerMap.ShipsCount}");
		}

		public void DrawEndGameText(bool playerWin)
		{
			Console.Clear();

			Console.WriteLine(playerWin ? "You Won!" : "You Lose!");
			Console.WriteLine("Play again? (Y / N)");
		}

		//////////

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

		//////////

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
