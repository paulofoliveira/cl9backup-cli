using Microsoft.AspNetCore.Mvc;

namespace Cl9Backup.CLI.FakeApi.Models
{
    public class RunBackupRequestDto
    {
        [FromForm(Name = "TargetID")]
        public Guid Device { get; set; }
        public Guid Source { get; set; }
        public Guid Destination { get; set; }
        public string Options { get; set; } = default!;
        [FromForm(Name = "Username")]
        public string UserName { get; set; } = default!;
        public string AuthType { get; set; } = default!;
        public string SessionKey { get; set; } = default!;
    }
}
