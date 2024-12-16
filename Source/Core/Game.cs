using Source.ProfileSystem;
using Source.Users;

namespace Source.Core
{
	public class Game
	{
		private Player _player1;
		private Player _player2;

		private ProfileService _profileService;

		public void Launch()
		{
			SetupProfiles();

			while (!IsEndGame())
			{
				Round round = new(_player1.GamePlayer, _player2.GamePlayer);

				round.PlayRound();

				ProcessRoundResult(round.RoundResult);

				DrawRoundScore();
			}

			SaveProfiles();

			DrawEndGameScore();
		}

		private void SetupProfiles()
		{
			_profileService = new();

			var profileSelector = new ProfileSelector(_profileService);

			Console.Clear();
			Console.WriteLine("Choose 1 profile!");

			List<int> reservedProfilesNumbers = new();

			var player1Stats = profileSelector.SelectProfile(reservedProfilesNumbers);

			Console.Clear();
			Console.WriteLine("Choose 2 profile!");

			var player2Stats = profileSelector.SelectProfile(reservedProfilesNumbers);

			_player1 = new(player1Stats, new(player1Stats.IsAi));
			_player2 = new(player2Stats, new(player2Stats.IsAi));
		}

		private void ProcessRoundResult(RoundResult result)
		{
			var player = result == RoundResult.Player1Win ? _player1 : _player2;

			player.IncreaseScore();
		}

		private void SaveProfiles()
		{
			UpdateProfilesInfo();

			_profileService.SaveProfiles();
		}

		private void UpdateProfilesInfo()
		{
			(var winner, var loser) = _player1.Score > _player2.Score ? (_player1, _player2) : (_player2, _player1);

			winner.Stats.WinCount++;
			loser.Stats.LosesCount++;
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
			Console.WriteLine(_player2.ToString());
		}
	}
}
