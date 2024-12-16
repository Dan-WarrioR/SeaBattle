using Source.ProfileSystem;
using Source.Users;

namespace Source.Core
{
    public class Application
    {
		private ProfileService _profileService;

		public void Launch()
        {
			Console.OutputEncoding = System.Text.Encoding.UTF8;
			Console.CursorVisible = false;

			var players = LoadPlayers();

			var game = new Game(players.player1, players.player2);

			game.Launch();

			SaveProfiles(players.player1, players.player2);
		}

		private (Player player1, Player player2) LoadPlayers()
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

			var player1 = new Player(player1Stats, new(player1Stats.IsAi));
			var player2 = new Player(player2Stats, new(player2Stats.IsAi));

			return (player1, player2);
		}

		private void SaveProfiles(Player player1, Player player2)
		{
			(var winner, var loser) = player1.Score > player2.Score ? (player1, player2) : (player2, player1);

			winner.Stats.WinCount++;
			loser.Stats.LosesCount++;

			_profileService.SaveProfiles();
		}
	}
}