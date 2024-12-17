namespace Source.Users
{
	public class Player
	{
		public PlayerStats Stats { get; private set; }

		public GamePlayer GamePlayer { get; private set; }

		public int Score { get; private set; } = 0;

		public Player(PlayerStats playerStats, GamePlayer gamePlayer)
		{
			Stats = playerStats;
			GamePlayer = gamePlayer;
		}

		public void IncreaseScore()
		{
			Score++;
		}

		public void Reset()
		{
			GamePlayer = new(GamePlayer.IsAi, GamePlayer.AiDifficulty);
		}

		public override string ToString()
		{
			return Stats.ToString();
		}
	}
}
