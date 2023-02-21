using Cl9Backup.CLI.FakeApi.Models;
using System.Text.Json;

namespace Cl9Backup.CLI.FakeApi.Persistence
{
    public class UserProfileRepository
    {
        public UserProfileDto? GetUserProfile(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return default;

            var jsonUserProfile = File.ReadAllText("Persistence/user-profile.json");

            if (string.IsNullOrEmpty(jsonUserProfile))
                return default;

            return JsonSerializer.Deserialize<UserProfileDto>(jsonUserProfile);
        }
    }
}
