using Microsoft.AspNetCore.Mvc;

namespace Cl9Backup.CLI.FakeApi.Models
{
    public class AuthModel
    {
        [FromForm(Name = "Username")]
        public string UserName { get; set; } = default!;
        public string AuthType { get; set; } = default!;
        public string SessionKey { get; set; } = default!;
    }
}
