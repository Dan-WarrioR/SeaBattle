namespace Source.Users
{
	public class User
	{
		public string Name { get; private set; }

		public int WinCount {  get; set; }

		public int LosesCount { get; set; }

		public float WinRate => WinCount + LosesCount == 0 ? 0f : WinCount / (WinCount + LosesCount);

		public Player Player { get; private set; }

		public User(string name, Player player)
		{
			Name = name;

			Player = player;
		}

		public void ResetPlayer()
		{
			Player = new(Player.IsAi);
		}
	}
}
