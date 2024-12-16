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

		public User SelectProfile(List<int> reservedNumbers)
		{
			DrawProfiles(_profileLoader.Profiles, reservedNumbers);

			int lastCommandNumber = _profileLoader.Profiles.Count + 1;
			
			int commandNumber = CalculateUserInput(lastCommandNumber, reservedNumbers);

			reservedNumbers.Add(commandNumber - 1);

			if (commandNumber == lastCommandNumber)
			{
				var data = EnterNewUserSettings();
				var newUser = new User(data.name, new Player(data.isAi));
				_profileLoader.AddProfile(newUser);

				return newUser;
			}
			else
			{
				return _profileLoader.Profiles[commandNumber - 1];
			}
		}

		private int CalculateUserInput(int maxNumber, List<int> reservedNumbers)
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

		private (string name, bool isAi) EnterNewUserSettings()
		{
			Console.WriteLine("==================================================");
			Console.WriteLine("Write new user name:");

			string name;

			do
			{
				name = Console.ReadLine();
			} while (string.IsNullOrWhiteSpace(name));

			Console.WriteLine($"Choose input handling: " +
							  $"\n1 - Native input " +
							  $"\n2 - Random input");

			bool isAi = false;
			ConsoleKey key;

			do
			{
				key = Console.ReadKey(true).Key;
			} while (key != ConsoleKey.D1 && key != ConsoleKey.D2);

			if (key == ConsoleKey.D2)
			{
				isAi = true;
			}

			return (name, isAi);
		}

		private void DrawProfiles(List<User> profiles, List<int> reservedNumbers)
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
