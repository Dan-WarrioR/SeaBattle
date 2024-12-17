namespace Source.Users
{
	public enum Difficulty
	{
		Easy,
		Medium,
		Hard,
		Impossible,
	}

	public class PlayerStats
	{
		public string Name { get; private set; }

		public int WinCount {  get; set; }

		public int LosesCount { get; set; }

		public float WinRate => WinCount + LosesCount == 0 ? 0f : MathF.Round(WinCount * 1f / (WinCount + LosesCount), 2);

		public bool IsAi { get; }

		public Difficulty Difficulty { get; private set; }

		public PlayerStats(string name, bool isAi, Difficulty difficulty = Difficulty.Easy)
		{
			Name = name;
			IsAi = isAi;
			Difficulty = difficulty;
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
