using Cl9Backup.CLI.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace Cl9Backup.CLI.Infrastructure.Api
{
    public class Cl9BackupApiClient
    {
        private readonly HttpClient _client;
        private JsonSerializerOptions _serializerOptions;
        public Cl9BackupApiClient(HttpClient client, JsonSerializerOptions serializerOptions)
        {
            _client = client;
            _serializerOptions = serializerOptions;
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

        public async Task<RunBackupResponseDto?> RunBackup(string userName, string sessionKey, Guid destination, Guid source, Guid device)
        {
            var runBackupRequestOptions = new RunBackupRequestOptionsDto()
            {
                SkipAlreadyRunning = true
            };

            var form = new Dictionary<string, string>() {
                { "Username", userName },
                { "AuthType", "SessionKey" },
                { "SessionKey", sessionKey },
                { "TargetID", device.ToString() },
                { "Source", source.ToString() },
                { "Destination", destination.ToString() },
                { "Options", JsonSerializer.Serialize(runBackupRequestOptions, _serializerOptions) }
            };

            using var content = new FormUrlEncodedContent(form);
            using var response = await _client.PostAsync("/user/web/dispatcher/run-backup-custom", content);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<RunBackupResponseDto>();

            // TODO: Melhorar retorno em caso de retorno diferente de 200.

            return default;
        }


    }
}
