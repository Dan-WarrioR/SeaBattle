using Source.Users;
using Source.Configs;
using Source.MapGeneration;
using Source.Tools.Math;

namespace Source.Rendering
{
    public class Renderer
	{
		private const int SpaceBetweenMaps = 5;

		private bool FirstPlayerMapVisible => _player1.IsAi && (!_player2.IsAi || _player2.IsAi);
		private bool SecondPlayerMapVisible => _player2.IsAi && (!_player1.IsAi || _player1.IsAi);

		private Player _player1;
		private Player _player2;

		public Renderer(Player player1, Player player2)
		{
			_player1 = player1;
			_player2 = player2;
		}

		public void Draw()
		{
			Console.Clear();

			DrawMap(Vector2.Zero, FirstPlayerMapVisible, _player1.Map);
			DrawMap(new(_player2.Map.Size + SpaceBetweenMaps, 0), SecondPlayerMapVisible, _player2.Map); 

			DrawPlayerCursor(_player1.CurrentPosition);

			Vector2 cursorPosition = new(_player2.CurrentPosition.X + _player2.Map.Size + SpaceBetweenMaps, _player2.CurrentPosition.Y);

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
			Console.WriteLine($"\nLeft map ships - {_player1.ShipsCount} \nRight map ships - {_player2.ShipsCount}");
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
