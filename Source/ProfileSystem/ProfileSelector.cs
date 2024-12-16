using Source.Users;

namespace Source.ProfileSystem
{
	public class ProfileSelector
	{
		private readonly ProfileService _profileLoader;

		public ProfileSelector(ProfileService loader)
		{
			_profileLoader = loader;
		}

		public PlayerStats SelectProfile(List<int> reservedProfilesNumbers)
		{
			DrawProfiles(_profileLoader.Profiles, reservedProfilesNumbers);

			int lastCommandNumber = _profileLoader.Profiles.Count + 1;
			
			int commandNumber = GetValidNumberInput(lastCommandNumber, reservedProfilesNumbers);

			reservedProfilesNumbers.Add(commandNumber - 1);

			return commandNumber == lastCommandNumber ? CreateNewProfile() : _profileLoader.Profiles[commandNumber - 1];
		}

		private int GetValidNumberInput(int maxNumber, List<int> reservedNumbers)
		{
			do
			{
				Console.WriteLine($"Enter a number between 1 and {maxNumber} and click ENTER:");

				var input = Console.ReadLine();

				if (int.TryParse(input, out int number) &&
					number >= 1 &&
					number <= maxNumber &&
					!reservedNumbers.Contains(number - 1))
				{
					return number;
				}

				Console.WriteLine("Invalid input! Try again.");
			} while (true);
		}

		private string GetProfileName()
		{
			Console.WriteLine("Enter new user name:");

			string input;
			do
			{
				input = Console.ReadLine();
			} while (string.IsNullOrWhiteSpace(input));

			return input;
		}

		private bool ChooseInputHandling()
		{
			Console.WriteLine($"Choose input handling: " +
							  $"\n1 - Native input " +
							  $"\n2 - Random input");

			ConsoleKey key;
			do
			{
				key = Console.ReadKey(true).Key;
			} while (key != ConsoleKey.D1 && key != ConsoleKey.D2);

			return key == ConsoleKey.D2;
		}

		private PlayerStats CreateNewProfile()
		{
			string name = GetProfileName();
			bool isAi = ChooseInputHandling();

			var newUser = new PlayerStats(name, isAi);
			_profileLoader.AddProfile(newUser);

			return newUser;
		}

		private void DrawProfiles(List<PlayerStats> profiles, List<int> reservedNumbers)
		{
			Console.WriteLine("\n==================================================\n");

			for (int i = 0; i < profiles.Count; i++)
			{
				string endText = reservedNumbers.Contains(i) ? "(reserved)" : "";
				Console.WriteLine($"{i + 1} - {profiles[i].Name} {endText}");
			}

			Console.WriteLine($"{profiles.Count + 1} - Create new profile\n" +
							  "\n==================================================\n");
		}
	}
}
