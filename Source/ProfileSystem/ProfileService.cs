using Newtonsoft.Json;
using Source.Users;

namespace Source.ProfileSystem
{
	public class ProfileService
	{
		private const string FilePath = "profiles.json";

		public List<User> Profiles { get; private set; } = new();

		public ProfileService()
		{
			LoadAllProfiles();
		}

		private void LoadAllProfiles()
		{
			if (!File.Exists(FilePath))
			{
				return;
			}

			var json = File.ReadAllText(FilePath);

			Profiles = JsonConvert.DeserializeObject<List<User>>(json);
		}



		public void AddProfile(User user)
		{
			Profiles.Add(user);

			SaveProfiles();
		}

		public User? GetProfile(int index)
		{
			if (index < 0 || index >= Profiles.Count)
			{
				return null;
			}

			return Profiles[index];
		}

		public User? GetProfile(string name)
		{
			return Profiles.Find(profile => profile.Name == name);
		}	

		public void RemoveProfile(int index)
		{
			if (index < 0 || index >= Profiles.Count)
			{
				return;
			}

			Profiles.RemoveAt(index);
		}

		public void RemoveProfile(string name)
		{
			var user = GetProfile(name);
			
			if (user != null)
			{
				Profiles.Remove(user);
			}
		}

		public void SaveProfiles()
		{
			var json = JsonConvert.SerializeObject(Profiles, Formatting.Indented);

			File.WriteAllText(FilePath, json);
		}

		public void ClearProfiles()
		{
			if (File.Exists(FilePath))
			{
				File.Delete(FilePath);

				Profiles.Clear();
			}
		}
	}
}
