using Source.Users;

namespace Source.Core
{
	public class Game
	{
		private Player _player1;
		private Player _player2;

		public Game(Player player1, Player player2)
		{
			_player1 = player1;
			_player2 = player2;
		}

		public void Launch()
		{
			while (!IsEndGame())
			{
				Round round = new(_player1.GamePlayer, _player2.GamePlayer);

				round.PlayRound();

				ProcessRoundResult(round.RoundResult);

				DrawRoundScore();

				ResetsPlayers();
			}

			DrawEndGameScore();
		}

		private void ProcessRoundResult(RoundResult result)
		{
			var player = result == RoundResult.Player1Win ? _player1 : _player2;

			player.IncreaseScore();
		}

		private void ResetsPlayers()
		{
			_player1.Reset();
			_player2.Reset();
		}
		


		private bool IsEndGame()
		{
			return _player1.Score >= 3 || _player2.Score >= 3;
		}

		

		private void DrawRoundScore()
		{
			Console.Clear();

			Console.WriteLine($"\tCurrent Score: {_player1.Score} | {_player2.Score}");

			Thread.Sleep(5000);
		}

		private void DrawEndGameScore()
		{
			Console.Clear();

			Console.WriteLine(_player1.ToString());
			Console.WriteLine();
			Console.WriteLine(_player2.ToString());
		}
	}
}
