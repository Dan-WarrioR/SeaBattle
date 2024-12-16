namespace Source.Users
{
	public class PlayerStats
	{
		public string Name { get; private set; }

		public int WinCount {  get; set; }

		public int LosesCount { get; set; }

		public float WinRate => WinCount + LosesCount == 0 ? 0f : WinCount / (WinCount + LosesCount);

		public bool IsAi { get; }

		public PlayerStats(string name, bool isAi)
		{
			Name = name;
			IsAi = isAi;
		}
		
		public override string ToString()
		{
			return $"{Name}\n" +
				$"\n{WinCount} - wins" +
				$"\n{LosesCount} - loses" +
				$"\n{WinRate * 100}% - win rate";
		}
	}
}
