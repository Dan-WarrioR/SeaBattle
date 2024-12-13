using Source.ProfileSystem;
using Source.Users;

namespace Source.Core
{
	public class Game
	{
		private User _user1;
		private User _user2;

		private int _player1Score = 0;
		private int _player2Score = 0;

		private ProfileLoader _profileLoader;

		public void Launch()
		{
			SetupProfiles();

			while (!IsEndGame())
			{
				Round round = new(_user1.Player, _user2.Player);

				round.PlayRound();

				ProcessRoundResult(round.RoundResult);

				DrawRoundScore();
			}

			SaveProfiles();

			DrawEndGameScore();
		}

		private void SetupProfiles()
		{
			_profileLoader = new();

			var profileSelector = new ProfileSelector(_profileLoader);

			Console.Clear();
			Console.WriteLine("Choose 1 profile!");

			List<int> reservedNumbers = new();

			_user1 = profileSelector.SelectProfile(reservedNumbers);

			Console.Clear();
			Console.WriteLine("Choose 2 profile!");

			_user2 = profileSelector.SelectProfile(reservedNumbers);
		}

		private void ProcessRoundResult(RoundResult result)
		{
			if (result == RoundResult.Player1Win)
			{
				_player1Score++;

				return;
			}

			_player2Score++;

			_user1.ResetPlayer();
			_user2.ResetPlayer();
		}

		private void SaveProfiles()
		{
			UpdateProfilesInfo();

			_profileLoader.SaveProfiles();
		}

		private void UpdateProfilesInfo()
		{
			var winner = _player1Score > _player2Score ? _user1 : _user2;
			var loser = winner == _user1 ? _user2 : _user1;

			winner.WinCount++;
			loser.LosesCount++;
		}
		


		private bool IsEndGame()
		{
			return _player1Score >= 3 || _player2Score >= 3;
		}

		

		private void DrawRoundScore()
		{
			Console.Clear();

			Console.WriteLine($"\tCurrent Score: {_player1Score} | {_player2Score}");

			Thread.Sleep(5000);
		}

		private void DrawEndGameScore()
		{
			Console.Clear();

			Console.WriteLine($"{_user1.Name}\n" +
				$"\n{_user1.WinCount} - wins" +
				$"\n{_user1.LosesCount} - loses" +
				$"\n{_user1.WinRate * 100}% - win rate");

			Console.WriteLine($"\n{_user2.Name}\n" +
				$"\n{_user2.WinCount} - wins" +
				$"\n{_user2.LosesCount} - loses" +
				$"\n{_user2.WinRate * 100}% - win rate");
		}
	}
}
