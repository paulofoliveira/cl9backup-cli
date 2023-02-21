using Cl9Backup.CLI.Models;
using System.Net.Http.Json;

namespace Cl9Backup.CLI.Infrastructure.Api
{
    public class Cl9BackupApiClient
    {
        private readonly HttpClient _client;
        private string _sessionKey = string.Empty;
        public Cl9BackupApiClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<LoginResponseDto?> Login(string userName, string password)
        {
            var form = new Dictionary<string, string>() { { "Username", userName }, { "AuthType", "Password" }, { "Password", password } };

            using var content = new FormUrlEncodedContent(form);
            using var response = await _client.PostAsync("/hybrid/session/start", content);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<LoginResponseDto>();

            // TODO: Melhorar retorno em caso de retorno diferente de 200.

            return default;
        }

        public async Task<UserProfileDto?> GetProfile(string userName, string sessionKey)
        {
            var form = new Dictionary<string, string>() { { "Username", userName }, { "AuthType", "SessionKey" }, { "SessionKey", sessionKey } };

            using var content = new FormUrlEncodedContent(form);
            using var response = await _client.PostAsync("/user/web/get-user-profile-and-hash", content);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<UserProfileDto>();

            // TODO: Melhorar retorno em caso de retorno diferente de 200.

            return default;
        }


    }
}
