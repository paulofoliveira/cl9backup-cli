using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Cl9Backup.CLI.FakeApi.Models
{
    public class LoginRequestDto
    {
        [FromForm(Name = "Username")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string UserName { get; set; } = default!;
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string AuthType { get; set; } = default!;
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Password { get; set; } = default!;
    }
}
