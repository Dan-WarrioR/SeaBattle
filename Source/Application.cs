namespace Source
{
	public class Application
	{
		private Game _game;

		public void Launch()
		{
			InitializeConsoleSettings();

			DrawAvailableModes();

			_game = new Game(GetGameMode());

			do
			{
				_game.ResetGame();

				_game.PlayGameCycle();
			} while (IsEndGame());	
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

		private void InitializeConsoleSettings()
		{
			Console.OutputEncoding = System.Text.Encoding.UTF8;
			Console.CursorVisible = false;
		}



		private bool IsEndGame()
		{
			return _game.Scores.firstPlayer < 3 && _game.Scores.secondPlayer < 3;
		}	



		private void DrawCongratulation()
		{
			Console.Clear();

			Console.WriteLine("Congratulations!!!");
			Console.WriteLine($"You win this game with {_game.Scores.firstPlayer} | {_game.Scores.secondPlayer} scores!");
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
	}
}