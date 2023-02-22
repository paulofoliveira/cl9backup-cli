using Cl9Backup.CLI.Shared;
using System.Text.Json;

namespace Cl9Backup.CLI.FakeApi.Persistence
{
    public class UserProfileRepository
    {
        public ProfileResponseDto? GetUserProfile(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return default;

            var jsonProfile = File.ReadAllText("Persistence/user-profile.json");

            if (string.IsNullOrEmpty(jsonProfile))
                return default;

            var dto = JsonSerializer.Deserialize<ProfileResponseDto>(jsonProfile);

            if (dto == null)
                return default;

            dto.Status = 200;
            dto.Message = "OK";

            return dto;
        }
    }
}
