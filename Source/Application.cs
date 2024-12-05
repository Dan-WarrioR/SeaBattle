﻿namespace Source
{
	public class Application
	{
		public void Launch()
		{
			InitializeConsoleSettings();

			DrawAvailableModes();

			var game = new Game(GetGameMode());

			while (!game.IsGameEnd())
			{
				game.ResetGame();
				game.PlayGameCycle();
				game.DrawRoundScore();
			}	
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