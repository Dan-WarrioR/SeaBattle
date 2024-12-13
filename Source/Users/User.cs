namespace Source.Users
{
	public class User
	{
		public string Name { get; private set; }

		public int WinCount {  get; private set; }

		public int LosesCount { get; private set; }

		public float WinRate => WinCount + LosesCount == 0 ? 0f : WinCount / (WinCount + LosesCount);

		public Player Player { get; private set; }

		public User(string name, Player player)
		{
			Name = name;

			Player = player;
		}
	}
}
