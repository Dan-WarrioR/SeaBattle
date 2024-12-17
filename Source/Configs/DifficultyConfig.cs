using Source.Users;

namespace Source.Configs
{
	public class DifficultyConfig
	{
		public const int MediumShootChance = 50;
		public const int HardShootChance = 75;
		public const int ImpossibleShootChance = 100;

		public static int GetChance(Difficulty difficulty)
		{
			return difficulty switch
			{
				Difficulty.Medium => MediumShootChance,
				Difficulty.Hard => HardShootChance,
				Difficulty.Impossible => ImpossibleShootChance,
				_ => 0,
			};
		}
	}
}
